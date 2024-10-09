#if UNITY_ANDROID || UNITY_IOS
using System;
using System.Collections.Generic;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using Io.AppMetrica;
using Sdk.AdController.Strategies;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class MobileAdControllerStrategy : IAdControllerStrategy,
        IAppodealInitializationListener,
        IInterstitialAdListener,
        IRewardedVideoAdListener
    {
        private float _adCooldown = 60;
        private float _lastTimeMobileAdShown;
#if UNITY_ANDROID
        private string _appKey = "4310a996b217931fd5a9a3a07b24d8b86de3bddfe8cdc144";
#elif UNITY_IOS
        private string _appKey = ;
#endif
        public float CooldownRemaining => _lastTimeMobileAdShown + _adCooldown - Time.time;
        
        private Action OnRewarded;

        public void Init(float adCooldown, Action onInitializationFinished)
        {
            _adCooldown = adCooldown;
            _lastTimeMobileAdShown = Time.time;
            
            int adTypes = AppodealAdType.Interstitial | AppodealAdType.RewardedVideo;
            
            Appodeal.Initialize(_appKey, adTypes, this);
            Appodeal.SetInterstitialCallbacks(this);
            Appodeal.SetRewardedVideoCallbacks(this);
            
            ResumeTime();
            onInitializationFinished?.Invoke();
        }

        private void PauseTime()
        {
            AudioListener.pause = true;
            Time.timeScale = 0;
            Debug.Log("PauseTime");
        }

        private void ResumeTime()
        {
            Debug.Log("ResumeTime");
            AudioListener.pause = false;
            Time.timeScale = 1;
        }
        
        public bool ShowRewardedAd(Action OnRewarded = null, string placement = "")
        {
            Debug.Log($"ShowRewardedAd called");
            this.OnRewarded = OnRewarded;
            if (Appodeal.IsLoaded(AppodealAdType.RewardedVideo))
            {
                Debug.Log($"ShowRewardedAd loaded");

                PauseTime();
                Appodeal.Show(AppodealAdType.RewardedVideo);
                return true;
            }

            Debug.Log($"ShowRewardedAd NOT loaded");

            return false;
        }

        public bool ShowInterstitialAd(string placement = "")
        {
            var nextShowTime = _lastTimeMobileAdShown + _adCooldown;
            if (Appodeal.IsLoaded(AppodealAdType.Interstitial) && nextShowTime < Time.time)
            {
                _lastTimeMobileAdShown = Time.time;
                PauseTime();
                Appodeal.Show(AppodealAdType.Interstitial);
                return true;
            }

            return false;
        }
        
        #region Interstitial callback handlers

        // Called when interstitial was loaded (precache flag shows if the loaded ad is precache)
        public void OnInterstitialLoaded(bool isPrecache)
        {
            Debug.Log("Interstitial loaded");
        }

        // Called when interstitial failed to load
        public void OnInterstitialFailedToLoad()
        {
            Debug.Log("Interstitial failed to load");
        }

        // Called when interstitial was loaded, but cannot be shown (internal network errors, placement settings, etc.)
        public void OnInterstitialShowFailed()
        {
            Debug.Log("Interstitial show failed");
            ResumeTime();
        }

        // Called when interstitial is shown
        public void OnInterstitialShown()
        {
            Debug.Log("Interstitial shown");

            AppMetrica.ReportEvent("ads:inter_shown");

            ResumeTime();
        }

        // Called when interstitial is closed
        public void OnInterstitialClosed()
        {
            Debug.Log("Interstitial closed");
            ResumeTime();
        }

        // Called when interstitial is clicked
        public void OnInterstitialClicked()
        {
            Debug.Log("Interstitial clicked");
            ResumeTime();
        }

        // Called when interstitial is expired and can not be shown
        public void OnInterstitialExpired()
        {
            Debug.Log("Interstitial expired");
        }

        #endregion
        #region Rewarded Video callback handlers

        //Called when rewarded video was loaded (precache flag shows if the loaded ad is precache).
        public void OnRewardedVideoLoaded(bool isPrecache)
        {
            Debug.Log("RewardedVideo loaded");
        }

        // Called when rewarded video failed to load
        public void OnRewardedVideoFailedToLoad()
        {
            Debug.Log("RewardedVideo failed to load");
        }

        // Called when rewarded video was loaded, but cannot be shown (internal network errors, placement settings, etc.)
        public void OnRewardedVideoShowFailed()
        {
            Debug.Log("RewardedVideo show failed");
        }

        // Called when rewarded video is shown
        public void OnRewardedVideoShown()
        {
            ResumeTime();
            _lastTimeMobileAdShown = Time.time;
            Debug.Log("RewardedVideo shown");
        }

        // Called when reward video is clicked
        public void OnRewardedVideoClicked()
        {
            ResumeTime();
            Debug.Log("RewardedVideo clicked");
        }

        // Called when rewarded video is closed
        public void OnRewardedVideoClosed(bool finished)
        {
            ResumeTime();
            Debug.Log("RewardedVideo closed");
        }

        // Called when rewarded video is viewed until the end
        public void OnRewardedVideoFinished(double amount, string name)
        {
            Debug.Log("RewardedVideo finished");
            AppMetrica.ReportEvent("ads:rev_rewarded");
            ResumeTime();
            OnRewarded?.Invoke();
        }

        //Called when rewarded video is expired and can not be shown
        public void OnRewardedVideoExpired()
        {
            Debug.Log("RewardedVideo expired");
        }

        #endregion

        public void OnInitializationFinished(List<string> errors) { }
    }
}
#endif