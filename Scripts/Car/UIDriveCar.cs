using UnityEngine;

namespace Game.Car
{
    public class UIDriveCar : MonoBehaviour
    {
        [SerializeField] private SwitchSprite rightSwitchSprite;
        [SerializeField] private SwitchSprite leftSwitchSprite;

        private Car _car;
        private bool _isLeft;
        private bool _isRight;
        
        public void Init(Car car)
        {
            _car = car;
            _car.InputCar.OnInput += SwitchSprites;
        }

        private void OnDisable()
        {
            _car.InputCar.OnInput -= SwitchSprites;
        }

        public void ClickSetLeft(bool isLeft)
        {
            _isLeft = isLeft;
            SetMovement();
        }
        
        public void ClickSetRight(bool isRight)
        {
            _isRight = isRight;
            SetMovement();
        }

        private void SetMovement()
        {
            _car.InputCar.SetMovement(_isLeft, _isRight);
        }

        private void SwitchSprites(int axis, bool isDouble)
        {
            if (isDouble)
            {
                rightSwitchSprite.Switch(true);
                leftSwitchSprite.Switch(true);
                return;
            }
            
            
            rightSwitchSprite.Switch(axis > 0);
            leftSwitchSprite.Switch(axis < 0);
        }
    }
}
