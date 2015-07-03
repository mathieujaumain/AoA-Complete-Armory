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
#endregion

        private List<AoAUnit> _BuildableUnits = new List<AoAUnit>();
        private List<AoAResearch> _Researches = new List<AoAResearch>();

        public List<AoAResearch> Researches
        {
            get { return _Researches; }
            set { _Researches = value; }
        }

        public List<AoAUnit> BuildableUnits
        {
            get { return _BuildableUnits; }
            set { _BuildableUnits = value; }
        }

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

        new public bool LoadData(NdfObject dataobject, TradManager dictionary, EdataManager iconPackage)
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
                    if (obj.LoadData(unit.Instance, dictionary, iconPackage))
                        if (obj.Type != ObjectType.Building)
                        {
                            AoAUnit aunit = new AoAUnit(obj);
                            if (aunit.LoadData(unit.Instance, dictionary, iconPackage)) // !!!!!
                                _BuildableUnits.Add(aunit);
                        }
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
                    if (aResearch.LoadData(research.Instance, DM.Armory.View.Windows.MainWindow.techDic, iconPackage)) // tech.dic !
                    {
                        Researches.Add(aResearch);
                    }
                }
            }

            return true;
            
        }
    }
}
