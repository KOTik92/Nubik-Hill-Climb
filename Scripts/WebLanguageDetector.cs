using System;
using System.Collections;
using System.Linq;
using InstantGamesBridge;
using Lean.Localization;
using UnityEngine;

[RequireComponent(typeof(LeanLocalization))]
public class WebLanguageDetector : MonoBehaviour
{
    public bool _onlyEnglish = false;
    public bool _onlyTurkish = false;
    string detectedLanguage;
    int maxRetries = 10;
    int currentRetries = 0;
    // Start is called before the first frame update
    
    private IEnumerator Start()
    {
        var ll = GetComponent<LeanLocalization>();
        //for webgl -> choose web language if possible, or choose system language
        //for others -> choose system language
        if (_onlyTurkish)
        {
            ll.CurrentLanguage = "Turkish";
            yield break;
        }
        if (_onlyEnglish)
        {
            ll.CurrentLanguage = "English";
            yield break;
        }

#if UNITY_WEBGL
        while (currentRetries < maxRetries)
        {
            try
            {
                if (detectedLanguage == ll.CurrentLanguage)
                    break;
        
                if (currentRetries >= maxRetries)
                    break;
        
                currentRetries++;
        
                string culturalCode = Bridge.platform.language;
                Debug.Log("Web Lang = " + culturalCode);
                ll.DetectLanguage = LeanLocalization.DetectType.None;
                //try to find the existing language
                var leanLanguages = LeanLocalization.CurrentLanguages.Values.ToList();
                bool found = false;
                for (int i = 0; i < leanLanguages.Count; i++)
                {
                    if (leanLanguages[i].Cultures.Find(c => c.Equals(culturalCode)) != null)
                    {
                        ll.CurrentLanguage = (leanLanguages[i].name);
                        detectedLanguage = leanLanguages[i].name;
                        Debug.Log($"Found the {culturalCode} in {leanLanguages[i].name}");
                        found = true;
                        break;
                    }
                }
        
                if (!found)
                {
                    Debug.Log($"Did not found {culturalCode}");
                    ll.DetectLanguage = LeanLocalization.DetectType.SystemLanguage;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.StackTrace);
                Debug.Log($"Error when getting and setting language");
                ll.DetectLanguage = LeanLocalization.DetectType.SystemLanguage;
            }
        
            Debug.Log($"Setted language is {ll.CurrentLanguage}");
            yield return null;
        }
#else
                ll.DetectLanguage = LeanLocalization.DetectType.SystemLanguage;
#endif
    }
}