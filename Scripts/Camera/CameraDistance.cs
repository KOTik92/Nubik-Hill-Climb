using Cinemachine;
using UnityEngine;

namespace Game
{
    public class CameraDistance : MonoBehaviour
    {
        [SerializeField] private float minTargetZ;
        [SerializeField] private float maxTargetZ;
        [SerializeField] private float smoothTime;
        [SerializeField] private float maxSpeed;

        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineTransposer _cinemachine;
        private float _currentVelocityZ;
        private Rigidbody2D _targetRigidbody;

        private void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cinemachine = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _targetRigidbody = _virtualCamera.Follow.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var magnitude = _targetRigidbody.velocity.magnitude;
            var t = Mathf.InverseLerp(0, maxSpeed, magnitude);
            var targetZ = Mathf.Lerp(minTargetZ, maxTargetZ, t);
            _cinemachine.m_FollowOffset.z = Mathf.SmoothDamp(_cinemachine.m_FollowOffset.z, targetZ, ref _currentVelocityZ, smoothTime, maxSpeed);
        }
    }
}
