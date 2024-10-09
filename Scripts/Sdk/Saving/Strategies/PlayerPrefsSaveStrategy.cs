using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sdk.Saving.Strategies
{
    public class PlayerPrefsSaveStrategy : SaveStrategy
    {
        public override event Action OnLoaded;
        
        private bool _isDirty;

        public override Dictionary<string, string> GetData()
        {
            return null;
        }

        public override void Load()
        {
            OnLoaded?.Invoke();
        }
        
        public override void Save()
        {
            PlayerPrefs.Save();
        }
        
        public override void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public override void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public override void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public override void SetArray<T>(string key, T[] list)
        {
            string json = JsonUtility.ToJson(list);
            PlayerPrefs.SetString(key, json);
        }

        public override string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public override int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public override bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public override T[] GetArray<T>(string key, T[] defaultValue = null)
        {
            string json = PlayerPrefs.GetString(key, "");
            if (string.IsNullOrEmpty(json))
            {
                return defaultValue;
            }

            return JsonUtility.FromJson<T[]>(json);
        }
    }
}