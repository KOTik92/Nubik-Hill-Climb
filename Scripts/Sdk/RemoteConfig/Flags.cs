using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Sdk.RemoteConfig
{
    public static class Flags
    {
        private static Dictionary<string, string> flags = new Dictionary<string, string>();
        private static Dictionary<string, int> cachedIntValues = new Dictionary<string, int>();
        private static Dictionary<string, float> cachedFloatValues = new Dictionary<string, float>();
        private static bool isInitialized;
        
        public static void Init(Dictionary<string, string> dictionary)
        {
            if (dictionary.Count == 0) return; 
            if (isInitialized) return;
            
            flags = dictionary;
            isInitialized = true;
        }
        
        public static string GetString(string key, string defaultValue = "")
        {
            if (!isInitialized) return defaultValue;
            
            return flags.ContainsKey(key) ? flags[key] : defaultValue;
        }
        
        public static int GetInt(string key, int defaultValue = 0)
        {
            if (!isInitialized) return defaultValue;
            // if we already have cached value, return it, to avoid parsing string to int multiple times
            if (cachedIntValues.ContainsKey(key))
            {
                return cachedIntValues[key];
            }
            
            // parse string to int and cache it
            if (flags.ContainsKey(key))
            {
                if (int.TryParse(flags[key], out var result))
                {
                    cachedIntValues[key] = result;
                    return result;
                }
                Debug.LogError("Flag value is not an integer, key: " + key);
                return defaultValue;
            }
            Debug.LogError("Flag not found, key: " + key);
            return defaultValue;
        }
        
        public static float GetFloat(string key, float defaultValue = 0f)
        {
            if (!isInitialized) return defaultValue;
            if (cachedFloatValues.ContainsKey(key))
            {
                return cachedFloatValues[key];
            }
            
            if (flags.ContainsKey(key))
            {
                if (float.TryParse(flags[key], NumberStyles.Float,CultureInfo.InvariantCulture, out var result ))
                {
                    cachedFloatValues[key] = result;
                    return result;
                }
                Debug.LogError("Flag value is not a float, key: " + key);
                return defaultValue;
            }
            Debug.LogError("Flag not found, key: " + key);
            return defaultValue;
        }
    }
}