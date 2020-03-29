using LogicCore.Utility;
using Repository.Data.Redis.Cache;
using System;
using System.Threading.Tasks;

namespace Repository.Data.Redis
{
    public class RedisCacheManager : Singleton.INode
    {
        public T Get<T>(Func<string> loader, object lockObject, DateTime expireTime, string keys) where T : class
        {
            return new RedisCachedObject<T>(loader, lockObject, (expireTime - DateTime.Now), keys).CachedObject;
        }

        public Task<T> GetAsync<T>(Func<string> loader, object lockObject, DateTime expireTime, string keys) where T : class
        {
            return Task.Run(() => new RedisCachedObject<T>(loader, lockObject, (expireTime - DateTime.Now), keys).CachedObject);
        }

        public T GetNullable<T>(string keys) where T : class
        {
            return new RedisCachedObject<T>(null, new object(), TimeSpan.Zero, keys).CachedObject;
        }

        public void Set(string value, object lockObject, DateTime expireTime, string keys)
        {
            new RedisCachedObject<string>(null, lockObject, (expireTime - DateTime.Now), keys).SetCachedObject(value);
        }
    }
}