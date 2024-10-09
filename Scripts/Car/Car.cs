using System;
using UnityEngine;

namespace Game.Car
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Car : MonoBehaviour
    {
        [SerializeField] private CarData carData;
        [SerializeField] private DriveCar driveCar = new DriveCar();
        [SerializeField] private FuelCar fuelCar = new FuelCar();
        [Header("Dead")]
        [SerializeField] private float startTimeNoFuel;
        [SerializeField] private float startTimeDead;
        [Header("Noobik")] 
        [SerializeField] private Transform pointNoobik;
        [SerializeField] private bool isLegs;
        [Header("Message Slip")] 
        [SerializeField] private Message message;
        [SerializeField] private Transform pointMessage;
        [SerializeField] private float maxTimeSlip;

        private InputCar _inputCar = new InputCar();
        private bool _isDead;
        private bool _isDrive;
        private float _timeDead;
        private Rigidbody2D _rigidbody;
        private string _deadType;
        private float _timeSlip;
        private bool _isSlip;
        private Vector3 _lastCarPosition;
        
        public Rigidbody2D Rigidbody => _rigidbody;
        public CarData CarData => carData;
        public InputCar InputCar => _inputCar;
        public DriveCar DriveCar => driveCar;
        public string DeadType => _deadType;
        
        
        public event Action IsDead;
        
        internal void Init()
        {
            _isDrive = true;
            _rigidbody = GetComponent<Rigidbody2D>();
            _lastCarPosition = transform.position;

            driveCar.Init(transform);
            fuelCar.Init();
        }

        private void Update()
        {
            driveCar.Drive(_inputCar.CheckInput(), _isDrive);

            if (!_isDrive)
            {
                TimerDead();
                return;
            }

            if (_inputCar.CheckInput() >= 1)
                CheckTimeSlip();
            else
                _timeSlip = 0;

            if(fuelCar.Fuel > 0)
                fuelCar.FuelResuction();
            else
                NoFuel();
                
        }
        
        private void CheckTimeSlip()
        {
            _timeSlip += Time.deltaTime;
            
            if (_timeSlip >= maxTimeSlip)
            {
                if (_lastCarPosition.x + 5 <= transform.position.x)
                {
                    _lastCarPosition = transform.position;
                    _timeSlip = 0;
                }
                else
                {
                    if (!message.gameObject.activeSelf)
                    {
                        message.SetPoint(pointMessage);
                        message.gameObject.SetActive(true);
                    }

                    _timeSlip = 0;
                }
            }
        }

        private void TimerDead()
        {
            if(_isDead)
                return;
            
            if (_timeDead > 0)
                _timeDead -= Time.deltaTime;
            else
            {
                _timeDead = 0;
                IsDead?.Invoke();
                _isDead = true;
            }
        }
        
        private void NoFuel()
        {
            _deadType = "OutOfFuel";
            _timeDead = startTimeNoFuel;
            _isDrive = false;
        }
        
        internal void Dead()
        {
            _deadType = "Death";
            _timeDead = startTimeDead;
            _isDrive = false;
        }

        public bool AddFuel(float fuel)
        {
            if (_isDead || _deadType == "Death")
                return false;
            
            fuelCar.AddFuel(fuel);
            _isDrive = true;
            return true;
        }

        internal (Transform, bool) GetNoobik() { return (pointNoobik, isLegs); }
    }
}
