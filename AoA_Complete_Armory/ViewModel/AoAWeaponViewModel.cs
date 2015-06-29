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
    }
}
