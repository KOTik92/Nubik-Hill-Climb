using UnityEngine;

namespace Game.Car.AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CarAI : MonoBehaviour
    {
        [SerializeField] internal TypeCar typeCar;
        
        [SerializeField] private DriveCar driveCar = new DriveCar();
        [SerializeField] private InputCarAI inputCarAI = new InputCarAI();
        [SerializeField] private float maxDistanceFromPlayer;
        [Header("Noobik")] 
        [SerializeField] private Transform pointNoobik;
        [SerializeField] private bool isLegs;

        public Rigidbody2D Rigidbody => _rigidbody;
        
        private bool _isDrive;
        private Rigidbody2D _rigidbody;
        private Car _car;
        private float _distance;

        public InputCarAI InputCarAI => inputCarAI;
        
        internal void Init(float speed, float rotationSpeed, float grip, Car car)
        {
            driveCar.Init(transform);
            _rigidbody = GetComponent<Rigidbody2D>();
            _car = car;

            driveCar.AddSpeed(speed);
            driveCar.AddRotationSpeed(rotationSpeed);
            driveCar.AddGrip(grip);
        }

        internal void StartMoving()
        {
            _isDrive = true;
        }

        private void FixedUpdate()
        {
            if(!_isDrive || _distance >= maxDistanceFromPlayer)
                return;
            
            inputCarAI.AddFixedUpdateFrameNumber();
        }

        private void Update()
        {
            if (transform.position.x > _car.transform.position.x)
            {
                _distance = Vector3.Distance(transform.position, _car.transform.position);

                if (_distance >= maxDistanceFromPlayer)
                    inputCarAI.SetIgnoreSettings(true);
            }
            
            driveCar.Drive(_distance >= maxDistanceFromPlayer ? 0 : inputCarAI.GetMovement(), _isDrive);
        }
        
        internal (Transform, bool) GetNoobik() { return (pointNoobik, isLegs); }

        internal void Dead()
        {
            _isDrive = false;
        }
    }
}
