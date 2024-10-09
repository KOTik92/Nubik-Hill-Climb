using System;
using Game.Car;
using UnityEngine;

public class ItemFuel : Item
{
    [SerializeField] private float fuel;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject obj;

    private bool _isTake;

    private void OnEnable()
    {
        _isTake = false;
        obj.SetActive(true);
        audioSource.Stop();
        if(particleSystem.isPlaying)
            particleSystem.Stop();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out Car car) && !_isTake)
        {
            if (car.AddFuel(fuel))
            {
                OnTake();
                _isTake = true;
                EffectTake();
            }
        }
    }

    private void EffectTake()
    {
        obj.SetActive(false);
        audioSource.Play();
        if(particleSystem.isStopped)
            particleSystem.Play();
        
        Invoke("DeactivateItem", 1);
    }

    private void DeactivateItem()
    {
        gameObject.SetActive(false);
    }
}