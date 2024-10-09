using System.Collections;
using DG.Tweening;
using Sdk.AdController;
using Sdk.RemoteConfig;
using Sdk.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wallet;

namespace Game
{
    public class Dead : MonoBehaviour
    {
        [SerializeField] private Car.LoadCar loadCar;
        [SerializeField] private WalletGame walletGame;
        [SerializeField] private Meters meters;
        [Space] 
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI textMoney;
        [SerializeField] private TextMeshProUGUI textMeters;
        [SerializeField] private TextMeshProUGUI textFlipLeft;
        [SerializeField] private TextMeshProUGUI textFlipRight;
        [SerializeField] private TextMeshProUGUI textTimeAir;
        [SerializeField] private GameManager gameManager;
        [Space]
        [SerializeField] private Button takeMoneyButton;
        [SerializeField] private TextMeshProUGUI textTakeMoney;
        [SerializeField] private Button rewardedButton;
        [SerializeField] private TextMeshProUGUI textRewarded;

        private MoneyPlayer _moneyPlayer;
        private int _moneyToMultiply;
        
        public void Init()
        {
            loadCar.GetSelectedCar().IsDead += ActivatePanel;
            if (loadCar.GetSelectedCar().TryGetComponent(out MoneyPlayer moneyPlayer))
                _moneyPlayer = moneyPlayer;

            takeMoneyButton.onClick.AddListener(ClickLoadScene);
            rewardedButton.onClick.AddListener(MultiplyMoneyReward);
        }

        private void OnDisable()
        {
            loadCar.GetSelectedCar().IsDead -= ActivatePanel;
        }

        private void ActivatePanel()
        {
            //StartCoroutine(AnimPanel());
            gamePanel.SetActive(false);
            panel.SetActive(true);
            textMoney.text = walletGame.Money.ToString();
            textMeters.text = meters.Distance.ToString();
            textFlipLeft.text = loadCar.GetSelectedCar().DriveCar.AmountFlipLeft.ToString();
            textFlipRight.text = loadCar.GetSelectedCar().DriveCar.AmountFlipRight.ToString();
            
            textTakeMoney.text = walletGame.Money.ToString();
            textRewarded.text = (walletGame.Money * 2).ToString();
            
            rewardedButton.gameObject.SetActive(FlagController.DoShowRewardedAds);

            _moneyToMultiply = walletGame.Money;
            
            meters.Save();
            walletGame.Save();

            if (_moneyPlayer != null)
                textTimeAir.text = _moneyPlayer.MaxTimeAir.ToString("0.0");
            else
                textTimeAir.text = "----";
        }

        public void ClickLoadScene()
        {
            gameManager.FinishedGame(loadCar.GetSelectedCar().DeadType);
        }
        
        public void MultiplyMoneyReward()
        {
            Ads.Instance.ShowReward(() =>
            {
                walletGame.AddMoney(_moneyToMultiply);
                var money = _moneyToMultiply * 2;
                var currentMoney = walletGame.Money;
                //textMoney.text = (walletGame.Money * 2).ToString();
                DOTween.To(() => currentMoney, x => currentMoney = x, money, 1f).OnUpdate(() =>
                {
                    textMoney.text = currentMoney.ToString("0");
                });
                textMoney.transform.DOScale(1.25f, 0.5f);
                
                walletGame.Save();
                Saves.Save();
                StartCoroutine(DelayedLoadScene());
            });
        }
        
        public IEnumerator DelayedLoadScene()
        {
            yield return new WaitForSeconds(1.5f);
            gameManager.FinishedGame(loadCar.GetSelectedCar().DeadType);
        }
        
        /*
        private IEnumerator AnimPanel()
        {
            yield return new WaitForSeconds(timePanelActivate);
            panel.SetActive(true);
        }
        */
    }
}
