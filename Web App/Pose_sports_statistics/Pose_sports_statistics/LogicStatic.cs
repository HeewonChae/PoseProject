using LogicCore;
using Pose_sports_statistics.Logic;
using RapidAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics
{
	public static class LogicStatic
	{
		public static void Init()
		{
			Singleton.Register<Repository.Data.Redis.RedisCacheManager>();
		}
	}
}