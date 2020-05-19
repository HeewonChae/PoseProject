using PoseSportsPredict.Models.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.InfraStructure.Cache
{
    public interface ICachedObject<T>
    {
        Task<T> Cached { get; }
        string CacheKey { get; }
        Func<Task<object>> Loader { get; }
        TimeSpan ExpireTime { get; }
        SerializeType SerializeType { get; }
    }
}