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
    public class AoABuilding : AoAGameObject, INdfbinLoadable
    {

        #region ndfQueries
        public static string PRODUCABLE_UNITS_PATH = "Modules.Factory.Default.ProducableUnits"; //Collection of reference to TUniteDescriptors
        public static string AVAILABLE_RESEARCHES_PATH = "Modules.TechnoRegistrar.Default.ResearchableTechnos"; // Collection of reference to TTechnoLevelDescriptor
        public static string TURRET_LIST_PATH = "Modules.WeaponManager.Default.TurretDescriptorList";
        public static string VIEW_RANGE_PATH = "Modules.ScannerConfiguration.Default.PorteeVision"; //Float32
        public static string STEALTH_PATH = "Modules.Visibility.Default.UnitStealthBonus"; //Float32
        public static string DAMMAGE_PATH = "Modules.Damage.Default.MaxDamages"; // float32
        public static string ARMOR_PATH = "Modules.Damage.Default.CommonDamageDescriptor.BlindageProperties.ArmorDescriptorFront.BaseBlindage"; // uint32
        #endregion

        private List<AoAUnit> _BuildableUnits = new List<AoAUnit>();
        private List<AoAResearch> _Researches = new List<AoAResearch>();
        private List<AoATurret> _Turrets = new List<AoATurret>();

        public List<AoAResearch> Researches
        {
            get { return _Researches; }
            set { _Researches = value; }
        }

        public List<AoATurret> Turrets
        {
            get { return _Turrets; }
            set {  _Turrets = value; }
        }

        public List<AoAUnit> BuildableUnits
        {
            get { return _BuildableUnits; }
            set { _BuildableUnits = value; }
        }
        public bool IsStealthy { get; private set; }
        public float ViewRange { get; private set; }
        public float Health { get; private set; }
        public int Armor { get; private set; }


        public AoABuilding(AoAGameObject obj)
        {
            Name = obj.Name;
            DebugName = obj.DebugName;
            Description = obj.Description;
            AluminiumCost = obj.AluminiumCost;
            CashCost = obj.CashCost;
            RareEarthCost = obj.RareEarthCost;
            ConstructionTime = obj.ConstructionTime;
            Faction = obj.Faction;
            Icon = obj.Icon;
        }
 
        new public bool LoadData(NdfObject dataobject, TradManager dictionary, TradManager techdic, EdataManager iconPackage)
        {
            NdfCollection collection;

            // UNITS
            if (dataobject.TryGetValueFromQuery<NdfCollection>(PRODUCABLE_UNITS_PATH, out collection))
            {

                List<CollectionItemValueHolder> unitss = collection.InnerList.FindAll(x => x.Value is NdfObjectReference);

                List<NdfObjectReference> units = new List<NdfObjectReference>();
                foreach (CollectionItemValueHolder uni in unitss)
                {
                    units.Add(uni.Value as NdfObjectReference);
                }

                AoAGameObject obj;
                foreach (NdfObjectReference unit in units)
                {
                    obj = new AoAGameObject();
                    if (obj.LoadData(unit.Instance, dictionary, techdic, iconPackage))
                        if (obj.Type != ObjectType.Building)
                        {
                            AoAUnit aunit = new AoAUnit(obj);
                            if (aunit.LoadData(unit.Instance, dictionary, techdic, iconPackage)) // !!!!!
                                _BuildableUnits.Add(aunit);
                        }
                }
            }

            //Stealth
            NdfSingle ndfFloat32;
            IsStealthy = false;
            if (dataobject.TryGetValueFromQuery<NdfSingle>(STEALTH_PATH, out ndfFloat32))
                IsStealthy = ndfFloat32.Value >= 50f;

            if (dataobject.TryGetValueFromQuery<NdfSingle>(DAMMAGE_PATH, out ndfFloat32))
                Health = ndfFloat32.Value;

            // Armor
            NdfUInt32 ndfuint32;
            Armor = 0;
            if (dataobject.TryGetValueFromQuery<NdfUInt32>(ARMOR_PATH, out ndfuint32))
                Armor = (int)ndfuint32.Value;

            // vIEW RANGE   
            if (dataobject.TryGetValueFromQuery<NdfSingle>(VIEW_RANGE_PATH, out ndfFloat32))
            {
                ViewRange = ndfFloat32.Value;
            }
            else { ViewRange = 0; }

            //Turrets
            NdfCollection ndfCollection;
            if (dataobject.TryGetValueFromQuery<NdfCollection>(TURRET_LIST_PATH, out ndfCollection))
            {
                List<CollectionItemValueHolder> turrs = ndfCollection.InnerList.FindAll(x => x.Value is NdfObjectReference);

                List<NdfObjectReference> turrets = new List<NdfObjectReference>();
                foreach (CollectionItemValueHolder turr in turrs)
                {
                    turrets.Add(turr.Value as NdfObjectReference);
                }

                AoATurret turret;
                foreach (NdfObjectReference turr in turrets)
                {
                    turret = new AoATurret();
                    if (turret.LoadData(turr.Instance, dictionary, techdic, iconPackage))
                        Turrets.Add(turret);
                }

            }

            //RESEARCHES
            if (dataobject.TryGetValueFromQuery<NdfCollection>(AVAILABLE_RESEARCHES_PATH, out collection))
            {

                List<CollectionItemValueHolder> ress = collection.InnerList.FindAll(x => x.Value is NdfObjectReference);

                List<NdfObjectReference> researches = new List<NdfObjectReference>();
                foreach (CollectionItemValueHolder uni in ress)
                {
                    researches.Add(uni.Value as NdfObjectReference);
                }

                AoAResearch aResearch;
                foreach (NdfObjectReference research in researches)
                {
                    aResearch = new AoAResearch();
                    if (aResearch.LoadData(research.Instance, dictionary, techdic, iconPackage)) // tech.dic !
                    {
                        Researches.Add(aResearch);
                    }
                }
            }

            return true;

        }
    }
}
