using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin_Tutorial.InfraStructure
{
	public class Singleton
	{
		public interface INode { }

		class Holder<T> where T : class, INode, new()
		{
			static T instance = null;

			public static T Instance
			{
				get
				{
					return instance ?? throw new Exception($"this instence not alloced Type: {typeof(T)}");
				}
			}
			public static bool HasInstance
			{
				get { return instance != null; }
			}

			public static void SetInstance(T newInstance)
			{
				if (instance != null)
				{
					throw new System.Exception($"Duplicate singleton Type: {typeof(T)}");
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
