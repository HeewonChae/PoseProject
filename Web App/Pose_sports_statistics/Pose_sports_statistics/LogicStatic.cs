using LogicCore.Utility;

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