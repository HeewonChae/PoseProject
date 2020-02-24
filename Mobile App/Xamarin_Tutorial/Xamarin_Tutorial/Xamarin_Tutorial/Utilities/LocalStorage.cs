using Plugin.Settings;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.Utilities
{
	public static class StorageKey
	{
		public static string UserEmail = "UserEmail";
		public static string UserPassword = "UserPassword";
		public static string IsAccountRemember = "IsAccountRemember";
	}

	public class LocalStorage
	{
		#region Singleton Pattern

		public static readonly LocalStorage Storage = new LocalStorage();

		private LocalStorage()
		{
		}

		#endregion Singleton Pattern

		private readonly object _locker = new object();
		private readonly string _defaultValue = string.Empty;

		public string this[string key]
		{
			get
			{
				lock (_locker)
				{
					return CrossSettings.Current.GetValueOrDefault(key, _defaultValue);
				}
			}

			set
			{
				lock (_locker)
				{
					CrossSettings.Current.AddOrUpdateValue(key, value);
				}
			}
		}
	}
}