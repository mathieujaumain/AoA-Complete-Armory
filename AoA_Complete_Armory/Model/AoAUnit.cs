using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace DM.Armory.Model
{
    public class AoAUnit : AoAGameObject, IUpdatable, INdfbinLoadable
    {
        #region Ndf Queries
        public static string TURRET_LIST_PATH = "Modules.WeaponManager.Default.TurretDescriptorList";
        public static string VISIBILITY_PATH = "Modules.Visibility.Default";
        public static string DAMMAGE_PATH = "Modules.Dammages.Default";
        public static string TRANSPORTABLE_PATH = "Modules.Transportable";
        public static string HARVESTER_PATH = "Modules.Harvester.Default";
        public static string STORAGE_PATH = "Modules.Storage.Default";
        public static string POW_PATH = "Modules.PilotGenerator.Default";
        public static string TRANSPORTER_PATH = "Modules.Transporter.Default";
        public static string SKILLS_PATH = "Modules.Capacite.Default.DefaultSkillList"; //list of TCapaciteDescriptor_ModernWarfare
        public static string PRODUCTION_PATH = "Modules.Production.Default";
        public static string SCANNER_CONFIG_PATH = "Modules.ScannerConfiguration.Default";
        public static string MOVEMENT_PATH = "Modules.MouvementHandler.Default";
        #endregion

       

        private object _Lock = new object();
        private List<AoAUppgrade> _PossibleUppgrades = new List<AoAUppgrade>();
        private List<AoATurret> _Turrets = new List<AoATurret>();
        private List<AoAUnit> _Children = new List<AoAUnit>();
        private List<AoAUnit> _Parents = new List<AoAUnit>();

        public AoAUnit(AoAGameObject obj)
        {
            Name = obj.Name;
            DebugName = obj.DebugName;
            Description = obj.Description;
            AluminiumCost = obj.AluminiumCost;
            CashCost = obj.CashCost;
            RareEarthCost = obj.RareEarthCost;
            ConstructionTime = obj.ConstructionTime;
            Faction = obj.Faction;

            Vehicle = new AoAVehicle();
        }

        public List<AoATurret> Turrets
        {
            get { lock(_Lock) return _Turrets; }
            set { lock(_Lock) _Turrets = value; }
        }

        public AoAVehicle Vehicle { get; set; }

        public void Update(double timeElapsed)
        {
            throw new NotImplementedException();
        }

        public bool LoadData(NdfObject dataobject, TradManager dictionary, EdataManager iconPackage)
        {
            return true;
        }

    }
}
