using System.Collections.Generic;
using Game.Car;
using Game.Car.AI;
using UnityEngine;

public class MotionRecording : MonoBehaviour
{
    [SerializeField] private LoadCar loadCar;
    [SerializeField] private List<Settings> settings;

    private int _fixedUpdateFrameNumber = 0;
    private int _axis;

    private void Start()
    {
        loadCar.SelectedCar.InputCar.OnInput += SetMove;
    }
    
    private void OnDisable()
    {
        loadCar.SelectedCar.InputCar.OnInput -= SetMove;
    }
    
    private void FixedUpdate()
    {
        _fixedUpdateFrameNumber++;
    }
    
    private void SetMove(int axis, bool isDouble)
    {
        if (_axis != axis)
        {
            Settings tempSetting = new Settings();
            tempSetting.fixedUpdateFrameNumber = _fixedUpdateFrameNumber;
            tempSetting.movement = axis;
            settings.Add(tempSetting);
            
            _axis = axis;
        }
    }
}
