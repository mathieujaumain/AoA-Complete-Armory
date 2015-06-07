﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace DM.Armory.Model
{
    public class AoAUnit : AoAGameObject, IUpdatable, INdfbinLoadable
    {
        #region Ndf Queries
        public static string NDF_CLASS_NAME = "TUniteDescriptor";
        public static string TURRET_LIST_PATH = "Modules.WeaponManager.Default.TurretDescriptorList";
        public static string VISIBILITY_PATH = "Modules.Visibility.Default";
        public static string DAMMAGE_PATH = "Modules.Dammages.Default";
        public static string TRANSPORTABLE_PATH = "Modules.Transportable";
        public static string HARVESTER_PATH = "Modules.Harvester.Default";
        public static string STORAGE_PATH = "Modules.Storage.Default";
        public static string POW_PATH = "Modules.PilotGenerator.Default";
        public static string TRANSPORTER_PATH = "Modules.Transporter.Default";
        public static string SKILLS_PATH = "Modules.Capacite.Default.DefaultSkillList"; //list of TCapaciteDescriptor_ModernWarfare
        public static string PRODUCTION_PATH = "Modules.Production.Default";
        public static string SCANNER_CONFIG_PATH = "Modules.ScannerConfiguration.Default";
        public static string MOVEMENT_PATH = "Modules.MouvementHandler.Default";
        public static string DEBUG_NAME = "ClassNameForDebug";
        public static 
        #endregion

        public static string[] DEBUG_NAME_USELESS = { "District", "Company", "Cadavre", "Wounded", "Fire", "Missile", "Fake", "Wounded", "Smoke", "En_Construction" }; //should eliminate a good chunk of useless data

        private object _Lock = new object();
        private List<AoAUppgrade> _PossibleUppgrades = new List<AoAUppgrade>();
        private List<AoATurret> _Turrets = new List<AoATurret>();
        private List<AoAUnit> _Children = new List<AoAUnit>();
        private List<AoAUnit> _Parents = new List<AoAUnit>();

        public List<AoATurret> Turrets
        {
            get { lock(_Lock) return _Turrets; }
            set { lock(_Lock) _Turrets = value; }
        }

        public AoAVehicle Vehicle { get; set; }

        public void Update(double timeElapsed)
        {
            throw new NotImplementedException();
        }

        public bool LoadData(NdfObject dataobject)
        {
            return false;
            NdfString debugstring;
            if (dataobject.TryGetValueFromQuery<NdfString>(DEBUG_NAME, out debugstring))
            {
                string debugname = debugstring.Value as string;
                if (DEBUG_NAME_USELESS.Any(x => debugname.Contains(x))) // verify if unit isn't a useless data
                    return false;

                ///Finish loading data

            }
            else
            {
                return false;
            }
        }

    }
}
