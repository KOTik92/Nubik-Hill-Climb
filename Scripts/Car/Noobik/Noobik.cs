using DG.Tweening;
using UnityEngine;

namespace Game.Car.Noobik
{
    public class Noobik : MonoBehaviour
    {
        [SerializeField] private GameObject[] legs;
        [SerializeField] private NoobikSpring[] noobikSprings;
        [Header("Head")]
        [SerializeField] private float maxTimeRotationHead;
        [SerializeField] private Transform head;
        
        private Transform _point;
        private bool _isDead;
        private float _timeRotationHead;
        private bool _isHead;
        private Tweener _headTweener;
        
        internal void SetCar(Transform car, bool isLegs, Rigidbody2D rigidbody)
        {
            foreach (var leg in legs)
                leg.SetActive(isLegs);

            _point = car;

            foreach (var noobikSpring in noobikSprings)
            {
                if (noobikSpring.HingeJoint.connectedBody == null)
                {
                    transform.position = _point.position;
                    noobikSpring.SetStartPosition(noobikSpring.HingeJoint.transform);
                    noobikSpring.HingeJoint.connectedBody = rigidbody;
                }
            }
        }

        internal void Reset()
        {
            transform.position = _point.position;
            
            foreach (var noobikSpring in noobikSprings)
            {
                if (noobikSpring.IsReset)
                {
                    noobikSpring.HingeJoint.transform.localPosition = noobikSpring.StartPosition;
                }
            }
        }

        private void OnDisable()
        {
            _headTweener?.Kill();
            _headTweener = null;
        }

        private void FixedUpdate()
        {
            if (_isDead)
                return;
        
            if (_point != null)
            {
                foreach (var noobikSpring in noobikSprings)
                    noobikSpring.Spring(_point);
            }

            if (_timeRotationHead >= maxTimeRotationHead && !_isHead)
            {
                _headTweener = head.DOLocalRotate(new Vector3(0, 64, 0), 0.5f).OnComplete(() =>
                {
                    _headTweener = head.DOLocalRotate(new Vector3(0, 0, 0), 0.5f).OnComplete(() =>
                    {
                        _timeRotationHead = 0;
                        _isHead = false;
                    }).SetDelay(2);
                });
                
                _isHead = true;
            }
            else if (_timeRotationHead < maxTimeRotationHead && !_isHead)
                _timeRotationHead += Time.fixedDeltaTime;
        }

        internal void Dead()
        {
            _isDead = true;
        }
    }
}
