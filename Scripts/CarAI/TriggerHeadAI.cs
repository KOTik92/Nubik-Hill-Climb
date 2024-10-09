using System.Collections;
using System.Collections.Generic;
using Game.Car.AI;
using UnityEngine;

namespace Game.Car.Noobik
{
    public class TriggerHeadAI : MonoBehaviour
    {
        [SerializeField] private Noobik noobik;
        [SerializeField] private AudioSource deathAudioSource;

        private bool _isDead;
        private CarAI _carAI;

        internal void SetCar(CarAI carAI)
        {
            _carAI = carAI;
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent(out LandSector landSector) && !_isDead)
            {
                deathAudioSource.Play();
                noobik.Dead();
                _carAI.Dead();
                _isDead = true;
            }
        }
    }
}
