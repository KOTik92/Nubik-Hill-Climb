using UnityEngine;
using UnityEngine.UI;

public class ClickOnLink : MonoBehaviour
{
    [SerializeField] private string link;
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Click);
    }

    public void Click()
    {
        Application.OpenURL(link);
    }
}
