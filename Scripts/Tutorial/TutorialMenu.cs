using DG.Tweening;
using Menu.SwitchPanel;
using Menu.Upgrade;
using Sdk.Saving;
using UnityEngine;
using UnityEngine.UI;


namespace Tutorial
{
    public class TutorialMenu : MonoBehaviour
    {
        [SerializeField] private Image[] arrows;
        [SerializeField] private SwitchPanels switchPanels;
        [SerializeField] private Upgrade upgrade;
        
        private Wallet.Wallet _wallet = new Wallet.Wallet();

        private int _numTutor;
        
        private void Start()
        {
            if (SavesFacade.TutorialMenu || SavesFacade.TotalTries == 0 || _wallet.MoneyValue < 2000)
                return;

            switchPanels.OnSwitchPanel += SwitchTutor;
            upgrade.OnBuy += DeactivateTutor;
            AnimArrow();
        }

        private void SwitchTutor(int numPanel)
        {
            if (numPanel == 1)
            {
                arrows[_numTutor].gameObject.SetActive(false);
                _numTutor++;
                AnimArrow();
            }
            else if(_numTutor > 0)
            {
                arrows[_numTutor].gameObject.SetActive(false);
                _numTutor--;
                AnimArrow();
            }
        }

        private void AnimArrow()
        {
            arrows[_numTutor].transform.DOKill();
            arrows[_numTutor].transform.localScale = Vector3.one;
            arrows[_numTutor].gameObject.SetActive(true);
            arrows[_numTutor].transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f).SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void DeactivateTutor()
        {
            foreach (var arrow in arrows)
                arrow.gameObject.SetActive(false);

            SavesFacade.TutorialMenu = true;
            switchPanels.OnSwitchPanel -= SwitchTutor;
            upgrade.OnBuy -= DeactivateTutor;
        }
    }
}
