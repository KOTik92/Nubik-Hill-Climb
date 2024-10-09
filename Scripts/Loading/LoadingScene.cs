using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using InstantGamesBridge;
using InstantGamesBridge.Common;
using InstantGamesBridge.Modules.RemoteConfig;
using Io.AppMetrica;
using Sdk.AdController;
using Sdk.Analytics;
using Sdk.RemoteConfig;
using Sdk.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Slider progressbar;
    [SerializeField] private string appMetricaApiKey;
    
    private float _targetProgress;

    private void Update()
    {
        progressbar.value = Mathf.Lerp(progressbar.value, _targetProgress, Time.deltaTime * 3f);
    }

    private IEnumerator Start()
    {
        var progress = 0f;
        var totalProgress = 3.1f;

        var asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            _targetProgress = (asyncOperation.progress + progress) / totalProgress;
            yield return null;
        }

        yield return LoadSaves();
        progress += 0.5f;
        _targetProgress = progress / totalProgress;
        
        yield return LoadAds();
        progress += 0.5f;
        _targetProgress = progress / totalProgress;
        
        yield return LoadAnalytics();
        progress += 0.5f;
        _targetProgress = progress / totalProgress;
        
        yield return LoadRemoteConfig();
        progress += 0.5f;
        _targetProgress = progress / totalProgress;
        
        asyncOperation.allowSceneActivation = true;
    }

    private IEnumerator LoadAds()
    {
        var isInitialized = false;
        var maxWaitTime = 3f;
        
        Ads.Instance.OnInitialized += () => isInitialized = true;
        Ads.Instance.Init();
        
        while (!isInitialized && maxWaitTime > 0)
        {
            maxWaitTime -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator LoadSaves()
    {
        var isInitialized = false;
        var maxWaitTime = 3f;
        
        Saves.Load(() => isInitialized = true);

        while (!isInitialized && maxWaitTime > 0)
        {
            maxWaitTime -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator LoadAnalytics()
    {
#if UNITY_WEBGL
        GameAnalytics.Initialize();
        
        yield return null;
#elif UNITY_ANDROID || UNITY_IOS
        var config = new AppMetricaConfig(appMetricaApiKey)
        {
            CrashReporting = true, // prefab field 'Exceptions Reporting'
            SessionTimeout = 10, // prefab field 'Session Timeout Sec'
            LocationTracking = false, // prefab field 'Location Tracking'
            Logs = false, // prefab field 'Logs'
            DataSendingEnabled = true, // prefab field 'Statistics Sending'
        };
        AppMetricaActivator.Init(config);
        yield return null;
#endif
    }

  	private bool _areConfigsPopulated;

    private IEnumerator LoadRemoteConfig()
    {
        if (Bridge.remoteConfig.isSupported)
        {
            Bridge.remoteConfig.Get(OnGetCompleted);
            yield return new WaitUntil(() => _areConfigsPopulated);
            Debug.Log("Remote configs has been populated");
            yield break;
        }
        Debug.Log("Remote configs are not supported. Will use default values for flags");
    } 
    
    private void OnGetCompleted(bool success, List<RemoteConfigValue> values)
    {
        if (success)
        {
            var dict = new Dictionary<string, string>();
            foreach (var value in values)
            {
                dict.Add(value.name, value.value);
            }
            Flags.Init(dict);
        }
        Debug.Log("Remote configs loading failed");
        _areConfigsPopulated = true;
    } 
}
