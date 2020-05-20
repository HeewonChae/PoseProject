using PoseSportsPredict.InfraStructure.Cache;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Models.Cache;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.Services.Cache
{
    public class CacheService : ICacheService
    {
        #region ICacheService

        public Task<T> GetAsync<T>(Func<Task<object>> loader, string key, TimeSpan expireTime, SerializeType serializeType = SerializeType.MessagePack) where T : class
        {
            return new CachedObject<T>(loader, key, expireTime, serializeType).Cached;
        }

        public async Task DeleteExpiredCachedDataAsync()
        {
            var deletingCachedData = (await _sqliteService.SelectAllAsync<AppCacheData>()).Where(elem => elem.ExpireTime < DateTime.UtcNow);
            foreach (var cachedData in deletingCachedData)
            {
                await _sqliteService.DeleteAsync<AppCacheData>(cachedData.PrimaryKey);
            }
        }

        public async Task DeleteAllCachedDataAsync()
        {
            await _sqliteService.DeleteAllAsync<AppCacheData>();
        }

        #endregion ICacheService

        private ISQLiteService _sqliteService;

        public CacheService(ISQLiteService sqliteService)
        {
            _sqliteService = sqliteService;
        }
    }
}