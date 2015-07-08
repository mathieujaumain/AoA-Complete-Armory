using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin;
using DM.Armory.Model;
using DM.Armory.ViewModel;
using DM.Armory.BL;

namespace DM.Armory.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static TradManager techDic;

        public MainWindow()
        {
            InitializeComponent();
            // Load everything !!
            //Loaded += delegate { Test(); };
            Loaded += delegate { StartUp(); };
        }

        public void Test()
        {
            string ndffile = @"C:\Users\mja\Documents\perso\mods\NDF_Win.dat";
            string EVERYTHING = @"pc\ndf\patchable\gfx\everything.ndfbin";

            string trans = @"C:\Users\mja\Documents\perso\mods\ZZ_Win.dat";
            string transFile = @"pc\localisation\us\localisation\unites.dic";
            string techTrans = @"pc\localisation\us\localisation\techno.dic";

            string zz4 = @"C:\Users\mja\Documents\perso\mods\ZZ_4.dat";
            string ICON_PACKAGE = @"pc\texture\pack\commoninterface.ppk";

            //load ndffile
            EdataManager edat = new EdataManager(ndffile);
            edat.ParseEdataFile();
            NdfbinManager everything = edat.ReadNdfbin(EVERYTHING);
            NdfObject losatndf = everything.GetClass("TUniteDescriptor").Instances[26]; // caserne CS 26


            //load unite dic
            edat = new EdataManager(trans);
            edat.ParseEdataFile();
            TradManager dic = edat.ReadDictionary(transFile);

            //load tech dic
            edat = new EdataManager(trans);
            edat.ParseEdataFile();
            TradManager tech = edat.ReadDictionary(techTrans);
            techDic = tech;

            //load iconspack
            edat = new EdataManager(zz4);
            edat.ParseEdataFile();
            EdataManager iconspack = edat.ReadPackage(ICON_PACKAGE);

            //Load Object
            AoAGameObject losatObject = new AoAGameObject();
            if (losatObject.LoadData(losatndf, dic, iconspack))
            {
                losatObject.Icon.Save("Losat.png");
            }

            // if object is unit...
            if (losatObject.Type != ObjectType.Building) 
            {
                AoAUnit losat = new AoAUnit(losatObject);
                losatObject = null;
                if (losat.LoadData(losatndf, dic, iconspack))
                {
                        ViewModel.AoAUnitViewModel model = new ViewModel.AoAUnitViewModel(losat);
                        UnitView view = new UnitView(model);
                        CartelTab.Content = view;            
                }
            }
            else 
            {
                AoABuilding building = new AoABuilding(losatObject);
                losatObject = null;
                if (building.LoadData(losatndf, dic, iconspack))
                {
                    ViewModel.BuildingViewModel model = new ViewModel.BuildingViewModel(building);
                    BuildingListView view = new BuildingListView(model);
                    CartelTab.Content = view;
                }
            }          
        }

        public async void StartUp()
        {
            InitializationController init = new InitializationController();
            var controller = await this.ShowProgressAsync("loading last update", "starting...");
            await Task.Delay(3000);
            init.OnLoadingUpdate += delegate(object sender, string mess) { controller.SetMessage(mess); };
            if ( await init.LoadData())
            {
                await Task.Delay(3000);
                List<BuildingViewModel> usmodels = new List<BuildingViewModel>();
                List<BuildingViewModel> carmodels = new List<BuildingViewModel>();
                List<BuildingViewModel> chimmodels = new List<BuildingViewModel>();
                //Fill the forms
                //US
                foreach(AoABuilding b in init.UsBuildings)
                {
                    BuildingViewModel buioding = new BuildingViewModel(b);
                    usmodels.Add(buioding);
                }
                FactionBuildingListView Usview = new FactionBuildingListView(new FactionViewModel(usmodels));
                UsGrid.Children.Add(Usview);

                //Cartel
                foreach (AoABuilding b in init.CartelBuildings)
                {
                    BuildingViewModel buioding = new BuildingViewModel(b);
                    carmodels.Add(buioding);
                }
                FactionBuildingListView cartelview = new FactionBuildingListView(new FactionViewModel(carmodels));
                CartelGrid.Children.Add(cartelview);

                //Chimera
                foreach (AoABuilding b in init.ChimeraBuildings)
                {
                    BuildingViewModel buioding = new BuildingViewModel(b);
                    chimmodels.Add(buioding);
                }
                FactionBuildingListView chimview = new FactionBuildingListView(new FactionViewModel(chimmodels));
                ChimeraGrid.Children.Add(chimview);

                await controller.CloseAsync();
            }
            else { Console.Out.WriteLine(DateTime.Now.ToShortDateString() + " : loading last update failed."); await controller.CloseAsync(); }

        }
    }


    
}
