using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.Armory.Model
{
    public class AoAGameObject:AoABaseObject
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int AluminiumCost { get; set; }
        public int CashCost { get; set; }
        public int RareEarthCost { get; set; }

        public FactionEnum Faction { get; set; }

    }

    public enum FactionEnum
    {
        US, Cartel, Chimera, Neutral, Other
    }
}
