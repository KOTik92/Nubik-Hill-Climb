using System.Collections.Generic;
using GameAnalyticsSDK;
using Io.AppMetrica;
using Newtonsoft.Json;
using UnityEngine;

public static class Analytics
{
    public static void LevelStarted(string levelName, string carName, int record, int tries, int totalTries)
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("levelType", levelName);
        dictionary.Add("carType", carName);
        dictionary.Add("record", record);
        dictionary.Add("tries", tries);
        dictionary.Add("totalTries", totalTries);
        
#if UNITY_ANDROID || UNITY_IOS
        SendEvent("level_start", dictionary);
#elif UNITY_WEBGL
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelName, carName);
#endif
    }
    
    public static void LevelFinished(string levelName, string reason, int meters, int record, int coinsEarned, int tries, int totalTries, string carName, int engineLevel, int tiresLevel, int rotationLevel)
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("levelType", levelName);
        dictionary.Add("reason", reason);
        dictionary.Add("meters", meters);
        dictionary.Add("record", record);
        dictionary.Add("coinsEarned", coinsEarned);
        dictionary.Add("tries", tries);
        dictionary.Add("totalTries", totalTries);
        dictionary.Add("carType", carName);
        dictionary.Add("engineLevel", engineLevel);
        dictionary.Add("tiresLevel", tiresLevel);
        dictionary.Add("rotationLevel", rotationLevel);

#if UNITY_ANDROID || UNITY_IOS
        SendEvent("level_finish", dictionary);
#elif UNITY_WEBGL
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelName, carName, reason, meters);
#endif
    }
    
    public static void OnBuy(string type, string name, int cost, int totalTries)
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("type", type);
        dictionary.Add("name", name);
        dictionary.Add("cost", cost);
        dictionary.Add("totalTries", totalTries);
        
#if UNITY_ANDROID || UNITY_IOS
        SendEvent("buy", dictionary);
#elif UNITY_WEBGL
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Coins", cost, type, name);
#endif
    }
    
    private static void SendEvent(string eventName, Dictionary<string, object> dictionary)
    {
        var json = JsonConvert.SerializeObject(dictionary);
        Debug.Log(json);
#if UNITY_ANDROID || UNITY_IOS
        AppMetrica.ReportEvent(eventName, json);
        AppMetrica.SendEventsBuffer();
#elif UNITY_WEBGL
#endif
    }
}
