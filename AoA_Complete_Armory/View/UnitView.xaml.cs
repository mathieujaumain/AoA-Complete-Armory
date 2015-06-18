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
        }

        public UnitView()
        {
            InitializeComponent();
        }
    }
}
