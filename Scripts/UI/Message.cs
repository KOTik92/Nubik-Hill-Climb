using DG.Tweening;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform canvas;

    private Transform _point;

    private void OnEnable()
    {
        canvasGroup.alpha = 0;
        
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, 1).OnComplete(() =>
        {
            DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, 1).SetDelay(2).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }

    public void SetPoint(Transform point)
    {
        _point = point;
    }

    private void Update()
    {
        if(_point == null)
            return;

        Vector2 viewportPosition = Camera.main.WorldToScreenPoint(_point.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, viewportPosition, null, out Vector2 position);
        transform.GetComponent<RectTransform>().anchoredPosition = position;
    }
}
