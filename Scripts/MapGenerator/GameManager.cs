using Game;
using Game.Car;
using Sdk.AdController;
using Sdk.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wallet;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int numberSceneMenu;
        [SerializeField] private Camera camera;
        [Space]
        [SerializeField] private LoadCar loadCar;
        [SerializeField] private LoadMap loadMap;
        [SerializeField] private PoolMoney poolMoney;
        [SerializeField] private PoolFuel poolFuel;
        [SerializeField] private Dead dead;
        [SerializeField] private Meters meters;
        [SerializeField] private WalletGame walletGame;
        [SerializeField] private ParallaxGenerator parallaxGenerator;
        [SerializeField] private UIDriveCar uiDriveCar;
        
        private void Awake()
        {
            loadCar.Init();
            uiDriveCar.Init(loadCar.GetSelectedCar());
            poolMoney.Init();
            poolFuel.Init();
            loadMap.Load();
            loadMap.SelectedMap.Init(camera.transform, loadMap.LoadCarAI.SelectedCarAI);
            loadMap.SelectedParallax.Init(camera.transform);
            dead.Init();
            meters.Init(loadCar.GetSelectedCar(), $"Meters_{loadMap.SelectedMap.MapData.typeMap}");
            parallaxGenerator.Init(camera.transform);
            
            AnalyticsEventStart();
        }

        private void AnalyticsEventStart()
        {
            TypeMap typeMap = loadMap.SelectedMap.MapData.typeMap;
            TypeCar carType = loadCar.GetSelectedCar().CarData.typeCar;
            int record = meters.MaxDistance;
            int tries = loadMap.NumberLoadMap;
            int totalTries = SavesFacade.TotalTries;
            
            Analytics.LevelStarted(typeMap.ToString(), carType.ToString(), record, tries, totalTries);
        }

        public void ResetGame()
        {
            meters.Save();
            walletGame.Save();
            Saves.Save();
            Ads.Instance.ShowInterstitial("restart");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void FinishedGame(string reason)
        {
            meters.Save();
            walletGame.Save();
            Saves.Save();
            AnalyticsEventFinished(reason);
            SceneManager.LoadScene(numberSceneMenu);
        }
        
        private void AnalyticsEventFinished(string reason)
        {
            TypeMap typeMap = loadMap.SelectedMap.MapData.typeMap;
            int amountMeters = meters.Distance;
            int record = meters.MaxDistance;
            int coinsEarned = walletGame.Money;
            int tries = loadMap.NumberLoadMap;
            int totalTries = SavesFacade.TotalTries;
            TypeCar carType = loadCar.GetSelectedCar().CarData.typeCar;
            int engineLevel = loadCar.SpeedUpgradeLevel;
            int rotationLevel = loadCar.RotationSpeedUpgradeLevel;
            int tiresLevel = loadCar.GripUpgradeLevel;
            
            Analytics.LevelFinished(typeMap.ToString(), reason, amountMeters, record, coinsEarned, tries, totalTries, carType.ToString(), engineLevel, tiresLevel, rotationLevel);
        }
    }
}