using System;
using Lean.Localization;
using Sdk.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wallet;

namespace Menu.Car
{
    public class CarItem : MonoBehaviour
    {
        [SerializeField] private CarData carData;
        [SerializeField] private TextMeshProUGUI textName;
        [Space] 
        [SerializeField] private Image button;
        [SerializeField] private Sprite spriteButtonUse;
        [SerializeField] private Sprite spriteButtonBuy;
        [SerializeField] private GameObject selectedPanel;
        [SerializeField] private TextMeshProUGUI textUse;
        [Header("Money")]
        [SerializeField] private WalletMenu walletMenu;
        [SerializeField] private TextMeshProUGUI textMoney;

        public CarData Car => carData;
        public event Action<CarItem> Selected;

        private bool _isBuy;

        public void ClickButton()
        {
            if(_isBuy)
                SelectedCar();
            else if(walletMenu.GetMoneyAmount() >= carData.cost)
            {
                walletMenu.TakeMoney(carData.cost);
                Analytics.OnBuy("Car", carData.typeCar.ToString(), carData.cost, SavesFacade.TotalTries);
                SavesFacade.SetBuy("CarBuy_" + carData.typeCar);
                SelectedCar();
            }
        }

        private void OnEnable()
        {
            LeanLocalization.OnLocalizationChanged += UpdateLocalization;
        }
        
        private void OnDisable()
        {
            LeanLocalization.OnLocalizationChanged -= UpdateLocalization;
        }

        internal void SelectedCar()
        {
            _isBuy = true;
            Selected?.Invoke(this);
            SetButton();
        }

        internal void SetPanel(bool isSelected)
        {
            if (!_isBuy)
            {
                SetButton();
                return;
            }
            
            selectedPanel.gameObject.SetActive(isSelected);
            textUse.text = isSelected
                ? LeanLocalization.GetTranslationText("Selected")
                : LeanLocalization.GetTranslationText("Choose");
        }

        private void SetButton()
        {
            if (_isBuy)
            {
                button.sprite = spriteButtonUse;
                textUse.gameObject.SetActive(true);
                textMoney.gameObject.SetActive(false);
            }
            else
            {
                button.sprite = spriteButtonBuy;
                textUse.gameObject.SetActive(false);
                textMoney.gameObject.SetActive(true);
                textMoney.text = carData.cost.ToString();
                
                selectedPanel.gameObject.SetActive(walletMenu.GetMoneyAmount() < carData.cost);
            }
        }

        internal bool Load(string typeCar)
        {
            UpdateLocalization();
            
            _isBuy = SavesFacade.GetBuy("CarBuy_" + carData.typeCar) || carData.cost == 0;
            SetButton(); 
            if (typeCar == carData.typeCar.ToString() && _isBuy)
            {
                SelectedCar();
                return true;
            }

            return false;
        }
        
        private void UpdateLocalization()
        {
            textName.text = LeanLocalization.GetTranslationText(carData.typeCar.ToString());
            textUse.text = LeanLocalization.GetTranslationText("Choose");
        }
    }
}
