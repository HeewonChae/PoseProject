using System;
using System.Threading.Tasks;

namespace LogicCore.Thread
{
	public static class AsyncHelper
	{
		#region Action Type

		public static Task Async(Action action)
		{
			return Task.Run(action);
		}

		public static Task Async<T1>(Action<T1> action, T1 input)
		{
			return Task.Run(() => action(input));
		}

		#endregion Action Type

		#region Func Type

		public static Task<T1> Async<T1>(Func<T1> func)
		{
			return Task.Run(func);
		}

		public static Task<T2> Async<T1, T2>(Func<T1, T2> func, T1 param1)
		{
			return Task.Run(() => func.Invoke(param1));
		}

		public static Task<T3> Async<T1, T2, T3>(Func<T1, T2, T3> func, T1 param1, T2 parma2)
		{
			return Task.Run(() => func.Invoke(param1, parma2));
		}

		public static Task<T4> Async<T1, T2, T3, T4>(Func<T1, T2, T3, T4> func, T1 param1, T2 parma2, T3 param3)
		{
			return Task.Run(() => func.Invoke(param1, parma2, param3));
		}

		public static Task<T2> Async<T1, T2>(Func<T1[], T2> func, params T1[] param1)
		{
			return Task.Run(() => func.Invoke(param1));
		}

		public static Task<T4> Async<T1, T2, T3, T4>(Func<T1, T2, T3[], T4> func, T1 param1, T2 param2, params T3[] param3)
		{
			return Task.Run(() => func.Invoke(param1, param2, param3));
		}

		#endregion Func Type
	}
}