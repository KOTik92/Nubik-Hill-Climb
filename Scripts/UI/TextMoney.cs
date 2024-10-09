using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game
{
    public class TextMoney : MonoBehaviour
    {
        [SerializeField] private Vector3 startScale;
        [SerializeField] private Vector3 scale;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        public void ActivateText(string text)
        {
            textMeshProUGUI.text = text;
            transform.DOKill();
            gameObject.SetActive(true);
            transform.localScale = startScale;

            transform.DOScale(scale, 0.5f).OnComplete(() =>
            {
                transform.DOScale(Vector3.zero, 0.3f).SetDelay(1).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            });
        }
    }
}
