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
using MahApps.Metro.Controls;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin;
using DM.Armory.Model;

namespace DM.Armory.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            // Load everything !!
            Loaded += delegate { Test(); };
        }

        public void Test()
        {
            string ndffile = @"C:\Users\mja\Documents\perso\mods\NDF_Win.dat";
            string EVERYTHING = @"pc\ndf\patchable\gfx\everything.ndfbin";

            string trans = @"C:\Users\mja\Documents\perso\mods\ZZ_Win.dat";
            string transFile = "pc\\localisation\\us\\localisation\\unites.dic";

            string zz4 = @"C:\Users\mja\Documents\perso\mods\ZZ_4.dat";
            string ICON_PACKAGE = @"pc\texture\pack\commoninterface.ppk";

            //load ndffile
            EdataManager edat = new EdataManager(ndffile);
            edat.ParseEdataFile();
            NdfbinManager everything = edat.ReadNdfbin(EVERYTHING);
            NdfObject losatndf = everything.GetClass("TUniteDescriptor").Instances[1];


            //load unite dic
            edat = new EdataManager(trans);
            edat.ParseEdataFile();
            TradManager dic = edat.ReadDictionary(transFile);

            //load iconspack
            edat = new EdataManager(zz4);
            edat.ParseEdataFile();
            EdataManager iconspack = edat.ReadPackage(ICON_PACKAGE);

            AoAGameObject losatObject = new AoAGameObject();
            if (losatObject.LoadData(losatndf, dic, iconspack))
            {
                losatObject.Icon.Save("Losat.png");
            }

            // if object is unit...
            // if object is building...
        }
    }


    
}
