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
using DM.Armory.View.Windows;

namespace DM.Armory.View
{
    /// <summary>
    /// Interaction logic for UnitIconView.xaml
    /// </summary>
    public partial class IconView : UserControl
    {
        private AoAUnitViewModel unit;
        private BuildingViewModel building;
        private AoAResearchViewModel research;

        public IconView()
        {
            InitializeComponent();
        }

        public IconView(AoAUnitViewModel model)
        {
            InitializeComponent();
            unit = model;
            DataContext = model;
            Tile.Click += UnitOpen;
            
        }

        private void UnitOpen(object sender, RoutedEventArgs e)
        {
            SecondaryWindow window = new SecondaryWindow();
            window.ContentGrid.Children.Add(new UnitView(unit));
            window.Title = unit.Name;
            window.Show();
        }

        public IconView(AoAResearchViewModel model)
        {
            InitializeComponent();
            DataContext = model;
            research = model;
            //Tile.Click += ResearchOpen;
        }

        private void ResearchOpen(object sender, RoutedEventArgs e)
        {

        }

        public IconView(BuildingViewModel model)
        {
            InitializeComponent();
            DataContext = model;
            building = model;
            //this.MouseUp += BuildingOpen;
            Tile.Click += BuildingOpen;
        }

        private void BuildingOpen(object sender, RoutedEventArgs e)
        {
            SecondaryWindow window = new SecondaryWindow();
            window.ContentGrid.Children.Add(new BuildingView(building));
            window.Title = building.Name;
            window.Show();
        }
    }
}
