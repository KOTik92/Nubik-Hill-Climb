using Game;
using Game.Car;
using Lean.Localization;
using UnityEngine;

namespace Wallet
{
    public class MoneyPlayer : MonoBehaviour
    {
        [SerializeField] private WalletGame walletGame;
        [SerializeField] private Car car;
        [Header("Flip")] 
        [SerializeField] private int moneyFlipLeft;
        [SerializeField] private int moneyFlipRight;
        [SerializeField] private TextMoney textMoney;
        [Header("Air")]
        [SerializeField] private int moneyAir;
        [SerializeField] private float timeAir;
        [SerializeField] private TextMoney textMoneyAir;

        public float MaxTimeAir => _maxTimeAir;
        
        private bool _isDead;
        private int _moneyAir;
        private float _timeAir;
        private float _maxTimeAir;
        private bool _isAir;

        private void OnEnable()
        {
            car.IsDead += Dead;
            car.DriveCar.IsFlipLeft += FlipLeft;
            car.DriveCar.IsFlipRight += FlipRight;
            car.DriveCar.IsGround += MoneyAir;
        }

        private void OnDisable()
        {
            car.IsDead -= Dead;
            car.DriveCar.IsFlipLeft -= FlipLeft;
            car.DriveCar.IsFlipRight -= FlipRight;
            car.DriveCar.IsGround -= MoneyAir;
        }

        internal bool AddMoney(int amount)
        {
            if (_isDead || car.DeadType == "Death")
                return false;
            
            walletGame.AddMoney(amount);
            return true;
        }

        private void FlipLeft()
        {
            walletGame.AddMoney(moneyFlipLeft);
            textMoney.ActivateText(LeanLocalization.GetTranslationText("backflip") + " +" + moneyFlipLeft);
        }
        
        private void FlipRight()
        {
            walletGame.AddMoney(moneyFlipRight);
            textMoney.ActivateText(LeanLocalization.GetTranslationText("forwardflip") + " +" + moneyFlipLeft);
        }

        private void MoneyAir(bool isGround)
        {
            if (!isGround)
            {
                if (!_isAir)
                    _timeAir = 0;
                
                _isAir = true;
                _timeAir += Time.deltaTime;

                if (_timeAir >= timeAir)
                {
                    _moneyAir += moneyAir;
                    _maxTimeAir += _timeAir;
                    _timeAir = 0;
                }
            }
            else
            {
                if (_isAir && _moneyAir != 0)
                {
                    walletGame.AddMoney(_moneyAir);
                    textMoneyAir.ActivateText(LeanLocalization.GetTranslationText("air") + " +" + _moneyAir);
                    _moneyAir = 0;
                    _maxTimeAir += _timeAir;
                }

                _isAir = false;
            }
        }

        private void Dead()
        {
            _maxTimeAir += _timeAir;
            _isDead = true;
            car.DriveCar.IsFlipLeft -= FlipLeft;
            car.DriveCar.IsFlipRight -= FlipRight;
            car.DriveCar.IsGround -= MoneyAir;
        }
    }
}
