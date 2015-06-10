using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Armory.Model
{
    public class AoABuilding : AoAGameObject
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
    }
}
