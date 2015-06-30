﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace DM.Armory.Model
{
    public class AoAWeapon:IUpdatable, INdfbinLoadable
    {

        #region Ndf Queries
        public static string CAN_SHOOT_WHILE_MOVING_PROPERTY = "TirEnMouvement"; //bool
        public static string AMMUNITION_PROPERTY = "Ammunition";
        public static string NAME_PROPERTY = "Ammunition.Name";//loca hash
        public static string TYPE_PROPERTY = "Ammunition.TypeArme"; //loca hash
        public static string ARME_PROPERTY = "Ammunition.Arme"; //uint32
        public static string POWER_PROPERTY = "Ammunition.Puissance";
        public static string TIME_BETWEEN_SHOTS_PROPERTY = "Ammunition.TempsEntreDeuxTirs"; //float32
        public static string MAX_RANGE_PROPERTY = "Ammunition.PorteeMaximale"; //float32
        public static string SPLASH_DAMAGE_RADIUS_PROPERTY = "Ammunition.RadiusSplashPhysicalDamages"; //float 32
        public static string DAMAGE_PROPERTY = "Ammunition.PhysicalDamages";//float32
        public static string AMBUSH_MULTIPLIER_PROPERTY = "Ammunition.AmbushShotDamageMultiplier"; //float32
        public static string SALVO_RELOAD_TIME_PROPERTY = "Ammunition.TempsEntreDeuxSalves"; //float32
        public static string SIMULTANEOUS_PROJECTILES_PROPERTY = "Ammunition.NbrProjectilesSimultanes"; //uint32
        public static string SHOTS_PER_SALVO_PROPERTY = "Ammunition.NbTirParSalves"; //uint32
        public static string POW_PROBABILITY_PROPERTY = "Ammunition.GeneratePilotsProbability";
        public static string POW_TAGS_PROPERTY = "Ammunition.PilotsTagSet"; //List of TTypeUnitTag, see DebugTagName property
        public static string INDIRECT_FIRE_PROPERTY = "Ammunition.TirIndirect"; //bool
        public static string TRIGGER_FIRE_PROPERTY = "Ammunition.FireTriggeringProbability"; //float32
        public static string HELICOPTER_MAX_RANGE_PROPERTY = "Ammunition.PorteeMaximaleTBA"; //uint32
        public static string MIN_RANGE_PROPERTY = "Ammunition.PorteeMinimal"; //float32
        public static string AIMING_TIME_PROPERTY = "Ammunition.TempsDeVisee"; //float32, null = 0 I think
        public static string PLANE_MAX_RANGE_PROPERTY = "Ammunition.PorteeMaximaleHA"; //float32
        public static string PLANE_MIN_RANGE_PROPERTY = "Ammunition.PorteeMinimaleHA";
        public static string HELICOPTER_MIN_RANGE_PROPERTY = "Ammunition.PorteeMinimaleTBA";
        #endregion

        #region Properties
        public string Name { get; private set; }
        public long MeanRoF { get; private set; }
        public long GroundRange { get; private set; }
        public long VLARange { get; private set; }
        public long VHARange { get; private set; }
        #endregion

        public int WeaponId = -1;
        public byte[] NameHash;
        private long _MaxNbShotsPerSalvo;
        private long _CurrentNbShotsInSalvo;

        private long _NbSimultaneousProjectilesInOneShot;

        private double _TimeBetweenShots;
        private double _SalvoReloadTime;
        private double _AimingTime = 0;   
        
        private bool _HasFirstShotBeenFired = false;

        private long _CurrentNbProjectilesFired;

        private WeaponsStatus _CurrentStatus = WeaponsStatus.IDLE;

        private double _TotalTimeElapsed = 0; // Shitty naming

        private TargetType Targetables = TargetType.None;

        void IUpdatable.Update(double timeElapsed)
        {
            _TotalTimeElapsed += timeElapsed;

            switch (_CurrentStatus) /// Fat, ugly, shitty, but quick, didn't want to make an actual FSM with actual classes and interfaces and shit :<<
            {
                case WeaponsStatus.IDLE:
                    _CurrentStatus = WeaponsStatus.AIMING;
                    _TotalTimeElapsed = 0;
                    break;

                case WeaponsStatus.AIMING:

                    if (_CurrentNbShotsInSalvo <= 0)
                    {
                        _CurrentStatus = WeaponsStatus.RELOADING;
                        _TotalTimeElapsed = 0;
                        break;
                    }

                    if (_TotalTimeElapsed > _AimingTime)
                    {
                        if (_CurrentNbShotsInSalvo > 0)
                        {
                            _CurrentStatus = WeaponsStatus.FIRING;
                            _TotalTimeElapsed = 0;
                        }
                        else
                        {
                            _CurrentStatus = WeaponsStatus.RELOADING;
                            _TotalTimeElapsed = 0;
                        }
                    }
                    break;

                case WeaponsStatus.RELOADING:
                    if (_TotalTimeElapsed > _SalvoReloadTime)
                    {
                        _CurrentNbShotsInSalvo = _MaxNbShotsPerSalvo;
                        _HasFirstShotBeenFired = false;
                        _CurrentStatus = WeaponsStatus.FIRING;
                        _TotalTimeElapsed = 0;
                    }
                    break;

                case WeaponsStatus.FIRING:
                    Firing(_TotalTimeElapsed); 
                    break;
            }
        }

        private void Firing(double totalTimeElapsed)
        {
            if (!_HasFirstShotBeenFired)
            {
                ShootNow();
                _HasFirstShotBeenFired = true;
                _TotalTimeElapsed = 0;
            }
            else
            {
                if (_CurrentNbShotsInSalvo > 0)
                {
                    if (totalTimeElapsed > _TimeBetweenShots)
                    {
                        ShootNow();
                        _TotalTimeElapsed = 0;
                    }
                }
                else
                {
                    _CurrentStatus = WeaponsStatus.RELOADING;
                    _TotalTimeElapsed = 0;
                }
            }
        }

        /// <summary>
        /// Fire one shot
        /// </summary>
        private void ShootNow()
        {
            if (_CurrentNbShotsInSalvo <= _MaxNbShotsPerSalvo && _CurrentNbShotsInSalvo > 0)
            {
                _CurrentNbProjectilesFired += _NbSimultaneousProjectilesInOneShot;
                _CurrentNbShotsInSalvo -= 1;
            }
        }

        public bool LoadData(NdfObject dataobject, IrisZoomDataApi.TradManager dictionary, IrisZoomDataApi.EdataManager iconPackage)
        {
            NdfBoolean ndfbool;
            NdfSingle ndffloat32;
            NdfUInt32 ndfUint32;
            NdfLocalisationHash ndfHash;
            string name;

            // Name
            if (dataobject.TryGetValueFromQuery<NdfLocalisationHash>(NAME_PROPERTY, out ndfHash)) 
            {
                if(dictionary.TryGetString(ndfHash.Value, out name))
                    Name = name;
            }
            else { Name=string.Empty;  }

            return true;
        }
    }

    public enum WeaponsStatus
    {
        IDLE, RELOADING, AIMING, FIRING,
    }

    public enum TargetType:byte
    {
        Infantry = 1, Armor = 2, Helicopters = 4, Planes = 8, None = 0
    }

    public enum WeaponType:uint
    {

    }
}
