using System;
using Lean.Localization;
using Sdk.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wallet;

namespace Skin
{
    public class SkinItem : MonoBehaviour
    {
        [SerializeField] private TypeSkin typeSkin;
        [SerializeField] private int money;
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

        public TypeSkin TypeSkin => typeSkin;
        public event Action<SkinItem> Selected;

        private bool _isBuy;

        public void ClickButton()
        {
            if (_isBuy)
                SelectedSkin();
            else if (walletMenu.GetMoneyAmount() >= money)
            {
                walletMenu.TakeMoney(money);
                Analytics.OnBuy("Skin", typeSkin.ToString(), money, SavesFacade.TotalTries);
                SavesFacade.SetBuy("SkinBuy_" + typeSkin);
                SelectedSkin();
            }
        }
        
        internal void SelectedSkin()
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
                textMoney.text = money.ToString();
                
                selectedPanel.gameObject.SetActive(walletMenu.GetMoneyAmount() < money);
            }
        }

        internal bool Load(string name)
        {
            textName.text = LeanLocalization.GetTranslationText(typeSkin.ToString());
            _isBuy = SavesFacade.GetBuy("SkinBuy_" + typeSkin) || money == 0;
            SetButton();
            if (name == typeSkin.ToString() && _isBuy)
            {
                SelectedSkin();
                return true;
            }

            return false;
        }
    }
}
