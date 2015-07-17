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
using DM.Armory.Model;

namespace DM.Armory.View
{
    /// <summary>
    /// Interaction logic for BuildingView.xaml
    /// </summary>
    public partial class BuildingView
    {

        public BuildingView(BuildingViewModel bVM) 
        {
            InitializeComponent();
            DataContext = bVM;
            DescriptionBox.Document = DM.Armory.BL.EugenStringConverter.MakeFlowDocument(bVM.Description, Brushes.LightGreen);
            
            foreach (AoATurretViewModel turretVM in bVM.Turrets) 
            {
                foreach (AoAWeaponViewModel weaponVM in turretVM.Weapons) 
                {
                    WeaponView view = new WeaponView(weaponVM);
                    WeaponsList.Children.Add(view);
                }
            }

            foreach (AoAResearchViewModel up in bVM.Researches)
            {
                IconView view = new IconView(up);
                ResearchesList.Children.Add(view);
            }
            /*
            foreach (AoAUnitViewModel unit in bVM.Children)
            {
                IconView view = new IconView(unit);
                UpgradesList.Children.Add(view);
            }*/
        }
    }



}
