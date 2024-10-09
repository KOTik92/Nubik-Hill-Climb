using UnityEngine;

namespace Menu
{
    public abstract class Panel : MonoBehaviour
    {
        [SerializeField] private Transform panel;

        public virtual void ActivatorPanel(bool isActivate)
        {
            if (isActivate)
            {
                if (panel.TryGetComponent(out CanvasGroup canvasGroup))
                {
                    canvasGroup.alpha = 1;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }
                else
                    panel.gameObject.SetActive(true);
            }
            else
            {
                if (panel.TryGetComponent(out CanvasGroup canvasGroup))
                {
                    canvasGroup.alpha = 0;
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                }
                else
                    panel.gameObject.SetActive(false);
            }
        }
    }
}
