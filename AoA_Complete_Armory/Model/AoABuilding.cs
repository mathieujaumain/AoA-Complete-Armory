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

        private List<AoAUnit> _BuildableUnits = new List<AoAUnit>();
        private List<AoAUppgrade> _Researches = new List<AoAUppgrade>();

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
        }

        bool INdfbinLoadable.LoadData(NdfObject dataobject, TradManager dictionary, EdataManager iconPackage)
        {
            throw new NotImplementedException();
        }
    }
}
