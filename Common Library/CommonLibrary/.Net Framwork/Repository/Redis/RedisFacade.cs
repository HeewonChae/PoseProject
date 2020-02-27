using Repository.Redis.Config;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Redis
{
	public static class RedisFacade
	{
		static readonly Dictionary<string, RedisConnector> _connectors = new Dictionary<string, RedisConnector>();

		internal static void AddConnection(RedisConfigurationSection configuration)
		{
			_connectors.Add(configuration.name, new RedisConnector(configuration.ConnectionString));
		}

		public static IDatabase GetRedis (string connectionName)
		{
			_connectors.TryGetValue(connectionName, out RedisConnector connector);
			return connector.RedisClient.GetDatabase();
		}
	}
}
