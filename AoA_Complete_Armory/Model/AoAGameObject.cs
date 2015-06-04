﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.Armory.Model
{
    public class AoAGameObject:AoABaseObject
    {
        public AoAGameObject()
        {
            Name = string.Empty;
            DebugName = string.Empty;
            Description = string.Empty;
            AluminiumCost = 0;
            CashCost = 0;
            RareEarthCost = 0;
            ConstructionTime = 0;
            Faction = FactionEnum.Neutral;
        }

        public string Name { get; set; }
        public string DebugName { get; set; }
        public string Description { get; set; }

        public int AluminiumCost { get; set; }
        public int CashCost { get; set; }
        public int RareEarthCost { get; set; }

        public long ConstructionTime { get; set; }

        public FactionEnum Faction { get; set; }

    }

    public enum FactionEnum:int
    {
        US = 3, Cartel = 1, Chimera = 2, Neutral , Other
    }

}
