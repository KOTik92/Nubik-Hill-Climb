using System;
using Lean.Localization;
using Sdk.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wallet;

namespace Menu.Map
{
    public class MapItem : MonoBehaviour
    {
        [SerializeField] private MapData mapData;
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

        public TypeMap TypeMap => mapData.typeMap;
        public event Action<MapItem> Selected;
        
        private bool _isBought;
        
        public void ClickButton()
        {
            if (_isBought)
                SelectMap();
            else if (walletMenu.GetMoneyAmount() >= mapData.money)
            {
                walletMenu.TakeMoney(mapData.money);
                Analytics.OnBuy("Level", mapData.typeMap.ToString(), mapData.money, SavesFacade.TotalTries);
                SavesFacade.SetBuy("MapBuy_" + mapData.typeMap);
                SelectMap();
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
        
        internal void SelectMap()
        {
            _isBought = true;
            Selected?.Invoke(this);
            SetButton();
        }

        internal void SetPanel(bool isSelected)
        {
            if (!_isBought)
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
            if (_isBought)
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
                textMoney.text = mapData.money.ToString();
                
                selectedPanel.gameObject.SetActive(walletMenu.GetMoneyAmount() < mapData.money);
            }
        }

        internal bool Load(string typeMap)
        {
            UpdateLocalization();
            _isBought = SavesFacade.GetBuy("MapBuy_" + mapData.typeMap) || mapData.money == 0;
            SetButton();
            if (typeMap == mapData.typeMap.ToString() && _isBought)
            {
                SelectMap();
                return true;
            }

            return false;
        }
        
        private void UpdateLocalization()
        {
            textName.text = LeanLocalization.GetTranslationText(mapData.typeMap.ToString());
        }
    }
}
