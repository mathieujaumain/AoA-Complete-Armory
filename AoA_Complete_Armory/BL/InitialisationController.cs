using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi;
using System.Drawing;
using DM.Armory.Model;
using DM.Armory.Properties;
using System.IO;
using IrisZoomDataApi.Model.Ndfbin;


namespace DM.Armory.BL
{
    public class InitializationController
    {
        
        private static string DATA_FOLDER = @"\Data\ACTOFRUSE\PC";

        private static string ZZ4_FILE = "ZZ_4.dat";
        private static string ICON_PACKAGE = @"pc\texture\pack\commoninterface.ppk";


        private static string ZZ_WIN_FILE = "ZZ_WIN.dat";
        private static string UNIT_DIC = @"pc\localisation\us\localisation\unites.dic";
        private static string TECH_DIC = @"pc\localisation\us\localisation\techno.dic";

        private static string NDF_WIN_FILE = "NDF_WIN.dat";
        private static string EVERYTHING = @"pc\ndf\patchable\gfx\everything.ndfbin";


        public event EventHandler<String> OnLoadingUpdate = delegate { };

        private NdfbinManager _EverythingNdfbin;
        public long Version { private set; get; }

        public NdfbinManager EverythingNdfbin
        {
            get { return _EverythingNdfbin; }
            set { _EverythingNdfbin = value; }
        }
        private TradManager _UniteDic;

        public TradManager UniteDic
        {
            get { return _UniteDic; }
            set { _UniteDic = value; }
        }
        private TradManager _TechDic;

        public TradManager TechDic
        {
            get { return _TechDic; }
            set { _TechDic = value; }
        }
        private EdataManager _IconsPack;

        public EdataManager IconsPack
        {
            get { return _IconsPack; }
            set { _IconsPack = value; }
        }

        private  List<AoABuilding> BuildingsList = new List<AoABuilding>();

        private List<AoABuilding> _CartelBuildingsList = new List<AoABuilding>();
        private List<AoABuilding> _USBuildingsList = new List<AoABuilding>();
        private List<AoABuilding> _ChimeraBuildingsList = new List<AoABuilding>();

        public List<AoABuilding> CartelBuildings { get { return _CartelBuildingsList; } }
        public List<AoABuilding> UsBuildings { get { return _USBuildingsList; } }
        public List<AoABuilding> ChimeraBuildings { get { return _ChimeraBuildingsList; } }

        public bool GetLastUpdateFromDataFolder()
        {
            DirectoryInfo dataDirectory = new DirectoryInfo(Settings.Default.PathToGameFolder + DATA_FOLDER);
            if (!dataDirectory.Exists) return false;
            DirectoryInfo[] dirs = dataDirectory.GetDirectories();
            //sort by version
            if (dirs.Length <= 0) return false;
            List<DirectoryInfo> orderedList;
            try
            {
                orderedList = dirs.OrderBy(x => -Convert.ToInt64(x.Name)).ToList(); // folder name = version
            } catch { return false; }
             

            bool everything = false;
            bool unitdic = false;
            bool techdic = false;
            bool icons = false;
            if (orderedList.Count > 0)
            {
                Version = Convert.ToInt64(orderedList[0].Name);
                foreach (DirectoryInfo dir in orderedList)
                {
                    OnLoadingUpdate(this, "loading data from version " + dir.Name);
                    if (!everything)
                    {
                        OnLoadingUpdate(this, "try to load everything.ndfbin file...");
                        Task.Delay(500);
                        everything = TryGetNdfbinFileFromFolder(dir.FullName, NDF_WIN_FILE, EVERYTHING, out _EverythingNdfbin);
                    }

                    if (!unitdic)
                    {
                        OnLoadingUpdate(this, "try to load unit.dic file...");
                        Task.Delay(500);
                        unitdic = TryGetDicFileFromFolder(dir.FullName, ZZ_WIN_FILE, UNIT_DIC, out _UniteDic);
                    }

                    if (!techdic)
                    {
                        OnLoadingUpdate(this, "try to load tech.dic  file...");
                        Task.Delay(500);
                        techdic = TryGetDicFileFromFolder(dir.FullName, ZZ_WIN_FILE, TECH_DIC, out _TechDic);
                    }

                    if (!icons)
                    {
                        OnLoadingUpdate(this, "try to load icons package file...");
                        Task.Delay(500);
                        icons = TryGetPackFileFromFolder(dir.FullName, ZZ4_FILE, ICON_PACKAGE, out _IconsPack);
                    }

                    if (everything && unitdic && techdic && icons)
                    {
                        OnLoadingUpdate(this, "loading successful");
                        return true;
                    }
                }
            }
            OnLoadingUpdate(this, "loading failed...");
            return false;
        }

        public bool GetUpdateFromDataFolder(long version)
        {
            DirectoryInfo dataDirectory = new DirectoryInfo(Settings.Default.PathToGameFolder);
            DirectoryInfo[] dirs = dataDirectory.GetDirectories(); 
            //sort by version
            List<DirectoryInfo> orderedList = dirs.OrderBy(x => -Convert.ToInt64(x.Name)).ToList();
            orderedList.RemoveAll(x => Convert.ToInt64(x.Name) > version); // remove all folders corresponding to newer versions of the game data

            bool everything = false;
            bool unitdic = false;
            bool techdic = false;
            bool icons = false;

            foreach (DirectoryInfo dir in orderedList)
            {
                if (!everything)
                    everything = TryGetNdfbinFileFromFolder(dir.FullName, NDF_WIN_FILE, EVERYTHING, out _EverythingNdfbin);

                if (!unitdic)
                    unitdic = TryGetDicFileFromFolder(dir.FullName, ZZ_WIN_FILE, UNIT_DIC, out _UniteDic);

                if (!techdic)
                    techdic = TryGetDicFileFromFolder(dir.FullName, ZZ_WIN_FILE, TECH_DIC, out _TechDic);

                if (!icons)
                    icons = TryGetPackFileFromFolder(dir.FullName, ZZ4_FILE, ICON_PACKAGE, out _IconsPack);

                if (everything && unitdic && techdic && icons)
                    return true;
            }

            return false;
        }

