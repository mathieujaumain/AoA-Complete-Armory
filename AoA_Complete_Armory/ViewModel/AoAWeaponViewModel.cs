using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.Armory.Model;
using System.Windows.Input;
using DM.Armory.View.Windows;
using DM.Armory.View;

namespace DM.Armory.ViewModel
{
    public class AoAWeaponViewModel
    {
        private AoAWeapon _Weapon;

        private ICommand _visualizeRoFCommmand;

        public AoAWeaponViewModel(AoAWeapon weapon)
        {
            _Weapon = weapon;
            _visualizeRoFCommmand = new VisualizeRofCommand(this);
        }

        public AoAWeapon Weapon { get { return _Weapon; } }
        public string Name { get { return _Weapon.Name; } }
        public float GroundRange { get { return _Weapon.GroundRange; } }
        public float Alpha { get { return _Weapon.Alpha; } }
        public float PowGen { get { return _Weapon.PoWGen; } }
        public float VHARange { get { return _Weapon.VHARange; } }
        public float VLARange { get { return _Weapon.VLARange; } }
        public float Splash { get { return _Weapon.Splash; } }
        public long Sustained { get { return (long)_Weapon.Sustained; }}
        public float AmbushMultiplier { get { return _Weapon.AmbushMultiplier; } }
        public bool IsSilenced { get { return _Weapon.IsSilenced; } }
        public float SupressDamages { get { return _Weapon.SupressDamages; } }

        public ICommand VisualizeRofCommand { get { return _visualizeRoFCommmand; } }
    }


    public class VisualizeRofCommand:ICommand
    {
        private AoAWeaponViewModel _weapon;

        public VisualizeRofCommand(AoAWeaponViewModel weapon)
        {
            _weapon = weapon;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            List<AoAWeaponViewModel> list = new List<AoAWeaponViewModel>();
            list.Add(_weapon);
            WeaponGraphViewModel model = new WeaponGraphViewModel(list);
            SecondaryWindow win = new SecondaryWindow();
            win.Closing += delegate { model.StopCommand.Execute(null); };
            win.Title = "Rate of fire of " + _weapon.Name;
            GraphView graph = new GraphView(model);
            
            win.ContentGrid.Children.Add(graph);
            win.Loaded += delegate
            {
                graph.Width = win.Width;
                graph.Height = win.Height;
            };
            win.Show();
        }
    }
}
