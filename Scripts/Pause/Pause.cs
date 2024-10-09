using Sdk.AdController;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Pause
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] private GameObject panelPause;
        [SerializeField] private GameObject panelGame;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TurningOffSound turningOffSound;

        public bool IsPause
        {
            get { return _isPause; }
            set
            {
                _isPause = value;

                panelGame.SetActive(!_isPause);
                panelPause.SetActive(_isPause);

                Time.timeScale = _isPause ? 0 : 1;
                AudioListener.volume = !_isPause ? !turningOffSound.IsSoundTurnedOff ? 1 : 0 : 0;
                turningOffSound.SetSwitch(!_isPause);
            }
        }

        private bool _isPause;

        public void ClickSetPause(bool isPause)
        {
            IsPause = isPause;
        }
        
        public void ClickReset()
        {
            Time.timeScale = 1;
            gameManager.ResetGame();
        }

        public void ClickMenu()
        {
            Time.timeScale = 1;
            gameManager.FinishedGame("Exit");
        }
    }
}
