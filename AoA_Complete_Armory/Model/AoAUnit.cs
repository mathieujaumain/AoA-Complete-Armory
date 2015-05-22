using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Armory.Model
{
    public class AoAUnit : AoAGameObject, IUpdatable
    {
        #region Ndf Constants
        public static string NDF_CLASS_NAME = "TUniteDescriptor";
        public static string UNIT_TYPE_PATH = "Modules.TypeUnit.Default";
        public static string WEAPON_PATH = "Modules.WeaponManager.Default";
        public static string VISIBILITY_PATH = "Modules.Visibility.Default";
        public static string DAMMAGE_PATH = "Modules.Dammages.Default";
        public static string TRANSPORTABLE_PATH = "Modules.Transportable.Default";
        public static string HARVESTER_PATH = "Modules.Harvester.Default";
        public static string POW_PATH = "Modules.PilotGenerator.Default";
        public static string TRANSPORTER_PATH = "Modules.Transporter.Default";
        public static string SKILLS_PATH = "Modules.Capacite.Default";
        public static string PRODUCTION_PATH = "Modules.Production.Default";
        public static string SCANNER_CONFIG_PATH = "Modules.ScannerConfiguration.Default";
        #endregion

        private object _Lock = new object();
        private List<AoAUppgrade> _Uppgrades = new List<AoAUppgrade>();
        private List<AoATurret> _Turrets = new List<AoATurret>();
        private List<AoAUnit> _Children = new List<AoAUnit>();
        private List<AoAUnit> _Parents = new List<AoAUnit>();

        public List<AoATurret> Turrets
        {
            get { lock(_Lock) return _Turrets; }
            set { lock(_Lock) _Turrets = value; }
        }

        public AoAVehicle Vehicle { get; set; }
    }
}
