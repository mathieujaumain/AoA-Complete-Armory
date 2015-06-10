using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi.Model.Ndfbin;

namespace DM.Armory.Model
{
    public class AoATurret : IUpdatable, INdfbinLoadable
    {
        #region Ndf Queries 
        public static string WEAPONS_PROPERTY = "MountedWeaponDescriptorList";
        public static string ROTATION_SPEED_PROPERTY = "VitesseRotation"; //Float32
        #endregion

        private List<AoAWeapon> _Weapons = new List<AoAWeapon>();

        public List<AoAWeapon> Weapons
        {
            get { return _Weapons; }
            set { _Weapons = value; }
        }

       // Ammo ?


        public void Update(double timeElapsed)
        {
            throw new NotImplementedException();
        }

        public bool LoadData(NdfObject dataobject)
        {
            throw new NotImplementedException();
        }

        public bool LoadData(NdfObject dataobject, IrisZoomDataApi.TradManager dictionary, IrisZoomDataApi.EdataManager iconPackage)
        {
            throw new NotImplementedException();
        }
    }
}
