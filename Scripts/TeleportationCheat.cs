using System;
using System.Collections;
using Game.Car;
using UnityEngine;

public class TeleportationCheat : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private LoadCar loadCar;
    [SerializeField] private LoadMap loadMap;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            var x = loadCar.GetSelectedCar().Rigidbody.position.x + 250;
            var y = loadMap.SelectedMap.GetY(x) + 10;
            
            var position = loadCar.GetSelectedCar().Rigidbody.position;
            position.x = x; 
            position.y = y;
            
            loadCar.GetSelectedCar().Rigidbody.isKinematic = true;
            loadCar.GetSelectedCar().Rigidbody.position = position;
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            loadCar.GetSelectedCar().Rigidbody.isKinematic = false;
        }
    }
#endif
}