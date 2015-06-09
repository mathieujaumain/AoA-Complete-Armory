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

        private NdfbinManager _EverythingNdfbin;
        private TradManager _UniteDic;
        private TradManager _TechDic;
        private EdataManager _IconsPack;


        public bool GetLastUpdateFromDataFolder()
        {
            DirectoryInfo dataDirectory = new DirectoryInfo(Settings.Default.PathToData);
            DirectoryInfo[] dirs = dataDirectory.GetDirectories();
            //sort by version
            List<DirectoryInfo> orderedList = dirs.OrderBy(x => Convert.ToInt32(x.Name)).ToList();

            bool everything = false;
            bool unitdic = false;
            bool techdic = false;
            bool icons = false;

            foreach (DirectoryInfo dir in orderedList)
            {
                if(!everything)
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
            try
            {
                ndfbin = dataManager.ReadNdfbin(ndfbinfile);
                return true;
            }
            catch
            {
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
    }
}
