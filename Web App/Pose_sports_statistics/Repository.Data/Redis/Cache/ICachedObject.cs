using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Redis.Cache
{
	public interface ICachedObject<T>
	{
		T CachedObject { get; }
		string CacheKey { get; }
		ICachedObject<T> By(params string[] cacheKeys);
		void SetCachedObject(string value);
	}
}
