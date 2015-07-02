using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace DM.Armory.Model
{
    public class AoAResearch:AoAGameObject, INdfbinLoadable
    {



        public AoAResearch() { }

        // Useless
        public AoAResearch(AoAGameObject obj) 
        {
            Name = obj.Name;
            DebugName = obj.DebugName;
            Description = obj.Description;
            AluminiumCost = obj.AluminiumCost;
            CashCost = obj.CashCost;
            RareEarthCost = obj.RareEarthCost;
            Faction = obj.Faction;
            Icon = obj.Icon;
        }

        public bool LoadData(NdfObject dataobject, IrisZoomDataApi.TradManager dictionary, IrisZoomDataApi.EdataManager iconPackage)
        {
            return true;
        }
    }
}
