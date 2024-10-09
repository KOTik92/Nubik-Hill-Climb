using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Wallet
{
    public class WalletMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        private Wallet _wallet = new Wallet();
        
        public event Action MoneyAmountChanged;
        
        private IEnumerator Start()
        {
            UpdateText();
            while (true)
            {
                UpdateText();
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnEnable()
        {
            _wallet.MoneyAmountChanged += UpdateText;
        }
        
        private void OnDisable()
        {
            _wallet.MoneyAmountChanged -= UpdateText;
        }

        private void UpdateText()
        {
            text.text = _wallet.MoneyValue.ToString();
            MoneyAmountChanged?.Invoke();
        }

        public void TakeMoney(int money)
        {
            _wallet.MoneyValue -= money;
            UpdateText();
        }

        public int GetMoneyAmount() { return _wallet.MoneyValue; }
    }
}
