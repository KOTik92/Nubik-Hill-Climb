using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float parallaxFactor;

    private Vector3 _startPos;
    private float _xOffset;
    private float _tempPos;
    private bool _isRight;
    
    public void Init()
    {
        _startPos = transform.localPosition;
    }
    
    public void Move(float playerPosX)
    {
        float distX = (playerPosX - _startPos.x) * parallaxFactor;
        
        transform.localPosition = new Vector3(_startPos.x + distX + _xOffset, transform.localPosition.y, transform.localPosition.z);

        if ((transform.localPosition.x - transform.localScale.x) < 0 && playerPosX > _tempPos)
        {
            _xOffset += transform.localScale.x;
        }
        else if ((transform.localPosition.x + transform.localScale.x) > 0 && playerPosX < _tempPos)
        {
            _xOffset -= transform.localScale.x;
        }

        _tempPos = playerPosX;
    }
}
