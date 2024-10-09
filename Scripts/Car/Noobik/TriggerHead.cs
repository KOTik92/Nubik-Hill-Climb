using System;
using Sdk.Saving;
using Skin;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Car.Noobik
{
    public class TriggerHead : MonoBehaviour
    {
        [SerializeField] private LoadCar loadCar;
        [SerializeField] private Noobik noobik;
        [SerializeField] private string[] womanSkinsNames;
        [SerializeField] private AudioSource womanDeathAudioSource;
        [SerializeField] private AudioSource manDeathAudioSource;
        
        private bool _isDead;
        private bool _isWoman;

        private void Start()
        {
            var skinName = SavesFacade.Skin;
            _isWoman = Array.Exists(womanSkinsNames, name => name == skinName);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent(out LandSector landSector) && !_isDead)
            {
                (_isWoman ? womanDeathAudioSource : manDeathAudioSource).Play();
                noobik.Dead();
                loadCar.GetSelectedCar().Dead();
                _isDead = true;
            }
        }
    }
}
