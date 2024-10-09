using System;

namespace Sdk.AdController.Strategies
{
    public interface IAdControllerStrategy
    {
        public float CooldownRemaining { get; }
        public void Init(float adCooldown = 60, Action OnInitializationFinished = null);
        public bool ShowRewardedAd(Action OnRewarded = null, string placement = "");
        public bool ShowInterstitialAd(string placement = "");
    }
}