using System;
using System.IO;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.Armory.Model;
using DM.Armory.BL;
using System.Drawing;

namespace DM.Armory.ViewModel
{
    public class AoAUnitViewModel
    {
        private AoAUnit _unit;
        private List<AoATurretViewModel> _turrets = new List<AoATurretViewModel>();
        private List<AoAResearchViewModel> _upgrades = new List<AoAResearchViewModel>();
        private List<AoAUnitViewModel> _children = new List<AoAUnitViewModel>();

        public AoAUnitViewModel(AoAUnit unit) 
        {
            _unit = unit;
            foreach (AoATurret turret in _unit.Turrets)
                _turrets.Add(new AoATurretViewModel(turret));

            foreach (AoAResearch up in _unit.Upgrades)
                _upgrades.Add(new AoAResearchViewModel(up));
            foreach (AoAUnit child in _unit.Children)
                _children.Add(new AoAUnitViewModel(child));
        }

        public string           Name                { get { return _unit.Name; } }
        public string           Description         { get { return _unit.Description; } }
        public int              CashCost            { get { return _unit.CashCost; } }
        public int              AluminiumCost       { get { return _unit.AluminiumCost; } }
        public int              ElectricityCost     { get { return _unit.ElectricityCost; } }
        public int              RareEarthCost       { get { return _unit.RareEarthCost; } }
        public ObjectType       Type                { get { return _unit.Type; } }
        public FactionEnum      Faction             { get { return _unit.Faction; } }
        public Bitmap           Icon                { get { return _unit.Icon; } }
        public int              nbrPOW              { get { return _unit.nbrPOW; } }
        public float            ViewRange           { get { return _unit.ViewRange; } }
        public bool             CanSpot             { get { return _unit.CanSpotStealthyUnits; } }
        public float            Health              { get { return _unit.Health; } }
        public bool             Stealth             { get { return _unit.IsStealthy; } }
        public int              TransportSlots      { get { return _unit.TransportSlot; } }
        public int              SeatsTaken          { get { return _unit.SlotTaken; } }
        public int              Armor               { get { return _unit.Armor; } }
        public float            Speed               { get { return _unit.Speed; } }
        public float            OnRoadSpeed         { get { return _unit.OnRoadSpeed; } }
        public bool             IsRevealedOnFiring  { get { return _unit.AutoReveal; } }
            
        public List<AoATurretViewModel> Turrets { get { return _turrets; } }
        public List<AoAResearchViewModel> Upgrades { get { return _upgrades; } }
        public List<AoAUnitViewModel> Children { get { return _children; } }

        //public AoAVehicle       Vehicle             { get { return _unit.Vehicle; } }
    }

    public class Int2VisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int integer = (int)value;
            return integer > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 1;
        }
    }

    public class Bool2VisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolean = (bool)value;
            return boolean ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 1;
        }
    }

    public class Int2BoolConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int integer = (int)value;
            return integer > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 1;
        }
    }

    public class WPFBitmapConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                MemoryStream ms = new MemoryStream();
                ((System.Drawing.Bitmap)value).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();

                return image;
            } 
            return null;
        
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class EugenStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string description = (string)value;

            return description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 1;
        }
    }


    public class Description2ToolTipConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string description = (string)value;
            string[] parts = description.Split(new string[] { "Available" }, StringSplitOptions.RemoveEmptyEntries);
            
            return parts.Length > 0 ? parts[0].Trim(): description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 1;
        }
    }


    public class Bool2StringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = (bool)value;
            return val ? "true" : "false";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 1;
        }
    }
}
