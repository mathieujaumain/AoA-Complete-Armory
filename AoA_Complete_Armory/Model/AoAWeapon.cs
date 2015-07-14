using System;
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
        public static string POWER_PROPERTY = "Ammunition.Puissance"; //float32
        public static string TIME_BETWEEN_SHOTS_PROPERTY = "Ammunition.TempsEntreDeuxTirs"; //float32
        public static string MAX_RANGE_PROPERTY = "Ammunition.PorteeMaximale"; //float32
        public static string SPLASH_DAMAGE_RADIUS_PROPERTY = "Ammunition.RadiusSplashPhysicalDamages"; //float 32
        public static string DAMAGE_PROPERTY = "Ammunition.PhysicalDamages";//float32
        public static string AMBUSH_MULTIPLIER_PROPERTY = "Ammunition.AmbushShotDamageMultiplier"; //float32
        public static string SALVO_RELOAD_TIME_PROPERTY = "Ammunition.TempsEntreDeuxSalves"; //float32
        public static string SIMULTANEOUS_PROJECTILES_PROPERTY = "Ammunition.NbrProjectilesSimultanes"; //uint32
        public static string SHOTS_PER_SALVO_PROPERTY = "Ammunition.NbTirParSalves"; //uint32
        public static string POW_PROBABILITY_PROPERTY = "Ammunition.GeneratePilotProbability";
        public static string POW_TAGS_PROPERTY = "Ammunition.PilotsTagSet"; //List of TTypeUnitTag, see DebugTagName property
        public static string INDIRECT_FIRE_PROPERTY = "Ammunition.TirIndirect"; //bool
        public static string TRIGGER_FIRE_PROPERTY = "Ammunition.FireTriggeringProbability"; //float32
        public static string HELICOPTER_MAX_RANGE_PROPERTY = "Ammunition.PorteeMaximaleTBA"; //uint32
        public static string MIN_RANGE_PROPERTY = "Ammunition.PorteeMinimal"; //float32
        public static string AIMING_TIME_PROPERTY = "Ammunition.TempsDeVisee"; //float32, null = 0 I think
        public static string PLANE_MAX_RANGE_PROPERTY = "Ammunition.PorteeMaximaleHA"; //float32
        public static string PLANE_MIN_RANGE_PROPERTY = "Ammunition.PorteeMinimaleHA";
        public static string HELICOPTER_MIN_RANGE_PROPERTY = "Ammunition.PorteeMinimaleTBA";
        public static string AMBUSH_PROPERTY = "Ammunition.AmbushShotDamageMultiplier"; //float32

        public static string NOISE = "Ammunition.NoiseDissimulationMalus"; // float 32
        public static string SUPRESS_DAMAGES = "Ammunition.SuppressDamages"; //float32
        #endregion

        #region Properties
        public string Name { get; private set; }
        public double Sustained { get; private set; }
        public float GroundRange { get; private set; }
        public float VLARange { get; private set; }
        public float VHARange { get; private set; }
        public float PoWGen { get; private set; }
        public float Splash { get; private set; }
        public float Alpha { get; private set; }

        public int SimulatenousShots { get; private set; }
        public int MaxShotsPerSalvo { get; private set; }
        public double TimeBetweenShots { get; private set; }
        public double SalvoReloadTime { get; private set; }
        public double AimingTime { get; private set; }
        public float AmbushMultiplier { get; private set; }
        public bool IsSilenced { get; private set; }
        public float SupressDamages { get; private set; }
        #endregion

        public int WeaponId = -1;
        public byte[] NameHash;
        private long _CurrentNbShotsInSalvo;
        private double _AimingTime = 0;   
        
        private bool _HasFirstShotBeenFired = false;

        private long _CurrentNbProjectilesFired;

        private WeaponsStatus _CurrentStatus = WeaponsStatus.IDLE;

        private double _TotalTimeElapsed = 0; // Shitty naming

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
                    if (_TotalTimeElapsed > SalvoReloadTime)
                    {
                        _CurrentNbShotsInSalvo = MaxShotsPerSalvo;
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
                    if (totalTimeElapsed > TimeBetweenShots)
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
            if (_CurrentNbShotsInSalvo <= MaxShotsPerSalvo && _CurrentNbShotsInSalvo > 0)
            {
                _CurrentNbProjectilesFired += SimulatenousShots;
                _CurrentNbShotsInSalvo -= 1;
            }
        }

        new public bool LoadData(NdfObject dataobject, IrisZoomDataApi.TradManager dictionary, IrisZoomDataApi.TradManager dictionary2, IrisZoomDataApi.EdataManager iconPackage)
        {
            NdfBoolean ndfbool;
            NdfSingle ndffloat32;
            NdfUInt32 ndfUint32;
            NdfLocalisationHash ndfHash;
            string name;

            // Name
            if (dataobject.TryGetValueFromQuery<NdfLocalisationHash>(NAME_PROPERTY, out ndfHash))
            {
                if (dictionary.TryGetString(ndfHash.Value, out name))
                    Name = name;
            }
            else { Name = string.Empty; }

            //GroundRange
            if (dataobject.TryGetValueFromQuery<NdfSingle>(MAX_RANGE_PROPERTY, out ndffloat32))
                GroundRange = ndffloat32.Value;




            //Alpha
            if (!dataobject.TryGetValueFromQuery<NdfSingle>(DAMAGE_PROPERTY, out ndffloat32))
                return false;
            Alpha = ndffloat32.Value;

            //TBARange
            if (dataobject.TryGetValueFromQuery<NdfSingle>(HELICOPTER_MAX_RANGE_PROPERTY, out ndffloat32))
                VLARange = ndffloat32.Value;

            //THARange
            if (dataobject.TryGetValueFromQuery<NdfSingle>(PLANE_MAX_RANGE_PROPERTY, out ndffloat32))
                VHARange = ndffloat32.Value;

            //Splash
            if (dataobject.TryGetValueFromQuery<NdfSingle>(SPLASH_DAMAGE_RADIUS_PROPERTY, out ndffloat32))
                Splash = ndffloat32.Value;

            //PowGen
            if (dataobject.TryGetValueFromQuery<NdfSingle>(POW_PROBABILITY_PROPERTY, out ndffloat32))
                PoWGen = ndffloat32.Value;

            //AmBush
            if (dataobject.TryGetValueFromQuery<NdfSingle>(AMBUSH_MULTIPLIER_PROPERTY, out ndffloat32))
                AmbushMultiplier = ndffloat32.Value;

            // SimulatenousShots
            if (!dataobject.TryGetValueFromQuery<NdfUInt32>(SIMULTANEOUS_PROJECTILES_PROPERTY, out ndfUint32))
                return false;
            SimulatenousShots = (int)ndfUint32.Value;

            // MaxShotsPerSalvo
            if (!dataobject.TryGetValueFromQuery<NdfUInt32>(SHOTS_PER_SALVO_PROPERTY, out ndfUint32))
                return false;
            MaxShotsPerSalvo = (int)ndfUint32.Value;

            //TimeBetweenShots
            if (!dataobject.TryGetValueFromQuery<NdfSingle>(TIME_BETWEEN_SHOTS_PROPERTY, out ndffloat32))
                return false;
            TimeBetweenShots = ndffloat32.Value;

            //Salvo Reload Time
            if (!dataobject.TryGetValueFromQuery<NdfSingle>(SALVO_RELOAD_TIME_PROPERTY, out ndffloat32))
                return false;
            SalvoReloadTime = ndffloat32.Value;

            //Aiming Time
            AimingTime = 0;
            if (dataobject.TryGetValueFromQuery<NdfSingle>(AIMING_TIME_PROPERTY, out ndffloat32))
                AimingTime = ndffloat32.Value;

            //Noise
            IsSilenced = false;
            if (dataobject.TryGetValueFromQuery<NdfSingle>(NOISE, out ndffloat32))
                IsSilenced = ndffloat32.Value < 5;

            //Supress
            SupressDamages = 0;
            if (dataobject.TryGetValueFromQuery<NdfSingle>(SUPRESS_DAMAGES, out ndffloat32))
                SupressDamages = ndffloat32.Value;

            if (dataobject.TryGetValueFromQuery<NdfSingle>(AMBUSH_MULTIPLIER_PROPERTY, out ndffloat32))
                AmbushMultiplier = ndffloat32.Value;


            double oneSalvoCycleTime =  TimeBetweenShots * MaxShotsPerSalvo + SalvoReloadTime; // sec
            float nbshotInOneCycle = SimulatenousShots * MaxShotsPerSalvo;
            double nbCycleInAMinute = 60 / oneSalvoCycleTime;

            Sustained = nbshotInOneCycle * nbCycleInAMinute * Alpha;

            return true;
        }
    }

    public enum WeaponsStatus
    {
        IDLE, RELOADING, AIMING, FIRING,
    }

    public enum WeaponType:uint
    {

    }
}
