using Newtonsoft.Json;
using Repository.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Redis.Cache
{
	internal class RedisCachedObject<T> : ICachedObject<T> where T : class
	{
		private readonly Object _loaderLock;
		private readonly Func<string> _loader;
		private TimeSpan _expireSpan;
		readonly List<string> _keys = new List<string>();

		#region Impl ICachedObject
		public T CachedObject => GetObject();
		public string CacheKey => string.Join(":", _keys);

		private T GetObject()
		{
			// select redis
			var redisClient = RedisFacade.GetRedis("redis_cache");
			var cachedItem = redisClient.StringGet(CacheKey);

			if (cachedItem.IsNullOrEmpty && _loader != null)
			{
				lock (_loaderLock)
				{
					var resultPreviousLoaded = redisClient.StringGet(CacheKey);
					if (resultPreviousLoaded.HasValue)
					{
						return JsonConvert.DeserializeObject<T>(cachedItem);
					}

					cachedItem = _loader();
#if RELEASE
					// insert redis
					redisClient.StringSet(CacheKey, cachedItem, _expireSpan);
#endif
				}
			}

			return cachedItem.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<T>(cachedItem);
		}

		public void SetCachedObject(string value)
		{
			lock (_loaderLock)
			{
				// select redis
				var redisClient = RedisFacade.GetRedis("redis_cache");

				// delete exist data
				redisClient.KeyDelete(CacheKey);

				// insert redis
				redisClient.StringSet(CacheKey, value, _expireSpan);
			}
		}

		public ICachedObject<T> By(params string[] cacheKeys)
		{
			_keys.AddRange(cacheKeys);
			return this;
		}
#endregion

		internal RedisCachedObject(Func<string> loader, Object lockObject, TimeSpan expireSpan, params string[] key)
		{
			if (expireSpan != TimeSpan.Zero && expireSpan.TotalSeconds < 0)
				Trace.Assert(false, "[RedisCachedObject] Invalid expireSpan");

			_loader = loader;
			_loaderLock = lockObject;
			_expireSpan = expireSpan;
			By(key);
		}
	}
}
