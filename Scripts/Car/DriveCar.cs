using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Car
{
    [Serializable]
    public class DriveCar
    {
        [SerializeField] private Rigidbody2D[] wheels;
        [SerializeField] private Rigidbody2D carRB;
        [SerializeField] private CarSound carSound = new CarSound();
        [Header("Drive")]
        [SerializeField] private PhysicsMaterial2D physicsMaterial2D;
        [Header("Rotation")]
        [SerializeField] private ColliderIsGround[] collidersIsGround;
        [SerializeField] private Transform pointRaycast;
        [SerializeField] private float lengthRaycast;
        [Header("Particles")]
        [SerializeField] private ParticleSystem[] groundParticles;
        [SerializeField] private float minDeltaRotation = 1f;
        [Header("Wheels")]
        [SerializeField] private float _wheelsRadius;
        [SerializeField] private float _scoopingThreshold = 0.01f;
        [SerializeField] private float _maxParticlesPerFrame = 0.5f;

        public event Action IsFlipLeft;
        public event Action IsFlipRight;
        public event Action<bool> IsGround;

        public int AmountFlipLeft => _amountFlipLeft;
        public int AmountFlipRight => _amountFlipRight;
        
        private float _speed;
        private float _rotationSpeed;
        private float _grip;
        private Transform _transform;
        private float _initialRotation;
        private bool _isGrounded;
        private bool _isFlipping;
        private int _amountFlipLeft;
        private int _amountFlipRight;
        private Vector2[] _lastWheelPositions;
        private float[] _lastWheelRotations;
        private float[] _particleProgress;

        internal void Init(Transform transform)
        {
            _transform = transform;

            physicsMaterial2D.friction = _grip;
            _lastWheelPositions = new Vector2[wheels.Length];
            _lastWheelRotations = new float[wheels.Length];
            _particleProgress = new float[wheels.Length];

            for (int i = 0; i < wheels.Length; i++)
            {
                _lastWheelPositions[i] = wheels[i].transform.position;
                _lastWheelRotations[i] = wheels[i].transform.rotation.eulerAngles.x;
                
                wheels[i].sharedMaterial = physicsMaterial2D;
                if (wheels[i].TryGetComponent(out Collider2D collider)) collider.sharedMaterial = physicsMaterial2D;
            }
        }

        internal void Drive(float axis, bool isDrive)
        {
            carSound.EngineSound(isDrive, axis);

            if(!isDrive)
                return;
            
            foreach (var tire in wheels)
                tire.AddTorque(-axis * _speed * Time.deltaTime);

            Rotate(axis);
            UpdateScoopingParticles(axis);
        }

        private void UpdateScoopingParticles(float axis)
        {
            // get tires collisions 
            var collisions = new List<ContactPoint2D>();

            for (var i = 0; i < wheels.Length; i++)
            {
                var tire = wheels[i];
                var contacts = new ContactPoint2D[3];
                var count = tire.GetContacts(contacts);
                for (int j = 0; j < count; j++)
                {
                    collisions.Add(contacts[j]);
                }
                
                if (!IsShouldPlayParticles(i))
                {
                    StopParticles(i);
                    continue;
                }

                if (count == 0)
                {
                    StopParticles(i);
                    continue;
                }
                
                var averageNormal = Vector2.zero;
                foreach (var contact in contacts)
                {
                    averageNormal += contact.normal;
                }
                
                averageNormal /= count;
                
                groundParticles[i].transform.up = averageNormal;
                groundParticles[i].transform.forward = -_transform.right * Mathf.Sign(axis);
                PlayParticles(i);
            }
        }
        
        private void PlayParticles(int wheelIndex)
        {
            if (_particleProgress[wheelIndex] >= 1f)
            {
                groundParticles[wheelIndex].Emit(1);
                _particleProgress[wheelIndex] -= 1f;
            }
            else
            {
                _particleProgress[wheelIndex] += _maxParticlesPerFrame;
            }
        }
        
        private void StopParticles(int wheelIndex)
        {
            groundParticles[wheelIndex].Stop();
        }

        private bool IsShouldPlayParticles(int wheelIndex)
        {
            var lastWheelPosition = _lastWheelPositions[wheelIndex];
            var lastWheelRotation = _lastWheelRotations[wheelIndex];
            
            Vector2 currentWheelPosition = wheels[wheelIndex].position;
            float deltaMagnitude = Vector2.Distance(currentWheelPosition, lastWheelPosition);
            
            float currentWheelRotation = wheels[wheelIndex].rotation;
            float deltaRotation = Mathf.Abs(currentWheelRotation - lastWheelRotation);
            
            float arcLength = 2f * Mathf.PI * _wheelsRadius * (deltaRotation / 360f);
            
            _lastWheelPositions[wheelIndex] = currentWheelPosition;
            _lastWheelRotations[wheelIndex] = currentWheelRotation;
            
            return Mathf.Abs(arcLength - deltaMagnitude) > _scoopingThreshold;
        }

        private void Rotate(float axis)
        {
            RaycastHit2D hit = Physics2D.Raycast(pointRaycast.position, -_transform.up, lengthRaycast);
            Debug.DrawRay(pointRaycast.position, -_transform.up * lengthRaycast, Color.red);
            if (hit.collider == null)
            {
                _isGrounded = false;
                
                foreach (var collider in collidersIsGround)
                {
                    if (collider.IsGround)
                        _isGrounded = true;
                }

                carRB.AddTorque(axis * _rotationSpeed * Time.deltaTime);
                
                if (Mathf.Abs(carRB.rotation - _initialRotation) >= 200f)
                {
                    _isFlipping = true;
                }
            }
            else
            {
                _initialRotation = carRB.rotation;
                _isGrounded = true;
            }

            IsGround?.Invoke(_isGrounded);
            
            if (_isFlipping)
            {
                float torqueDirection =
                    Mathf.Sign(carRB.angularVelocity);

                if (torqueDirection < 0)
                {
                    _amountFlipRight++;
                    IsFlipRight?.Invoke();
                }
                else if (torqueDirection > 0)
                {
                    _amountFlipLeft++;
                    IsFlipLeft?.Invoke();
                }
                
                _initialRotation = carRB.rotation;
                _isFlipping = false;
            }
        }

        internal void AddSpeed(float speed)
        {
            _speed = speed;
        }

        internal void AddRotationSpeed(float rotationSpeed)
        {
            _rotationSpeed = rotationSpeed;
        }

        internal void AddGrip(float grip)
        {
            _grip = grip;
            physicsMaterial2D.friction = _grip;

            foreach (var tire in wheels)
            {
                tire.sharedMaterial = physicsMaterial2D;
                if (tire.TryGetComponent(out Collider2D collider)) collider.sharedMaterial = physicsMaterial2D;
            }
        }
    }
}
