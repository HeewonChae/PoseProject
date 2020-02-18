using LogicCore.DataMapping;
using RapidAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Logic.WebAPI
{
	public static class RequestExtensions
	{
		public static void RequestEx<T1, T2, T3>(this IRapidAPI rapidAPI, Func<T1, IList<IList<T2>>> requsetMethod, T1 param, out IList<IList<T3>> result)
			where T3 : new()
		{
			result = new List<IList<T3>>();

			// invoke request
			IList<IList<T2>> response = requsetMethod.Invoke(param);

			if (response == null)
				return;

			foreach (IList<T2> datas in response)
			{
				var innerList = new List<T3>();
				result.Add(innerList);
				foreach (T2 data in datas)
				{
					innerList.Add(DataMapper.Map<T2, T3>(data));
				}
			}
		}

		public static void RequestEx<T1, T2>(this IRapidAPI rapidAPI, Func<IList<T1>> requsetMethod, out IList<T2> result)
			where T2 : new()
		{
			result = new List<T2>();

			// invoke request
			IList<T1> response = requsetMethod.Invoke();

			if (response == null)
				return;

			foreach (T1 data in response)
			{
				result.Add(DataMapper.Map<T1, T2>(data));
			}
		}

		public static void RequestEx<T1, T2, T3>(this IRapidAPI rapidAPI, Func<T1, IList<T2>> requsetMethod, T1 param, out IList<T3> result)
			where T3 : new()
		{
			result = new List<T3>();

			// invoke request
			IList<T2> response = requsetMethod.Invoke(param);

			if (response == null)
				return;

			foreach (T2 data in response)
			{
				result.Add(DataMapper.Map<T2, T3>(data));
			}
		}

		public static void RequestEx<T1, T2, T3, T4>(this IRapidAPI rapidAPI, Func<T1, T2, IList<T3>> requsetMethod, T1 param1, T2 param2, out IList<T4> result)
			where T4 : new()
		{
			result = new List<T4>();

			// invoke request
			IList<T3> response = requsetMethod.Invoke(param1, param2);

			if (response == null)
				return;

			foreach (T3 data in response)
			{
				result.Add(DataMapper.Map<T3, T4>(data));
			}
		}

		public static void RequestEx<T1, T2, T3>(this IRapidAPI rapidAPI, Func<T1, T2> requsetMethod, T1 param, out T3 result)
			where T3 : new()
		{
			result = default;

			// invoke request
			T2 response = requsetMethod.Invoke(param);

			if (response == null)
				return;

			result = DataMapper.Map<T2, T3>(response);
		}

		public static void RequestEx<T1, T2, T3, T4>(this IRapidAPI rapidAPI, Func<T1, T2, T3> requsetMethod, T1 param1, T2 param2, out T4 result)
			where T4 : new()
		{
			result = default;

			// invoke request
			T3 response = requsetMethod.Invoke(param1, param2);

			if (response == null)
				return;

			result = DataMapper.Map<T3, T4>(response);
		}
	}
}
