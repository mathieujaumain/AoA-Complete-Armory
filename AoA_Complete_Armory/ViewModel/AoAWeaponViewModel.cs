using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.Armory.Model;

namespace DM.Armory.ViewModel
{
    public class AoAWeaponViewModel
    {
        private AoAWeapon _Weapon;

        public AoAWeaponViewModel(AoAWeapon weapon)
        {
            _Weapon = weapon;
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
    }
}
