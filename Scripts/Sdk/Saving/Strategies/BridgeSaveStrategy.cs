using System;
using System.Collections.Generic;
using Sdk.Saving.Modules;
using UnityEngine;

namespace Sdk.Saving.Strategies
{
#if UNITY_WEBGL
    public class BridgeSaveStrategy : SaveStrategy
    {
        public override event Action OnLoaded;

        public override void Load()
        {
            BridgeSaves.onLoaded += Loaded;
            BridgeSaves.Load();
        }

        public override Dictionary<string, string> GetData()
        {
            return BridgeSaves.GetData();
        }
        
        private void Loaded()
        {
            OnLoaded?.Invoke();
            BridgeSaves.onLoaded -= Loaded;
        }
        
        public override void Save()
        {
            BridgeSaves.SaveProgress();
        }
        
        public override void SetString(string key, string value)
        {
            BridgeSaves.SetString(key, value);
        }

        public override void SetInt(string key, int value)
        {
            BridgeSaves.SetInt(key, value);
        }

        public override void SetBool(string key, bool value)
        {
            BridgeSaves.SetBool(key, value);
        }

        public override void SetArray<T>(string key, T[] list)
        {
            string json = JsonUtility.ToJson(list);
            BridgeSaves.SetString(key, json);
        }

        public override string GetString(string key, string defaultValue = "")
        {
            return BridgeSaves.GetString(key, defaultValue);
        }

        public override int GetInt(string key, int defaultValue = 0)
        {
            return BridgeSaves.GetInt(key, defaultValue);
        }

        public override bool GetBool(string key, bool defaultValue = false)
        {
            return BridgeSaves.GetBool(key, defaultValue);
        }

        public override T[] GetArray<T>(string key, T[] defaultValue = null)
        {
            string json = BridgeSaves.GetString(key, "");
            if (string.IsNullOrEmpty(json))
            {
                return defaultValue;
            }
            return JsonUtility.FromJson<T[]>(json);
        }
    }
#endif
}