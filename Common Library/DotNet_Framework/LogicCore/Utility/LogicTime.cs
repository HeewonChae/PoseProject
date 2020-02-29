using System;

namespace LogicCore.Utility
{
	public class LogicTime
	{
		public static Lazy<long> TimeTicks = new Lazy<long>(() => DateTime.UtcNow.Ticks, true);

		public static long TIME()
		{
			var diff = DateTime.UtcNow.Ticks - TimeTicks.Value;

			return diff / TimeSpan.TicksPerMillisecond; // 프로그램 시작부터 몇밀리세컨드가 흘렀는지..
		}
	}
}