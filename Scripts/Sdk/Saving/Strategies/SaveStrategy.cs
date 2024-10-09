using System;
using System.Collections.Generic;

namespace Sdk.Saving.Strategies
{
    public abstract class SaveStrategy
    {
        public abstract event Action OnLoaded;
        public abstract Dictionary<string, string> GetData();
        public abstract void Load();
        public abstract void Save();
        public abstract void SetString(string key, string value);
        public abstract void SetInt(string key, int value);
        public abstract void SetBool(string key, bool value);
        public abstract void SetArray<T>(string key, T[] list);
        public abstract string GetString(string key, string defaultValue = "");
        public abstract int GetInt(string key, int defaultValue = 0);
        public abstract bool GetBool(string key, bool defaultValue = false);
        public abstract T[] GetArray<T>(string key, T[] defaultValue = null);
    }
}