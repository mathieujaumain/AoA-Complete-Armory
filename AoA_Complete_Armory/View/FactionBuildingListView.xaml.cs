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
using DM.Armory.ViewModel;

namespace DM.Armory.View
{
    /// <summary>
    /// Interaction logic for FactionBuildingListView.xaml
    /// </summary>
    public partial class FactionBuildingListView : UserControl
    {


        public FactionBuildingListView(FactionViewModel model)
        {
            InitializeComponent();

            foreach (BuildingViewModel building in model.Buildings)
            {
                BuildingListView view = new BuildingListView(building);
                BuildingsListing.Children.Add(view);
            }
        }
    }
}
