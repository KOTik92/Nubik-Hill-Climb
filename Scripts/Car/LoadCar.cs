using Cinemachine;
using Sdk.Saving;
using UnityEngine;

namespace Game.Car
{
    public class LoadCar : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private Car[] cars;
        [SerializeField] private Noobik.Noobik noobik;

        public int SpeedUpgradeLevel => _speedUpgradeLevel;
        public int RotationSpeedUpgradeLevel => _rotationSpeedUpgradeLevel;
        public int GripUpgradeLevel => _gripUpgradeLevel;
        public Car SelectedCar => _selectedCar;
        
        private Car _selectedCar;
        private int _speedUpgradeLevel;
        private int _rotationSpeedUpgradeLevel;
        private int _gripUpgradeLevel;

        public void Init()
        {
            foreach (var car in cars)
                car.Init();
            
            Load();
        }

        private void Load()
        {
            string carType = SavesFacade.Car;
            
            if (carType != "None")
            {
                for (int i = 0; i < cars.Length; i++)
                {
                    if (cars[i].CarData.typeCar.ToString() == carType)
                    {
                        _selectedCar = cars[i];
                        _selectedCar.gameObject.SetActive(true);
                        
                        var selectedCarTransform = _selectedCar.transform;
                        cinemachineVirtualCamera.Follow = selectedCarTransform;
                        cinemachineVirtualCamera.LookAt = selectedCarTransform;
                        (Transform, bool) field = _selectedCar.GetNoobik();
                        noobik.SetCar(field.Item1, field.Item2, _selectedCar.Rigidbody);
                        SetCharacteristics();
                    }
                    else
                        cars[i].gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (var car in cars)
                    car.gameObject.SetActive(false);
                
                _selectedCar = cars[0];
                _selectedCar.gameObject.SetActive(true);
                
                var selectedCarTransform = _selectedCar.transform;
                cinemachineVirtualCamera.Follow = selectedCarTransform;
                cinemachineVirtualCamera.LookAt = selectedCarTransform;
                (Transform, bool) field = _selectedCar.GetNoobik();
                noobik.SetCar(field.Item1, field.Item2, _selectedCar.Rigidbody);
                SetCharacteristics();
            }
        }

        private void SetCharacteristics()
        {
            _speedUpgradeLevel = SavesFacade.GetUpgrade("Upgrade_Engine_" + _selectedCar.CarData.typeCar);
            float speedUpgrade = _selectedCar.CarData.speedUpgradeStep * _speedUpgradeLevel;
            float speed = _selectedCar.CarData.initialSpeed + speedUpgrade;
            _selectedCar.DriveCar.AddSpeed(speed);

            _rotationSpeedUpgradeLevel = SavesFacade.GetUpgrade("Upgrade_Rotation_" + _selectedCar.CarData.typeCar);
            float speedRotationUpgrade = _selectedCar.CarData.rotationSpeedUpgradeStep * _rotationSpeedUpgradeLevel;
            float speedRotation = _selectedCar.CarData.initialRotationSpeed + speedRotationUpgrade;
            _selectedCar.DriveCar.AddRotationSpeed(speedRotation);

            _gripUpgradeLevel = SavesFacade.GetUpgrade("Upgrade_Tires_" + _selectedCar.CarData.typeCar);
            float gripUpgrade = _selectedCar.CarData.gripUpgradeStep * _gripUpgradeLevel;
            float grip = _selectedCar.CarData.initialGrip + gripUpgrade;
            _selectedCar.DriveCar.AddGrip(grip);
        }

        public Car GetSelectedCar()
        {
            return _selectedCar;
        }
    }
}
