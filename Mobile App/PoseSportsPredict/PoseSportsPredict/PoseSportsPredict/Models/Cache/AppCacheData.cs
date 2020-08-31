using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Utilities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.Models.Cache
{
    public class AppCacheData : ISQLiteStorable
    {
        #region ISQLiteStorable

        [PrimaryKey]
        public string PrimaryKey { get => _cachingKey; set => _cachingKey = value; }

        public int Order { get; set; } = 0;
        public DateTime StoredTime { get => _storedTime; set => _storedTime = value; }

        #endregion ISQLiteStorable

        #region Fields

        private string _cachingKey;
        private DateTime _storedTime;
        private string _cachedData;
        private SerializeType _serializeType;
        private DateTime _expireTime;

        #endregion Fields

        #region Properties

        public string CachedData { get => _cachedData; set => _cachedData = value; }
        public SerializeType SerializeType { get => _serializeType; set => _serializeType = value; }
        public DateTime ExpireTime { get => _expireTime; set => _expireTime = value; }

        #endregion Properties

        public void BindCacheData(object cachingData, string cachingKey, TimeSpan expireTimeSpan, SerializeType serializeType)
        {
            PrimaryKey = cachingKey;
            StoredTime = DateTime.UtcNow;
            SerializeType = serializeType;
#if DEBUG
            ExpireTime = StoredTime.Add(TimeSpan.Zero);
#else
            ExpireTime = StoredTime.Add(expireTimeSpan);
#endif

            if (SerializeType == SerializeType.Json)
                CachedData = cachingData.JsonSerialize();
            else if (SerializeType == SerializeType.MessagePack)
            {
                if (cachingData is byte[] bytes)
                {
                    CachedData = Convert.ToBase64String(bytes);
                }
                else
                    Debug.Assert(false, "cachingData param is must be byte array");
            }
        }
    }
}