using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.InfraStructure.Cache
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(Func<Task<object>> loader, string key, TimeSpan expireTime, SerializeType serializeType) where T : class;

        Task DeleteExpiredCachedDataAsync();
    }
}