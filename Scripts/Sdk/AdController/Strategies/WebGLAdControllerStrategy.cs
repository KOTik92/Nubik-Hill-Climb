#if UNITY_WEBGL
using System;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Advertisement;
using UnityEngine;

namespace Sdk.AdController.Strategies
{
    public class WebGLAdControllerStrategy : IAdControllerStrategy
    {
        private bool _allowToIssueReward;
        private float _lastTimeAdShown;
        private float _adCooldown;
        
        public float CooldownRemaining => _lastTimeAdShown + _adCooldown - Time.time;
        
        private Action OnRewarded;

        public void Init(float adCooldown, Action onInitializationFinished)
        {
            _adCooldown = adCooldown;
            
            Bridge.advertisement.SetMinimumDelayBetweenInterstitial((int)adCooldown);
            Bridge.advertisement.rewardedStateChanged += RewardedStateChanged;
            Bridge.advertisement.interstitialStateChanged += InterstitialStateChanged;
            ResumeTime();
            
            onInitializationFinished?.Invoke();
        }

        public bool ShowRewardedAd(Action OnRewarded = null, string placement = "")
        {
            this.OnRewarded = OnRewarded;
            PauseTime();
            Bridge.advertisement.ShowRewarded();
            return true;
        }

        public bool ShowInterstitialAd(string placement = "")
        {
            PauseTime();
            Bridge.advertisement.ShowInterstitial();
            return true;
        }
        
        private void PauseTime()
        {
            AudioListener.pause = true;
            Time.timeScale = 0;
        }

        private void ResumeTime()
        {
            AudioListener.pause = false;
            Time.timeScale = 1;
        }
        
        private void RewardedStateChanged(RewardedState state)
        {
            switch (state)
            {
                case RewardedState.Rewarded:
                    Debug.Log("Rewarded");
                    _allowToIssueReward = true;
                    break;
                case RewardedState.Closed:
                    if (_allowToIssueReward)
                    {
                        ResumeTime();
                        OnRewarded?.Invoke();
                    }
                    _allowToIssueReward = false;
                    break;
                case RewardedState.Failed:
                    ResumeTime();
                    _allowToIssueReward = false;
                    break;
            }
        }

        private void InterstitialStateChanged(InterstitialState state)
        {
            switch (state)
            {
                case InterstitialState.Closed:
                    ResumeTime();
                    break;
                case InterstitialState.Failed:
                    ResumeTime();
                    break;
            }
        }
    }
}
#endif
