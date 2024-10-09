using System;
using UnityEngine;

namespace Game.Car.Noobik
{
    [Serializable]
    public class NoobikSpring
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private HingeJoint2D hingeJoint;
        [SerializeField] private float springStrength = 1;
        [SerializeField] private float damperStrength = 1;
        [SerializeField] private bool isReset;

        public bool IsReset => isReset;
        public Vector3 StartPosition => _startPosition;
        public HingeJoint2D HingeJoint => hingeJoint;

        private Vector3 _startPosition;

        internal void Spring(Transform car)
        {
            float angleDifference = Vector2.SignedAngle(rb.transform.up, car.up);
            float springTorque = springStrength * angleDifference;
            float dampTorque = damperStrength * -rb.angularVelocity;
            
            rb.AddTorque(springTorque + dampTorque, ForceMode2D.Force);
        }

        internal void SetStartPosition(Transform transform)
        {
            if(!isReset)
                return;

            _startPosition = transform.localPosition;
        }
    }
}
