using System.Collections.Generic;
using Menu.Upgrade;
using Sdk.Saving;
using UnityEngine;

namespace Menu.Car
{
    public class SwitchCar : Panel
    {
        [SerializeField] private UpgradePanel upgradePanel;
        [SerializeField] private Transform content;
        [SerializeField] private SwipeScroll swipeScroll;

        private List<CarItem> _carItems = new List<CarItem>();

        private void OnEnable()
        {
            for (int i = 0; i < content.childCount; i++)
                _carItems.Add(content.GetChild(i).GetComponent<CarItem>());
            
            foreach (var car in _carItems)
                car.Selected += Switch;
            
            Load();
        }
        
        private void OnDisable()
        {
            foreach (var car in _carItems)
                car.Selected -= Switch;
        }
        
        public override void ActivatorPanel(bool isActivate)
        {
            base.ActivatorPanel(isActivate);
            if(isActivate)
                Load();
        }

        private void Load()
        {
            bool isLoad = false;
            
            print(SavesFacade.Car);
            if (SavesFacade.Car != "None")
            {
                foreach (var car in _carItems)
                {
                    isLoad = car.Load(SavesFacade.Car) ? true : isLoad;
                }

                if (!isLoad)
                    _carItems[0].SelectedCar();
            }
            else
            {
                _carItems[0].SelectedCar();
            }
        }

        private void Switch(CarItem carItem)
        {
            carItem.SetPanel(true);

            for (int i = 0; i < _carItems.Count; i++)
            {
                if(_carItems[i].Car.typeCar != carItem.Car.typeCar)
                    _carItems[i].SetPanel(false);
                else
                    swipeScroll.Init(i);
            }

            SavesFacade.Car = carItem.Car.typeCar.ToString();
            Saves.Save();
            upgradePanel.SetCar(carItem.Car);
        }
    }
}
