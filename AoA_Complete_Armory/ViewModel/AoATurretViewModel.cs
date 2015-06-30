using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.Armory.Model;

namespace DM.Armory.ViewModel
{
    public class AoATurretViewModel
    {
        private AoATurret _turret;
        private List<AoAWeaponViewModel> _weapons = new List<AoAWeaponViewModel>();

        public AoATurretViewModel(AoATurret turret)
        {
                _turret = turret;
                foreach (AoAWeapon weapon in _turret.Weapons)
                    _weapons.Add(new AoAWeaponViewModel(weapon));
        }

        public float TraverseSpeed { get { return _turret.TraverseSpeed; } }
        public List<AoAWeaponViewModel> Weapons { get { return _weapons; } }
    }
}
