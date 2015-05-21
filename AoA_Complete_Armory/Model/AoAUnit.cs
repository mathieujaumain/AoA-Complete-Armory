using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Armory.Model
{
    public class AoAUnit : AoAGameObject, IUpdatable
    {

        private List<AoATurret> _Turrets = new List<AoATurret>();
        private List<AoAUnit> _Children = new List<AoAUnit>();
        private List<AoAUnit> _Parents = new List<AoAUnit>();

        public List<AoATurret> Turrets
        {
            get { return _Turrets; }
            set { _Turrets = value; }
        }

        public AoAVehicle Vehicle { get; set; }
    }
}
