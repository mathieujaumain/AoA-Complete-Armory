using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Armory.Model
{
    public class AoAWeapon:IUpdatable
    {
        public int WeaponId = -1;
        public byte[] NameHash;
        private long _MaxNbShotsPerSalvo;
        private long _CurrentNbShotsInSalvo;

        private long _NbSimultaneousProjectilesInOneShot;

        private double _TimeBetweenShots;
        private double _SalvoReloadTime;
        private double _AimingTime = 0.2;   // This is my best bet, I don't remember where to find it, 
        //plus it's obviously different amoung the weapons, like arty and all

        private bool _HasFirstShotBeenFired = false;

        private long _CurrentNbProjectilesFired;

        private WeaponsStatus _CurrentStatus = WeaponsStatus.IDLE;

        private double _TotalTimeElapsed = 0; // Shitty naming

        private TargetType Targetables = 

        void IUpdatable.Update(double timeElapsed)
        {
            _TotalTimeElapsed += timeElapsed;

            switch (_CurrentStatus) /// Fat, ugly, shitty, dirty-d but quick, didn't want to make an actual FSM with actual classes and interfaces and shit :<<
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
                    Firing(_TotalTimeElapsed); // because this and stuffs
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


    }

    public enum WeaponsStatus
    {
        IDLE, RELOADING, AIMING, FIRING,
    }

    public enum TargetType
    {
        Infantry, Armor, Air
    }
}
