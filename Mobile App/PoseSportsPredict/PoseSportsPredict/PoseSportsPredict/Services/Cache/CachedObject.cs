using MessagePack;
using PoseSportsPredict.InfraStructure.Cache;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Models.Cache;
using PoseSportsPredict.Utilities;
using Shiny;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.Services.Cache
{
    public class CachedObject<T> : ICachedObject<T> where T : class
    {
        #region Fields

        private string _cacheKey;
        private Func<Task<object>> _loader;
        private TimeSpan _expireTime;
        private SerializeType _serializeType;

        #endregion Fields

        public string CacheKey => _cacheKey;
        public Func<Task<object>> Loader => _loader;
        public TimeSpan ExpireTime => _expireTime;
        public SerializeType SerializeType => _serializeType;
        public Task<T> Cached => GetObject();

        public CachedObject(Func<Task<object>> loader, string key, TimeSpan expireTime, SerializeType serializeType)
        {
            _loader = loader;
            _cacheKey = key;
            _expireTime = expireTime;
            _serializeType = serializeType;
        }

        public async Task<T> GetObject()
        {
            var sqliteService = ShinyHost.Resolve<ISQLiteService>();

            var cachedData = await sqliteService.SelectAsync<AppCacheData>(this.CacheKey);
            if (cachedData == null || cachedData?.ExpireTime < DateTime.UtcNow)
            {
                var loadedData = await Loader.Invoke();
                if (loadedData == null)
                    return null;

                if (ExpireTime != TimeSpan.Zero)
                {
                    cachedData = new AppCacheData();
                    cachedData.BindCacheData(loadedData, CacheKey, ExpireTime, SerializeType);
                    await sqliteService.InsertOrUpdateAsync(cachedData);
                }
                else
                {
                    if (SerializeType == SerializeType.Json)
                        return (loadedData as string).JsonDeserialize<T>();
                    else if (SerializeType == SerializeType.MessagePack)
                    {
                        return MessagePackSerializer.Deserialize<T>((loadedData as byte[]));
                    }
                }
            }

            return DeserializeCacheData(cachedData);
        }

        private T DeserializeCacheData(AppCacheData cachedData)
        {
            T returnValue = default;

            if (cachedData.SerializeType == SerializeType.Json)
                returnValue = cachedData.CachedData.JsonDeserialize<T>();
            else if (cachedData.SerializeType == SerializeType.MessagePack)
            {
                var rawData = Convert.FromBase64String(cachedData.CachedData);
                returnValue = MessagePackSerializer.Deserialize<T>(rawData);
            }

            Debug.Assert(returnValue != null);

            return returnValue;
        }
    }
}