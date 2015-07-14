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
using System.Windows.Shapes;
using DM.Armory.Model;
using DM.Armory.ViewModel;
using DM.Armory.BL;

namespace DM.Armory.View
{
    /// <summary>
    /// Interaction logic for UnitWindow.xaml
    /// </summary>
    public partial class UnitView
    {
        public UnitView(AoAUnitViewModel unitVM) 
        {
            InitializeComponent();
            DataContext = unitVM;
            DescriptionBox.Document = DM.Armory.BL.EugenStringConverter.MakeFlowDocument(unitVM.Description, Brushes.LightGreen);

            foreach (AoATurretViewModel turretVM in unitVM.Turrets) 
            {
                foreach (AoAWeaponViewModel weaponVM in turretVM.Weapons) 
                {
                    WeaponView view = new WeaponView(weaponVM);
                    WeaponsList.Children.Add(view);
                }
            }

            foreach (AoAResearchViewModel up in unitVM.Upgrades)
            {
                IconView view = new IconView(up);
                ResearchesList.Children.Add(view);
            }

            foreach(AoAUnitViewModel unit in unitVM.Children)
            {
                IconView view = new IconView(unit);
                UpgradesList.Children.Add(view);
            }
        }

        public UnitView()
        {
            InitializeComponent();
        }
    }
}
