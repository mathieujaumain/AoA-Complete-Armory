using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private SettingsFlyout _flyout;

        public MainWindow()
        {
            InitializeComponent();
            // Load everything !!
            //Loaded += delegate { Test(); };
            //Loaded += delegate { new Thread(StartUp).Start(); };
            Loaded += MainWindow_Loaded;
            Factions.UsTile.Click += delegate { TabControl.SelectedItem = USTab; };
            Factions.CartelTile.Click += delegate { TabControl.SelectedItem = CartelTab; };
            Factions.ChimeraTile.Click += delegate { TabControl.SelectedItem = ChimeraTab; };
            _flyout = new SettingsFlyout();
            _flyout.OnRequestReloading += _flyout_OnRequestReloading;
            FlyoutControl.Items.Add(_flyout);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        async void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            MetroDialogSettings set = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Sorry",
            };

            MessageDialogResult res = await this.ShowMessageAsync("Exception", ex.Message, MessageDialogStyle.Affirmative, set);
            if (res == MessageDialogResult.Affirmative)
            {
                this.Close();
            }
        }

       

        public async Task<bool> LoadLastUpdateAndFill(ProgressDialogController controller)
        {
            DateTime start = DateTime.Now;
            InitializationController init = new InitializationController();
            
            await Task.Delay(2000);
            init.OnLoadingUpdate += delegate(object sender, string mess) { controller.SetMessage(mess); };
            if ( init.LoadData())
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.Title += " | Game version = " + init.Version;
                });

                List<BuildingViewModel> usmodels = new List<BuildingViewModel>();
                List<BuildingViewModel> carmodels = new List<BuildingViewModel>();
                List<BuildingViewModel> chimmodels = new List<BuildingViewModel>();
                //Fill the forms
                //US
                foreach (AoABuilding b in init.UsBuildings)
                {
                    BuildingViewModel buioding = new BuildingViewModel(b);
                    usmodels.Add(buioding);
                }
                
                this.Dispatcher.Invoke(() =>
                {
                    FactionBuildingListView Usview = new FactionBuildingListView(new FactionViewModel(usmodels));
                    UsGrid.Children.Clear();
                    UsGrid.Children.Add(Usview);
                });
                init.UsBuildings.Clear();
            
                

                //Cartel
                foreach (AoABuilding b in init.CartelBuildings)
                {
                    BuildingViewModel buioding = new BuildingViewModel(b);
                    carmodels.Add(buioding);
                }
                
                this.Dispatcher.Invoke(() =>
                {
                    FactionBuildingListView cartelview = new FactionBuildingListView(new FactionViewModel(carmodels));
                    CartelGrid.Children.Clear();
                    CartelGrid.Children.Add(cartelview);
                });
                init.CartelBuildings.Clear();
                    
                

                //Chimera
                foreach (AoABuilding b in init.ChimeraBuildings)
                {
                    BuildingViewModel buioding = new BuildingViewModel(b);
                    chimmodels.Add(buioding);
                }
                
                this.Dispatcher.Invoke(() =>
                {
                    FactionBuildingListView chimview = new FactionBuildingListView(new FactionViewModel(chimmodels));
                    ChimeraGrid.Children.Clear();
                    ChimeraGrid.Children.Add(chimview);
                });

                init.ChimeraBuildings.Clear();
                controller.SetMessage("finished, time taken : " + (DateTime.Now - start).TotalSeconds);
                await Task.Delay(2000);
                
                return true;
            }
            else
            {
                init.Clear(); Console.Out.WriteLine(DateTime.Now.ToShortDateString() + " : loading last update failed."); 
                controller.SetMessage("loading failed...");
                await Task.Delay(2000); 
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _flyout.IsOpen = !_flyout.IsOpen;
        }


        async void _flyout_OnRequestReloading(object sender, EventArgs e)
        {
            ProgressDialogController controller = await this.ShowProgressAsync("loading last update", "starting...");
            Thread thread = new Thread(async () =>
            {
                await LoadLastUpdateAndFill(controller);
                await controller.CloseAsync();
            });

            thread.Start();
                                                                                           
        }


        async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressDialogController controller = await this.ShowProgressAsync("loading last update", "starting...");
            Thread thread = new Thread(async () =>
            {
                await LoadLastUpdateAndFill(controller);
                await controller.CloseAsync();
            });

            thread.Start();

        }

    }


    
}