        public bool TryGetNdfbinFileFromFolder(string folder, string filename, string ndfbinfile, out NdfbinManager ndfbin)
        {
            ndfbin = null;
            DirectoryInfo folderInfo = new DirectoryInfo(folder);
            FileInfo[] fileInfos = folderInfo.GetFiles(filename, SearchOption.TopDirectoryOnly);
            if (fileInfos.Length <= 0)
                return false;

            EdataManager dataManager = new EdataManager(fileInfos[0].FullName);
            dataManager.ParseEdataFile();
            try
            {
                ndfbin = dataManager.ReadNdfbin(ndfbinfile);
                return true;
            }
            catch
            {
                // TODO : Dump error to log
            }
            return false;
        }

        public bool TryGetDicFileFromFolder(string folder, string filename, string dicfile, out TradManager dic)
        {
            dic = null;
            DirectoryInfo folderInfo = new DirectoryInfo(folder);
            FileInfo[] fileInfos = folderInfo.GetFiles(filename, SearchOption.TopDirectoryOnly);
            if (fileInfos.Length <= 0)
                return false;

            EdataManager dataManager = new EdataManager(fileInfos[0].FullName);
            dataManager.ParseEdataFile();
            try
            {
                dic = dataManager.ReadDictionary(dicfile);
                return true;
            }
            catch
            {
            }
            return false;
        }

        public bool TryGetPackFileFromFolder(string folder, string filename, string packfile, out EdataManager pack)
        {
            pack = null;
            DirectoryInfo folderInfo = new DirectoryInfo(folder);
            FileInfo[] fileInfos = folderInfo.GetFiles(filename, SearchOption.TopDirectoryOnly);
            if (fileInfos.Length <= 0)
                return false;
            EdataManager dataManager = new EdataManager(fileInfos[0].FullName);
            dataManager.ParseEdataFile();
            try
            {
                pack = dataManager.ReadPackage(packfile);
                return true;
            }
            catch
            {
            }
            return false;
        }

        public bool LoadData()
        {
            List<AoAGameObject> result = new List<AoAGameObject>();
            if (GetLastUpdateFromDataFolder())
            {

                // Load all GameObjects
                List<AoABuilding> BuildingsList = new List<AoABuilding>();
                List<NdfObject> tunites = _EverythingNdfbin.GetClass("TUniteDescriptor").Instances;
                OnLoadingUpdate(this, "loading buildings and units...");
                foreach (NdfObject obj in tunites)
                {
                    AoAGameObject gobj = new AoAGameObject();
                    if (gobj.LoadData(obj, _UniteDic, _TechDic,_IconsPack))
                    {
                        if (gobj.Type == ObjectType.Building)
                        {
                            AoABuilding building = new AoABuilding(gobj);
                            if (building.LoadData(obj, _UniteDic, _TechDic, _IconsPack))
                            {
                                BuildingsList.Add(building);
                            }
                        }
                        else
                        {

                        }
                    }

                }
                OnLoadingUpdate(this, "processing buildings...");
                Classify(BuildingsList);
                return true;
            }
            else
            {
                return false;
            }

            
        }


        public Task LoadData(long version)
        {
            if (GetUpdateFromDataFolder(version))
            {
                // Load all GameObjects
                List<AoABuilding> BuildingsList = new List<AoABuilding>();
                List<NdfObject> tunites = _EverythingNdfbin.GetClass("TUniteDescriptor").Instances;
                foreach (NdfObject obj in tunites)
                {
                    AoAGameObject gobj = new AoAGameObject();
                    if (gobj.LoadData(obj, _UniteDic, _TechDic, _IconsPack)) 
                    {
                        if (gobj.Type == ObjectType.Building)
                        {
                            AoABuilding building = new AoABuilding(gobj);
                            if (building.LoadData(obj, _UniteDic, _TechDic, _IconsPack))
                            {
                                BuildingsList.Add(building);
                            } 
                               
                        }
                        else
                        {
                            
                        }
                    }
                        
                }
                Classify(BuildingsList);
            }
            else
            {
                throw new Exception("Couldn't load game data, check game folder.");
            }

            return null;
        }

        private void Classify(List<AoABuilding> BuildingsList)
        {
            foreach (AoABuilding building in BuildingsList)
            {
                switch (building.Faction)
                {
                    case FactionEnum.Cartel:
                        _CartelBuildingsList.Add(building);
                        break;

                    case FactionEnum.US:
                        _USBuildingsList.Add(building);
                        break;

                    case FactionEnum.Chimera:
                        _ChimeraBuildingsList.Add(building);
                        break;

                    default: break;
                }
            }
        }

        public void Clear()
        {
            UniteDic = null;
            TechDic = null;
            EverythingNdfbin = null;
            CartelBuildings.Clear();
            ChimeraBuildings.Clear();
            UsBuildings.Clear();
        }

    }
}
