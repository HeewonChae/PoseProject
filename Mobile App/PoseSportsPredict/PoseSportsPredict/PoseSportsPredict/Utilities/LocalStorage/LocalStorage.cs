using Newtonsoft.Json;
using Plugin.Settings;
using PoseSportsPredict.InfraStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Utilities.LocalStorage
{
    public sealed class LocalStorage
    {
        #region Singleton Pattern

        private static LocalStorage _storage;

        public static LocalStorage Storage
        {
            get
            {
                if (_storage == null)
                    _storage = new LocalStorage();

                return _storage;
            }
        }

        private LocalStorage()
        {
        }

        #endregion Singleton Pattern

        private readonly object _locker = new object();
        private readonly string _defaultValue = string.Empty;

        private string this[string key]
        {
            get
            {
                lock (_locker)
                {
                    return CrossSettings.Current.GetValueOrDefault(key, _defaultValue);
                }
            }

            set
            {
                lock (_locker)
                {
                    CrossSettings.Current.AddOrUpdateValue(key, value);
                }
            }
        }

        public void GetValueOrDefault<T>(string key, out T foundValue)
        {
            foundValue = default;

            string savedValue = this[key];
            if (savedValue.Equals(_defaultValue))
                return;

            foundValue = JsonConvert.DeserializeObject<T>(savedValue);
        }

        public void AddOrUpdateValue<T>(string key, T saveValue)
        {
            string serializeValue = JsonConvert.SerializeObject(saveValue);

            this[key] = serializeValue;
        }

        public void Remove(string key)
        {
            this[key] = _defaultValue;
        }
    }
}