using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Car
{
    [Serializable]
    public class FuelCar
    {
        [SerializeField] private float maxFuel;
        [SerializeField] private float speed;
        [SerializeField] private Image slider;
        [SerializeField] private TextMeshProUGUI text;

        internal float Fuel
        {
            get { return _fuel; }
            set
            {
                _fuel = value;
                slider.fillAmount = value / maxFuel;
                text.text = $"{value.ToString("0")}/{maxFuel}";
            }
        }
        
        private float _fuel;

        internal void Init()
        {
            Fuel = maxFuel;
        }

        internal void FuelResuction()
        {
            Fuel -= Time.deltaTime * speed;
        }

        internal void AddFuel(float fuel)
        {
            if (Fuel + fuel > maxFuel)
                Fuel = maxFuel;
            else
                Fuel += fuel;
        }
    }
}
