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

namespace DM.Armory.View
{
    /// <summary>
    /// Interaction logic for TileMatrix.xaml
    /// </summary>
    public partial class TileMatrix : UserControl
    {
        private int x = 0, y = 0;
        public static int MAX_X = 4, MAX_Y = 5; 

        public TileMatrix()
        {
            InitializeComponent();
        }

        public void AddATile(UIElement view)
        {
            if (y < MAX_Y)
            {
                Grid.SetColumn(view, x);
                Grid.SetRow(view, y);

                Matrix.Children.Add(view);

                x += 1;

                if (x >= MAX_X)
                {
                    x = 0;
                    y += 1;
                }
            }
        }
    }
}
