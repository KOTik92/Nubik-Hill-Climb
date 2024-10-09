using System.Collections;
using Sdk.Saving;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#elif UNITY_ANDROID
using Google.Play.Review;
#elif UNITY_WEBGL
using InstantGamesBridge;
using InstantGamesBridge.Modules.Social;
#endif

namespace Sdk.Rating
{
    /// <summary>
    /// Requires Google's Play Review plugin for Android
    /// </summary>
    public class AppRating : MonoBehaviour
    {
#if UNITY_ANDROID
        private ReviewManager _reviewManager;
        private PlayReviewInfo _playReviewInfo;
        private Coroutine _coroutine;
#endif

        private void Start()
        {
#if UNITY_ANDROID
            _coroutine = StartCoroutine(InitReview());
#endif
        }

        public void RateAndReview()
        {
            if (SavesFacade.IsAlreadyAskedToRate) return;
#if UNITY_IOS
            Device.RequestStoreReview();
#elif UNITY_ANDROID
            StartCoroutine(LaunchReview());
#elif UNITY_WEBGL
            Bridge.social.Rate();
#endif
            SavesFacade.IsAlreadyAskedToRate = true;
        }

#if UNITY_ANDROID
        private IEnumerator InitReview(bool force = false)
        {
            if (_reviewManager == null) _reviewManager = new ReviewManager();

            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                if (force) DirectlyOpen();
                yield break;
            }

            _playReviewInfo = requestFlowOperation.GetResult();
        }

        public IEnumerator LaunchReview()
        {
            if (_playReviewInfo == null)
            {
                if (_coroutine != null) StopCoroutine(_coroutine);
                yield return StartCoroutine(InitReview(true));
            }

            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;
            _playReviewInfo = null;
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                DirectlyOpen();
                yield break;
            }
        }
#endif

        private void DirectlyOpen() { Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}"); }
    }
}