using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float minTargetZ;
    [SerializeField] private float maxTargetZ;
    [SerializeField] private float smoothTime;

    private Vector2 _posCar;
    private Vector3 _targetPosition;
    private float _currentVelocityZ;

    private void Update()
    {
        _posCar = new Vector2(point.position.x, point.position.y);
        _targetPosition = new Vector3(_posCar.x + offset.x, _posCar.y + offset.y, transform.position.z);

        float targetZ = maxTargetZ;
        if (point.GetComponent<Rigidbody2D>().velocity.magnitude < 1f)
        {
            targetZ = minTargetZ;
        }
        
        _targetPosition.z = Mathf.SmoothDamp(transform.position.z, targetZ, ref _currentVelocityZ, smoothTime);

        transform.position = _targetPosition;
    }
}

