using System;
using System.Collections.Generic;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Storage;
using Newtonsoft.Json;
using Sdk.Saving;
using UnityEngine;

namespace Sdk.Saving.Modules
{
    public static class BridgeSaves
    {
#if UNITY_WEBGL
        public static Action onLoaded;

        private static class Saves
        {
            public static bool IsDirty;
            public static Dictionary<string, string> Data;
        }

        private static bool IsCloudAvailable =>
            Bridge.storage.IsSupported(StorageType.PlatformInternal) &&
            Bridge.storage.IsAvailable(StorageType.PlatformInternal);
        
        public static Dictionary<string, string> GetData()
        {
            return Saves.Data;
        }

        public static void Load()
        {
            var storageType = IsCloudAvailable ? StorageType.PlatformInternal : StorageType.LocalStorage;
            Bridge.storage.Get("saves", OnStorageGetCompleted, storageType);
        }

        public static void DeleteAll(StorageType storageType)
        {
            Bridge.storage.Delete("saves", null, storageType);
        }

        private static void OnStorageGetCompleted(bool success, string data)
        {
            if (success)
            {
                if (data == null)
                {
                    Saves.Data = new();
                    onLoaded.Invoke();
                    return;
                }

                var keyValuePairs =
                    JsonConvert.DeserializeObject(data, typeof(Dictionary<string, string>)) as
                        Dictionary<string, string>;
                Saves.Data = keyValuePairs;
                Saves.Data ??= new();
            }
            else
            {
                Debug.LogError("Error loading data");
            }

            onLoaded.Invoke();
        }

        public static void SetString(string key, string value)
        {
            if (Saves.Data.ContainsKey(key))
                Saves.Data[key] = value.ToString();
            else
                Saves.Data.Add(key, value.ToString());
            Saves.IsDirty = true;
        }

        public static void SetInt(string key, int value)
        {
            if (Saves.Data.ContainsKey(key))
                Saves.Data[key] = value.ToString();
            else
                Saves.Data.Add(key, value.ToString());

            Saves.IsDirty = true;
        }

        public static void SetBool(string key, bool value)
        {
            if (Saves.Data.ContainsKey(key))
                Saves.Data[key] = value.ToString();
            else
                Saves.Data.Add(key, value.ToString());
            Saves.IsDirty = true;
        }

        public static string GetString(string key, string defaultValue = default)
        {
            if (Saves.Data.ContainsKey(key)) return Saves.Data[key];
            return defaultValue;
        }

        public static int GetInt(string key, int defaultValue = default)
        {
            if (Saves.Data.ContainsKey(key)) return Convert.ToInt32(Saves.Data[key]);
            return defaultValue;
        }

        public static bool GetBool(string key, bool defaultValue = default)
        {
            if (Saves.Data.ContainsKey(key)) return Convert.ToBoolean(Saves.Data[key]);
            return defaultValue;
        }

        public static void SaveProgress()
        {
            StorageType storageType = IsCloudAvailable ? StorageType.PlatformInternal : StorageType.LocalStorage;
            var json = JsonConvert.SerializeObject(Saves.Data);
            Bridge.storage.Set("saves", json, null, storageType);
            string savesStringDebug = json.Replace(",", "\n")
                .Replace("\"", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace(":", ": ");
            Debug.Log($"SaveProgress {storageType.ToString()}\n{savesStringDebug}");
        }
#endif
    }
}