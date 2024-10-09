using System;
using UnityEngine;

namespace Game.Car
{
    [Serializable]
    public class InputCar
    {
        public event Action<int, bool> OnInput;
        
        private bool _isD => Input.GetKey(KeyCode.D);
        private bool _isA => Input.GetKey(KeyCode.A);
        private bool _isLeftArrow => Input.GetKey(KeyCode.LeftArrow);
        private bool _isRightArrow => Input.GetKey(KeyCode.RightArrow);
        
        private bool _isLeft;
        private bool _isRight;
        private bool _isDouble;

        internal int CheckInput()
        {
            int axis = 0;
            _isDouble = false;
            
            if (_isD || _isRightArrow || _isRight)
                axis = 1;
            if (_isA || _isLeftArrow || _isLeft)
                axis = -1;
            if ((_isD && _isA) || (_isRightArrow && _isLeftArrow) || (_isRight && _isLeft))
            {
                _isDouble = true;
                axis = 0;
            }

            OnInput?.Invoke(axis, _isDouble);
            return axis;
        }

        internal void SetMovement(bool isLeft, bool isRight)
        {
            _isLeft = isLeft;
            _isRight = isRight;
        }
    }
}
