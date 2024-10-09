using TMPro;
using UnityEngine;

namespace Wallet
{
    public class WalletGame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;

        public int Money => _money;

        private int _startMoney;
        private int _money;
        private Wallet _wallet = new Wallet();

        private void Start()
        {
            _startMoney = _wallet.MoneyValue;
            moneyText.text = _startMoney.ToString();
        }

        internal void AddMoney(int money)
        {
            _money += money;
            int moneyAmount = _startMoney + _money;
            moneyText.text = moneyAmount.ToString();
        }

        public void Save()
        {
            _wallet.MoneyValue += _money;
            _money = 0;
            _startMoney = _wallet.MoneyValue;
        }
    }
}
