using System;
using System.Threading;

namespace LogicCore.Utility
{
	public class Singleton
	{
		public interface INode { }

		private class Holder<T> where T : class, INode, new()
		{
			private static readonly Lazy<T> lazyInstance = new Lazy<T>(() => new T(), LazyThreadSafetyMode.ExecutionAndPublication);
			private static T instance = null;

			public static T Instance
			{
				get
				{
					return instance ?? lazyInstance.Value;
				}
			}

			public static bool HasInstance
			{
				get { return (instance != null) || (lazyInstance.IsValueCreated == true); }
			}

			public static void SetInstance(T newInstance)
			{
				if (instance != null && instance != newInstance || lazyInstance.IsValueCreated == true)
				{
					throw new System.Exception("Duplicate singleton " + typeof(T));
				}

				instance = instance ?? newInstance;
			}
		}

		public static bool Exists<T>() where T : class, INode, new()
		{
			return Holder<T>.HasInstance;
		}

		public static T Get<T>() where T : class, INode, new()
		{
			return Holder<T>.Instance;
		}

		public static void Register<T>(T instance) where T : class, INode, new()
		{
			Holder<T>.SetInstance(instance);
		}

		public static void Register<T>() where T : class, INode, new()
		{
			Register(new T());
		}
	}
}