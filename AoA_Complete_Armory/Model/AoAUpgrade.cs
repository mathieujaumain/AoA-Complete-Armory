using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace DM.Armory.Model
{
    public class AoAUpgrade : AoAResearch, INdfbinLoadable
    {

        #region NdfQueries
        public static string UNIT_UPGRADE = "UpgradeDescriptor";
        public static string UPGRADE_EFFECT_COLLECTION = "Effect.Effects";
        #endregion

        public AoAUnit UpgradedUnit { private set; get; }
        public AoABuilding UpgradedBuilding { private set; get; }
        public ObjectType Type { private set; get; }

        public AoAUpgrade(AoAResearch res)
        {
            Name = res.Name;
            Description = res.Description;
            CashCost = res.CashCost;
            AluminiumCost = res.AluminiumCost;
            RareEarthCost = res.RareEarthCost;
            Icon = res.Icon;
        }

        new public bool LoadData(NdfObject dataobject, IrisZoomDataApi.TradManager dictionary, IrisZoomDataApi.TradManager techdic, IrisZoomDataApi.EdataManager iconPackage)
        {

            AoAGameObject obj = new AoAGameObject();
            NdfObjectReference objRef;

            if (dataobject.TryGetValueFromQuery<NdfObjectReference>(UNIT_UPGRADE, out objRef))
            {
                if (obj.LoadData(objRef.Instance, dictionary, techdic, iconPackage))
                {
                    if(obj.Type != ObjectType.Building)
                    {
                        AoAUnit unit = new AoAUnit(obj);
                        if (unit.LoadData(objRef.Instance, dictionary, techdic, iconPackage))
                        {
                            UpgradedUnit = unit;
                            Type = UpgradedUnit.Type;
                        }
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                }

            }
            return false;
        }




    }
}
