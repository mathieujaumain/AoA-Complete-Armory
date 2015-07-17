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
    /// Interaction logic for BuildingView.xaml
    /// </summary>
    public partial class BuildingListView
    {
        public BuildingListView()
        {
            InitializeComponent();
        }

        public BuildingListView(BuildingViewModel data) 
        {
            InitializeComponent();
            DataContext = data;

            IconView bView = new IconView(data);
            Building.Children.Add(bView);

            if (data.Units.Count <= 0)
            {
                UnitBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            foreach(AoAUnitViewModel unit in data.Units){
                IconView view = new IconView(unit);
                UnitsList.AddATile(view);
                //UnitsList.Children.Add(view);
            }

            if (data.Researches.Count <= 0)
            {
                ResearchBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            foreach (AoAResearchViewModel res in data.Researches)
            {
                IconView view = new IconView(res);
                ResearchesList.AddATile(view);
                //ResearchesList.Children.Add(view);
            }
        }
    }
}
