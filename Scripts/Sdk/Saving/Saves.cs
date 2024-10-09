using System;
using Cysharp.Threading.Tasks;
using Sdk.Saving.Strategies;
using UnityEngine;

namespace Sdk.Saving
{
    /// <summary>
    /// Class used for cross-platform saving
    /// <b>Save() should be called manually</b>
    /// </summary>
    public static class Saves
    {
        private static SaveStrategy _saveStrategy;
        private static SaveStrategy _secondarySaveStrategy;
        private static bool _isDirty;
        private static float _saveInterval = 1f;
        
        public static void Load(Action onLoaded = null)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _saveStrategy = new BridgeSaveStrategy();
            _secondarySaveStrategy = new PlayerPrefsSaveStrategy();
#else 
            _saveStrategy = new PlayerPrefsSaveStrategy();
            _secondarySaveStrategy = new PlayerPrefsSaveStrategy();
#endif
            var isLoaded = false;
            _saveStrategy.OnLoaded += onLoaded;
            _saveStrategy.OnLoaded += () => isLoaded = true;
            _saveStrategy.Load();
            
            // UniTask to try to save data every 0.5 seconds, but only if data is dirty
            UniTask.Create(async () =>
            {
                await UniTask.WaitUntil(() => isLoaded);
                while (true)
                {
                    if (_isDirty)
                    {
                        _saveStrategy.SetInt("timestamp", (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                        _saveStrategy.Save();
                        _isDirty = false;
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(_saveInterval));
                }
            });
        }

        public static void Save()
        {
            _secondarySaveStrategy.SetInt("timestamp", (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _secondarySaveStrategy.Save();
        }

        public static void SetString(string key, string value)
        {
            _isDirty = true;
            _saveStrategy.SetString(key, value);
            _secondarySaveStrategy.SetString(key, value);
        }

        public static void SetInt(string key, int value)
        {
            _isDirty = true;
            _saveStrategy.SetInt(key, value);
            _secondarySaveStrategy.SetInt(key, value);
        }

        public static void SetBool(string key, bool value)
        {
            _isDirty = true;
            _saveStrategy.SetBool(key, value);
            _secondarySaveStrategy.SetBool(key, value);
        }

        public static void SetArray<T>(string key, T[] list)
        {
            _isDirty = true;
            _saveStrategy.SetArray(key, list);
            _secondarySaveStrategy.SetArray(key, list);
        }

        public static string GetString(string key, string defaultValue = "")
        {
            if (_secondarySaveStrategy.GetInt("timestamp") > _saveStrategy.GetInt("timestamp"))
                return _secondarySaveStrategy.GetString(key, defaultValue);
            return _saveStrategy.GetString(key, defaultValue);
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            if (_secondarySaveStrategy.GetInt("timestamp") > _saveStrategy.GetInt("timestamp"))
                return _secondarySaveStrategy.GetInt(key, defaultValue);
            return _saveStrategy.GetInt(key, defaultValue);
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            if (_secondarySaveStrategy.GetInt("timestamp") > _saveStrategy.GetInt("timestamp"))
                return _secondarySaveStrategy.GetBool(key, defaultValue);
            return _saveStrategy.GetBool(key, defaultValue);
        }

        public static T[] GetArray<T>(string key, T[] defaultValue = null)
        {
            if (_secondarySaveStrategy.GetInt("timestamp") > _saveStrategy.GetInt("timestamp"))
                return _secondarySaveStrategy.GetArray(key, defaultValue);
            return _saveStrategy.GetArray(key, defaultValue);
        }
    }
}