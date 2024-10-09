using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wallet;

namespace Menu.Upgrade
{
    [RequireComponent(typeof(Button))]
    public abstract class Upgrade : MonoBehaviour
    {
        [SerializeField] private GameObject panelButton;
        [SerializeField] private TextMeshProUGUI textMaxUpgrade;
        [Header("Money")] 
        [SerializeField] private WalletMenu walletMenu;
        [SerializeField] private TextMeshProUGUI textMoney;
        [Header("Level")] 
        [SerializeField] private Image slider;
        [SerializeField] private TextMeshProUGUI textLevel;

        public event Action OnBuy;

        protected int _upgradeLevel;
        protected int _maxLevel;
        protected CarData _carData;
        protected int _previousCost;
        protected int _cost;
        protected Button _button;

        protected virtual bool TryToUpgrade()
        {
            if (walletMenu.GetMoneyAmount() >= _cost)
            {
                _upgradeLevel++;
                walletMenu.TakeMoney(_cost);
                UpdateCost();
                UpdateView();
                OnBuy?.Invoke();
                return true;
            }

            return false;
        }

        private void ExecuteUpgrade()
        {
            TryToUpgrade();
        }

        internal void SetCar(CarData carData)
        {
            _carData = carData;
            Load();
        }

        public virtual void Load()
        {
            _button = GetComponent<Button>();
            UpdateCost();
            UpdateView();
        }

        private void Start()
        {
            _button.onClick.AddListener(ExecuteUpgrade);
        }

        private void OnEnable()
        {
            walletMenu.MoneyAmountChanged += UpdateView;
        }
        
        private void OnDisable()
        {
            walletMenu.MoneyAmountChanged -= UpdateView;
        }

        protected void UpdateView()
        {
            UpdateLevelIndicators();

            var isMaxLevel = _upgradeLevel >= _maxLevel;

            panelButton.SetActive(isMaxLevel);
            textMaxUpgrade.gameObject.SetActive(isMaxLevel);
            textMoney.gameObject.SetActive(!isMaxLevel);
            _button.interactable = !isMaxLevel;
            
            textMoney.text = _cost.ToString();
            
            panelButton.SetActive(walletMenu.GetMoneyAmount() < _cost);
        }

        private void UpdateLevelIndicators()
        {
            slider.fillAmount = (float)_upgradeLevel / _maxLevel;
            textLevel.text = $"{_upgradeLevel}/{_maxLevel}";
        }

        protected abstract void UpdateCost();
    }
}
