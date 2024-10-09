using System;
using GameAnalyticsSDK;
using Sdk.AdController.Strategies;
using UnityEngine;

namespace Sdk.AdController
{
    public class Ads : MonoBehaviour
    {
        public static Ads Instance;
        
        [SerializeField] private float adCooldown = 60;
        
        public float CooldownRemaining => _adControllerStrategy.CooldownRemaining;

        public event Action OnInitialized;
        
        private IAdControllerStrategy _adControllerStrategy;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            Destroy(gameObject);
        }
        
        public void Init()
        {
#if UNITY_WEBGL
            _adControllerStrategy = new WebGLAdControllerStrategy();
#elif UNITY_ANDROID || UNITY_IOS
            _adControllerStrategy = new MobileAdControllerStrategy();
#endif
            _adControllerStrategy.Init(adCooldown, OnInitializationFinished);
        }
        
        public bool ShowInterstitial(string placement = "")
        {
            return _adControllerStrategy.ShowInterstitialAd(placement);
        }

        public bool ShowReward(Action reward = null, string placement = "")
        {
            return _adControllerStrategy.ShowRewardedAd(reward, placement);
        }

        private void OnInitializationFinished()
        {
            OnInitialized?.Invoke();
        }
    }
}