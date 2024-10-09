using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Car
{
    [Serializable]
    public class CarSound
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float engineFadingTime;
        [SerializeField] private AnimationCurve engineSpeed;
        [SerializeField] private float minSoundVolume;
        [SerializeField] private float maxSoundVolume;
        [SerializeField] private float timeCurve;

        private float _currentSpeed;
        private float _currentSoundVolume;
        private bool _isNoDrive;

        internal void EngineSound(bool isDrive, float axis)
        {
            if (isDrive)
            {
                if (_isNoDrive)
                    _currentSpeed = 0;
                
                _isNoDrive = false;
                if (axis != 0)
                {
                    _currentSpeed = Mathf.Lerp(_currentSpeed, engineSpeed[engineSpeed.length - 1].time,
                        timeCurve * Time.deltaTime);
                    _currentSoundVolume = Mathf.Lerp(_currentSoundVolume, maxSoundVolume, timeCurve * Time.deltaTime);
                }
                else
                {
                    _currentSpeed = Mathf.Lerp(_currentSpeed, 0, timeCurve * Time.deltaTime);
                    _currentSoundVolume = Mathf.Lerp(_currentSoundVolume, minSoundVolume, timeCurve * Time.deltaTime);
                }

                audioSource.pitch = engineSpeed.Evaluate(_currentSpeed);
                audioSource.volume = _currentSoundVolume;
            }
            else if(!_isNoDrive)
            {
                float pitch = audioSource.pitch;
                DOTween.To(() => pitch, x => audioSource.pitch = x, 0, engineFadingTime);
                DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, engineFadingTime);
                _isNoDrive = true;
            }
        }
    }
}
