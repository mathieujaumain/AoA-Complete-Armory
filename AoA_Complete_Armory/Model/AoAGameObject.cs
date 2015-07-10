using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;
using System.Drawing;

namespace DM.Armory.Model
{
    public class AoAGameObject:AoABaseObject, INdfbinLoadable
    {
        #region  NDFbin queries
        public static string UNIT_FACTION = "Modules.TypeUnit.Default.MotherCountry";
        public static string UNIT_FACTION_ALT = "Modules.TypeUnit.Nationalite";
        public static string UNIT_ICON_ALT = "Modules.TypeUnit.TextureForInterface.FileName";
        public static string UNIT_ICON = "Modules.TypeUnit.Default.TextureForInterface.FileName";
        public static string UNIT_NAME_HASH = "Modules.TypeUnit.Default.NameInMenuToken";
        public static string UNIT_NAME_HASH_ALT = "Modules.TypeUnit.NameInMenuToken";
        public static string UNIT_TYPE = "Modules.TypeUnit.Default.Category";
        public static string UNIT_TYPE_ALT = "Modules.TypeUnit.Category";
        public static string UNIT_DESCRIPTION_HASH = "Modules.TypeUnit.Default.DescriptionHintToken";
        public static string UNIT_DESCRIPTION_HASH_ALT = "Modules.TypeUnit.DescriptionHintToken";
        public static string UNIT_CASH_COST = "Modules.Production.Default.ProductionRessourcesNeeded.5";
        public static string UNIT_ALU_COST = "Modules.Production.Default.ProductionRessourcesNeeded.3";
        public static string UNIT_ELEC_COST = "Modules.Production.Default.ProductionRessourcesNeeded.6";
        public static string UNIT_REA_COST = "Modules.Production.Default.ProductionRessourcesNeeded.14";

        public static string DEBUG_NAME = "ClassNameForDebug";
        public static string NDF_CLASS_NAME = "TUniteDescriptor";
        
        #endregion

        public static string[] DEBUG_NAME_USELESS = { "District", "Company", "Cadavre", "Wounded","Extracteur", "Fire", "Missile", "Fake", "Wounded", "Smoke", "En_Construction" }; //should eliminate a good chunk of useless data

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

        public string Name { get; internal set; }
        public string DebugName { get; internal set; }
        public string Description { get; internal set; }
        public ObjectType Type { get; internal set; }
        public uint InstanceIndex { get; internal set; }

        public Bitmap Icon { get; internal set; }

        public int AluminiumCost { get; internal set; }
        public int CashCost { get; internal set; }
        public int RareEarthCost { get; internal set; }
        public int ElectricityCost { get; internal set; }

        public long ConstructionTime { get; internal set; }

        public FactionEnum Faction { get; internal set; }


        public bool LoadData(NdfObject dataobject, TradManager dictionary, TradManager dictionary2, EdataManager iconPackage)
        {
            InstanceIndex = dataobject.Id;

            NdfString debugstring;
            if (dataobject.TryGetValueFromQuery<NdfString>(DEBUG_NAME, out debugstring))
            {
                DebugName = debugstring.ToString();
                if (DEBUG_NAME_USELESS.Any(x => DebugName.Contains(x))) // verify if unit isn't a useless data
                    return false;

                //Finish loading data
                NdfLocalisationHash localisation;
                string aString;

                //NAME
                if (!dataobject.TryGetValueFromQuery<NdfLocalisationHash>(UNIT_NAME_HASH, out localisation))
                    if (!dataobject.TryGetValueFromQuery<NdfLocalisationHash>(UNIT_NAME_HASH_ALT, out localisation))
                        return false;
                if (!dictionary.TryGetString(localisation.Value, out aString))
                    return false;
                Name = aString;

                //Description
                if (!dataobject.TryGetValueFromQuery<NdfLocalisationHash>(UNIT_DESCRIPTION_HASH, out localisation))
                    if (!dataobject.TryGetValueFromQuery<NdfLocalisationHash>(UNIT_DESCRIPTION_HASH_ALT, out localisation))
                        return false;
                if (!dictionary.TryGetString(localisation.Value, out aString))
                    return false;
                Description = aString;

                //Icon
                string iconPath;
                NdfString ndfstring = null;
                Bitmap bitmap;
                if (!dataobject.TryGetValueFromQuery<NdfString>(UNIT_ICON, out ndfstring))
                    dataobject.TryGetValueFromQuery<NdfString>(UNIT_ICON_ALT, out ndfstring);
                if (ndfstring != null)
                {
                    string iconpath = ndfstring.ToString().Replace(@"/", @"\").Replace(@"GameData:\", @"pc\texture\").Replace("png", "tgv").ToLower();
                    if (iconPackage.TryToLoadTgv(iconpath, out bitmap)) // must modify icon path first
                        Icon = new Bitmap(bitmap);
                }

                //Faction
                NdfString str;
                if (!dataobject.TryGetValueFromQuery<NdfString>(UNIT_FACTION, out str))
                    return false;

                if (str != null)
                {
                    Faction = FromString2FactionEnum(str.ToString());
                }
                else
                {
                    Faction = FactionEnum.Other;
                }

                //Type
                NdfInt32 ndfint32;
                if (!dataobject.TryGetValueFromQuery<NdfInt32>(UNIT_TYPE, out ndfint32))
                    if (!dataobject.TryGetValueFromQuery<NdfInt32>(UNIT_TYPE_ALT, out ndfint32))
                        return false;
                Type = (ObjectType)ndfint32.Value;


                //COSTS
                NdfUInt32 ndfuint32;
                if (dataobject.TryGetValueFromQuery<NdfUInt32>(UNIT_CASH_COST, out ndfuint32))
                    CashCost = (int)ndfuint32.Value;
                if (dataobject.TryGetValueFromQuery<NdfUInt32>(UNIT_ALU_COST, out ndfuint32))
                    AluminiumCost = (int)ndfuint32.Value;
                if (dataobject.TryGetValueFromQuery<NdfUInt32>(UNIT_ELEC_COST, out ndfuint32))
                    ElectricityCost = (int)ndfuint32.Value;
                if (dataobject.TryGetValueFromQuery<NdfUInt32>(UNIT_REA_COST, out ndfuint32))
                    RareEarthCost = (int)ndfuint32.Value;

                return true;
            }
            else
            {
                return false;
            }
        }

        public FactionEnum FromString2FactionEnum(string str)
        {
            switch (str)
            {
                case "US": return FactionEnum.US;
                case "CS": return FactionEnum.Cartel;
                case "TFT": return FactionEnum.Chimera;
                default: return FactionEnum.Other;
            }
        }
    }

    public enum FactionEnum:uint
    {
        US = 0, Cartel = 1, Chimera = 2, Neutral , Other, None
    }

    

    public enum ObjectType : uint
    {
        Ground_Unit_A = 3, Ground_Unit_B = 5, Ground_Unit_C = 6, Aircraft = 2, Building = 8, Research = 50
    }

}
