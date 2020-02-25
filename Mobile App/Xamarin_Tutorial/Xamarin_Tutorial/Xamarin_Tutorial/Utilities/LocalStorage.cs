using Newtonsoft.Json;
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

		public T GetValueOrDefault<T>(string key)
		{
			string savedValue = this[key];
			if (savedValue.Equals(_defaultValue))
				return default;

			return JsonConvert.DeserializeObject<T>(savedValue);
		}

		public void AddOrUpdateValue<T>(string key, T @value)
		{
			string serializeValue = JsonConvert.SerializeObject(@value);

			this[key] = serializeValue;
		}
	}
}