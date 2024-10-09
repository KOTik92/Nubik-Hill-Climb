using System;
using Game.Car;
using Game.Car.AI;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioSource audioSourceFirst;
    [SerializeField] private AudioSource audioSource;

    public event Action<bool, Flag> OnWin;
    
    private Car _car;
    private CarAI _carAI;
    private bool _isCheckCar;
    private bool _isFirst;

    public void Init(Car car, CarAI carAI)
    {
        _car = car;
        _carAI = carAI;
    }

    public void Activate()
    {
        _isCheckCar = true;
        _isFirst = true;
    }

    private void Update()
    {
        if (_isCheckCar)
        {
            if (_carAI != null)
            {
                if (_carAI.transform.position.x >= transform.position.x)
                    _isFirst = false;
            }

            if (_car.transform.position.x >= transform.position.x)
            {
                if(particleSystem.isStopped)
                    particleSystem.Play();
                
                if(_isFirst)
                    audioSourceFirst.Play();
                else
                    audioSource.Play();
                
                OnWin?.Invoke(_isFirst, this);
                _isCheckCar = false;
            }
        }
    }
}
