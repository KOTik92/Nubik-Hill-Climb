using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image panel;
    
    private bool _isActivate;

    public void ClickActivator(bool isActivate)
    {
        _isActivate = isActivate;
        
        Anim();
    }

    private void Anim()
    {
        if (_isActivate)
        {
            background.gameObject.SetActive(true);
            background.color = Color.clear;
            panel.transform.localScale = Vector3.zero;

            background.DOColor(new Color(0, 0, 0, 0.5f), 0.3f);
            panel.transform.DOScale(Vector3.one, 0.3f);
        }
        else
        {
            background.DOColor(new Color(0, 0, 0, 0), 0.3f).OnComplete(() => {background.gameObject.SetActive(false);});
            panel.transform.DOScale(Vector3.zero, 0.3f);
        }
    }
}
