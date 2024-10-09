using System;
using Game.Car;
using Game.Car.AI;
using Game.Car.Noobik;
using Sdk.RemoteConfig;
using Skin;
using UnityEngine;

[Serializable]
public class LoadCarAI
{
    [SerializeField] private CarAI[] carsAI;
    [SerializeField] private Noobik noobikAI;
    [SerializeField] private LoadSkin loadSkinAI;
    [SerializeField] private TriggerHeadAI triggerHeadAI;
    [SerializeField] private LoadCar loadCar;

    public CarAI SelectedCarAI => _selectedCarAI;
    
    private CarAI _selectedCarAI;
    
    public void Load(MapGenerator selectedMap)
    {
        if(!FlagController.IsAIEnabled)
            return;
        
        foreach (var carAI in carsAI)
        {
            if (carAI.typeCar == selectedMap.MapData.typeCar)
            {
                carAI.gameObject.SetActive(true);
                noobikAI.gameObject.SetActive(true);
                carAI.Init(selectedMap.MapData.speed, selectedMap.MapData.speedRotation, selectedMap.MapData.grip, loadCar.SelectedCar);
                (Transform, bool) fields = carAI.GetNoobik();
                noobikAI.SetCar(fields.Item1, fields.Item2, carAI.Rigidbody);
                triggerHeadAI.SetCar(carAI);
                loadSkinAI.SetSkin(selectedMap.MapData.typeSkin.ToString());
                carAI.InputCarAI.SetSettings(selectedMap.MapData.settings);
                loadCar.SelectedCar.InputCar.OnInput += StartMoving;

                _selectedCarAI = carAI;
            }
            else
                carAI.gameObject.SetActive(false);
        }
        
        if(selectedMap == null)
            noobikAI.gameObject.SetActive(false);
    }

    private void StartMoving(int axis, bool isDouble)
    {
        if (axis != 0)
        {
            _selectedCarAI.StartMoving();
            loadCar.SelectedCar.InputCar.OnInput -= StartMoving;
        }
    }
}
