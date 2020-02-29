using Dapper;
using Repository.Redis;
using Repository.Redis.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public static class RepositoryStatic
	{
		public static void Init_Mysql()
		{
			SqlMapper.AddTypeHandler(new Mysql.Dapper.DapperTypeHandler.DateTimeHandler());
		}

		public static void Init_Redis(params RedisConfigurationSection[] configurations)
		{
			foreach (var ridisConfig in configurations)
			{
				RedisFacade.AddConnection(ridisConfig);
			}
		}
	}
}
