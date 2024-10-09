using System.Collections;
using DG.Tweening;
using Game.Car;
using InstantGamesBridge;
using Lean.Localization;
using Sdk.Saving;
using TMPro;
using UnityEngine;
using DeviceType = InstantGamesBridge.Modules.Device.DeviceType;

namespace Tutorial
{
    public class TutorialGame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private LoadCar loadCar;

        private void Start()
        {
            if (SavesFacade.TutorialGame)
                return;

            ActivateText();
            loadCar.GetSelectedCar().InputCar.OnInput += DeactivateText;
        }

        private void ActivateText()
        {
            if (Bridge.device.type == DeviceType.Mobile || Bridge.device.type == DeviceType.Tablet)
                text.text = LeanLocalization.GetTranslationText("TutorialGame_Mobile");
            else
                text.text = LeanLocalization.GetTranslationText("TutorialGame");

            text.gameObject.SetActive(true);
            text.transform.localScale = Vector3.zero;
            text.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {
                text.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });
        }

        private void DeactivateText(int axis, bool isDouble)
        {
            if (axis != 0)
            {
                text.transform.DOKill();
                text.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => { text.gameObject.SetActive(false); });
                SavesFacade.TutorialGame = true;
                loadCar.GetSelectedCar().InputCar.OnInput -= DeactivateText;
            }
        }
    }
}
