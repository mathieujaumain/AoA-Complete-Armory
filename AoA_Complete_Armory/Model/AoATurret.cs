using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Armory.Model
{
    public class AoATurret : IUpdatable
    {
        private List<AoAWeapon> _Weapons = new List<AoAWeapon>();

        public List<AoAWeapon> Weapons
        {
            get { return _Weapons; }
            set { _Weapons = value; }
        }

       // Ammo ?

    }
}
