using InstantGamesBridge;
using InstantGamesBridge.Modules.Platform;
using Sdk.AdController;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private int numberScene;

        private void Start()
        {
            Ads.Instance.ShowInterstitial("menu");
#if UNITY_WEBGL
            Bridge.platform.SendMessage(PlatformMessage.GameReady);
#endif
        }

        public void ClickStartGame()
        {
            SceneManager.LoadScene(numberScene);
        }
    }
}
