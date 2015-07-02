using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using DM.Armory.ViewModel;

namespace DM.Armory.View
{
    public class AoATile:Tile
    {
        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register("BackgroundImage",
                                        typeof(Bitmap),
                                        typeof(AoATile),
                                        new PropertyMetadata(default(bool), BackgroundImagePropertyChangedCallback));

        private static void BackgroundImagePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var tile = (Tile)dependencyObject;
            if(e.NewValue != null)
                if (e.OldValue != e.NewValue)
                {
                    WPFBitmapConverter converter = new WPFBitmapConverter();
                    Bitmap bit = e.NewValue as Bitmap;
                    tile.Background =  new ImageBrush((BitmapImage)converter.Convert(bit, typeof(BitmapImage), null, null));
                }
        }

        public Bitmap BackgroundImage
        {
            get { return (Bitmap)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }


    }
}
