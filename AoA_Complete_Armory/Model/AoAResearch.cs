using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace DM.Armory.Model
{
    public class AoAResearch:AoAGameObject, INdfbinLoadable
    {

        #region NDF queries
        public static string NAME_HASH = "NameToken"; // local hash
        public static string DESCRIPTION_HASH = "DescriptionToken";
        public static string ICON_PATH = "Texture.FileName";
        public static string CASH_COST = "ResourcesNeeded.5";
        public static string ALU_COST = "ResourcesNeeded.3";
        public static string REA_COST = "ResourcesNeeded.14";
        public static string ACTIONS = "Contrainte.Actions"; // list of XUnitEffect
        public static string ACTION_UPGRADE_KIND = "TActionLaunchUnitEffectInTeam";
        public static string EFFECT_UPGRADE_CLASS = "TUnitEffectLaunchUpgradeDescriptor";
        public static string UNIT_UPGRADE = "Effect.Effects[0]";
        #endregion


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


        new public bool LoadData(NdfObject dataobject, TradManager dictionary, TradManager dictionary2, EdataManager iconPackage)
        {

            InstanceIndex = dataobject.Id;

            //Finish loading data
            NdfLocalisationHash localisation;
            string aString;

            //NAME
            if (dataobject.TryGetValueFromQuery<NdfLocalisationHash>(NAME_HASH, out localisation))
            {
                if (!dictionary2.TryGetString(localisation.Value, out aString))
                    return false;
                Name = aString;
            }

            //Description
            if (dataobject.TryGetValueFromQuery<NdfLocalisationHash>(DESCRIPTION_HASH, out localisation))
            {
                if (!dictionary2.TryGetString(localisation.Value, out aString))
                    return false;
                Description = aString;
            }

            //Icon
            string iconPath;
            NdfString ndfstring = null;
            Bitmap bitmap;
            if (dataobject.TryGetValueFromQuery<NdfString>(ICON_PATH, out ndfstring))
                if (ndfstring != null)
                {
                    string iconpath = ndfstring.ToString().Replace(@"/", @"\").Replace(@"GameData:\", @"pc\texture\").Replace("png", "tgv").ToLower();
                    if (iconPackage.TryToLoadTgv(iconpath, out bitmap)) // must modify icon path first
                        Icon = new Bitmap(bitmap);
                }

            Type = ObjectType.Research;

            //Faction
            Faction = FactionEnum.None;

            //COSTS
            NdfUInt32 ndfuint32;
            try
            {
                if (dataobject.TryGetValueFromQuery<NdfUInt32>(CASH_COST, out ndfuint32))
                    CashCost = (int)ndfuint32.Value;
            }
            catch { }
            try
            {
                if (dataobject.TryGetValueFromQuery<NdfUInt32>(ALU_COST, out ndfuint32))
                    AluminiumCost = (int)ndfuint32.Value;
            }
            catch { }
            try
            {
                if (dataobject.TryGetValueFromQuery<NdfUInt32>(REA_COST, out ndfuint32))
                    RareEarthCost = (int)ndfuint32.Value;
            }
            catch { }


            return true;
        }
    }
}
