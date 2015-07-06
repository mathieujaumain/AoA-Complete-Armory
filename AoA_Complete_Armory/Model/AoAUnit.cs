using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;
using DM.Armory.Model.Skills;

namespace DM.Armory.Model
{
    public class AoAUnit : AoAGameObject, IUpdatable, INdfbinLoadable
    {
        #region Ndf Queries
        public static string TURRET_LIST_PATH = "Modules.WeaponManager.Default.TurretDescriptorList";
        public static string STEALTH_PATH = "Modules.Visibility.Default.UnitStealthBonus"; //Float32
        public static string AUTOREVEAL_PATH = "Modules.Visibility.Default.AutoRevealType"; //Int32 
        public static string DAMMAGE_PATH = "Modules.Damage.Default.MaxDamages"; // float32
        public static string ARMOR_PATH = "Modules.Damage.Default.CommonDamageDescriptor.BlindageProperties.ArmorDescriptorFront.BaseBlindage"; // uint32
        public static string TRANSPORTABLE_PATH = "Modules.Transportable.NbSeatsOccupied"; //int32
        public static string HARVESTER_PATH = "Modules.Harvester.Default";
        public static string STORAGE_PATH = "Modules.Storage.Default";
        public static string POW_PATH = "Modules.PilotGenerator.Default.NbMaxPilotsGenerated"; //Uint32
        public static string SKILLS_PATH = "Modules.Capacite.Default.DefaultSkillList"; //list of TCapaciteDescriptor_ModernWarfare
        public static string SCANNER_CONFIG_PATH = "Modules.ScannerConfiguration.Default";
        public static string MOVEMENT_PATH = "Modules.MouvementHandler.Default";
        public static string VIEW_RANGE_PATH = "Modules.ScannerConfiguration.Default.PorteeVision"; //Float32
        public static string TRANSPORT_PATH = "Modules.Transporter.Default.NbSeatsAvailable"; //int32
        #endregion

       

        private object _Lock = new object();
        private List<AoAResearch> _PossibleUppgrades = new List<AoAResearch>();
        private List<AoATurret> _Turrets = new List<AoATurret>();
        private List<AoAUnit> _Children = new List<AoAUnit>();
        private List<IAoASkills> _Skills = new List<IAoASkills>();

        public AoAUnit(AoAGameObject obj)
        {
            Name = obj.Name;
            DebugName = obj.DebugName;
            Description = obj.Description;
            AluminiumCost = obj.AluminiumCost;
            CashCost = obj.CashCost;
            RareEarthCost = obj.RareEarthCost;
            Faction = obj.Faction;
            Icon = obj.Icon;

            //Vehicle = new AoAVehicle();
        }

        public List<AoATurret> Turrets
        {
            get { lock(_Lock) return _Turrets; }
            set { lock(_Lock) _Turrets = value; }
        }

        //public AoAVehicle Vehicle { get; set; }
        public float ViewRange { get; set; }
        public int nbrPOW { get; set; }
        public float Health { get; set; }
        public int TransportSlot { get; set; }
        public int SlotTaken { get; set; }
        public bool CanSpotStealthyUnits { get; set; }
        public bool CanHarvest { get; set; }
        public int StorageSize { get; set; }
        public bool IsStealthy { get; set; }
        public int Armor { get; set; }
        public int AutoReveal { get; set; }

        

        public void Update(double timeElapsed)
        {
            throw new NotImplementedException();
        }

        new public bool LoadData(NdfObject dataobject, TradManager dictionary, EdataManager iconPackage)
        {
            NdfUInt32 ndfuint32;
            NdfSingle ndfFloat32;
            NdfInt32 ndfInt32;
            NdfObject ndfObject;
            NdfCollection ndfCollection;

            // HP
            if (!dataobject.TryGetValueFromQuery<NdfSingle>(DAMMAGE_PATH, out ndfFloat32))
                return false;
            Health = ndfFloat32.Value;

            // Armor
            if (dataobject.TryGetValueFromQuery<NdfUInt32>(ARMOR_PATH, out ndfuint32))
                Armor = (int)ndfuint32.Value;

            //POW
            if (dataobject.TryGetValueFromQuery<NdfUInt32>(POW_PATH, out ndfuint32))
                nbrPOW = (int)ndfuint32.Value;

            //AutoReveal
            if (!dataobject.TryGetValueFromQuery<NdfInt32>(AUTOREVEAL_PATH, out ndfInt32))
                return false;
            AutoReveal = ndfInt32.Value;

            //Transporter
            if (dataobject.TryGetValueFromQuery<NdfInt32>(TRANSPORT_PATH, out ndfInt32))
            {
                TransportSlot = ndfInt32.Value;
            }
            else { TransportSlot = 0; }

            //Stealth
            if (!dataobject.TryGetValueFromQuery<NdfSingle>(STEALTH_PATH, out ndfFloat32))
                return false;
            IsStealthy = ndfFloat32.Value > 0f; 

            // Slot Taken
            if(dataobject.TryGetValueFromQuery<NdfInt32>(TRANSPORTABLE_PATH, out ndfInt32))
            {
                SlotTaken = ndfInt32.Value;
            } else { SlotTaken = 0; }

            // Slot Taken
            if(dataobject.TryGetValueFromQuery<NdfSingle>(VIEW_RANGE_PATH, out ndfFloat32))
            {
                ViewRange = ndfFloat32.Value;
            } else { ViewRange = 0; }


            //Turrets
            if(dataobject.TryGetValueFromQuery<NdfCollection>(TURRET_LIST_PATH, out ndfCollection))
            {
                List<CollectionItemValueHolder> turrs = ndfCollection.InnerList.FindAll(x => x.Value is NdfObjectReference);

                List<NdfObjectReference> turrets = new List<NdfObjectReference>();
                foreach (CollectionItemValueHolder turr in turrs)
                {
                    turrets.Add(turr.Value as NdfObjectReference);
                }
                
                AoATurret turret;
                foreach(NdfObjectReference turr in turrets)
                {
                    turret = new AoATurret();
                    if(turret.LoadData(turr.Instance, dictionary, null))
                        Turrets.Add(turret);
                }

            }// a tester


            //Vehicle



            return true;
        }

    }

    
}
