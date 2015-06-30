﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

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

        public float TraverseSpeed { get; private set; }

        public void Update(double timeElapsed)
        {
            throw new NotImplementedException();
        }

        public bool LoadData(NdfObject dataobject, IrisZoomDataApi.TradManager dictionary, IrisZoomDataApi.EdataManager iconPackage)
        {
            NdfSingle ndfFloat32;
            NdfCollection ndfCollection;

            // Traerse speed
            if (dataobject.TryGetValueFromQuery<NdfSingle>(ROTATION_SPEED_PROPERTY, out ndfFloat32))
            {
                TraverseSpeed = ndfFloat32.Value;
            }
            else { TraverseSpeed = 0; }


            // Weapons
            if (dataobject.TryGetValueFromQuery<NdfCollection>(WEAPONS_PROPERTY, out ndfCollection))
            {
                List<CollectionItemValueHolder> weaponss = ndfCollection.InnerList.FindAll(x => x.Value is NdfObjectReference);

                List<NdfObjectReference> weapons = new List<NdfObjectReference>();
                foreach (CollectionItemValueHolder w in weaponss)
                {
                    weapons.Add(w.Value as NdfObjectReference);
                }

                AoAWeapon weapon;
                foreach (NdfObjectReference w in weapons)
                {
                    weapon = new AoAWeapon();
                    if (weapon.LoadData(w.Instance, dictionary, null))
                        Weapons.Add(weapon);
                }
            }
            return true;
        }
    }
}
