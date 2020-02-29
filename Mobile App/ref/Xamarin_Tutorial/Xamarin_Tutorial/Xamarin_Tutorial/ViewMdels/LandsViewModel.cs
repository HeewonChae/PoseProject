using GalaSoft.MvvmLight.Command;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;
using Xamarin_Tutorial.Services;
using Xamarin_Tutorial.Utilities;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LandsViewModel : BaseViewModel
	{
		private int noti_id = 3000;

		#region ICarryViewPage Impl

		public override async Task<bool> PrepareView(params object[] data)
		{
			return await LoadCountries();
		}

		#endregion ICarryViewPage Impl

		#region Attributes

		private List<CountryItem> _countryList;
		private ObservableCollection<CountryItem> _countries;
		private string _filter;
		private bool _isRefresh;

		#endregion Attributes

		#region Properties

		public ObservableCollection<CountryItem> Countries { get => _countries; set => SetValue(ref _countries, value); }
		public string Filter { get => _filter; set => SetValue(ref _filter, value); }
		public bool IsRefresh { get => _isRefresh; set => SetValue(ref _isRefresh, value); }

		#endregion Properties

		#region Commands

		public ICommand SelectCountryCommand
		{
			get
			{
				return new RelayCommand<CountryItem>(SelectCountry);
			}
		}

		public ICommand RefreshCommand
		{
			get
			{
				return new RelayCommand(RefreshCountries);
			}
		}

		public ICommand SearchCommand
		{
			get
			{
				return new RelayCommand(SearchCountries);
			}
		}

		private async void SelectCountry(CountryItem countryItem)
		{
			//var notification = new Notification
			//{
			//	Category = "default",
			//	Message = $"Notification Active Select:{countryItem.Name}",
			//};

			//var notification_scheduled = new Notification
			//{
			//	Category = "default",
			//	Message = $"Notification Active Select:{countryItem.Name}",
			//	ScheduleDate = DateTimeOffset.UtcNow.AddSeconds(20),
			//};

			//await ShinyHost.Resolve<INotificationManager>().Send(notification);
			//await ShinyHost.Resolve<INotificationManager>().Send(notification_scheduled);

			//var notification = new NotificationRequest
			//{
			//	NotificationId = noti_id++,
			//	Title = "Test",
			//	Description = "Test Description",
			//	ReturningData = "Dummy data", // Returning data when tapped on notification.
			//	NotifyTime = DateTime.Now.AddSeconds(10) // Used for Scheduling local notification, if not specified notification will show immediately.
			//};
			//NotificationCenter.Current.Show(notification);

			var request = new NotificationRequest
			{
				NotificationId = noti_id++,
				Title = "Test",
				Description = $"Notification Active Select:{countryItem.Name}",
				NotifyTime = DateTime.Now.AddSeconds(10),
				Android = new Plugin.LocalNotification.AndroidOptions
				{
					IconName = "ic_stat",
				},
			};

			NotificationCenter.Current.Show(request);

			await PageSwitcher.PushNavPageAsync(
					Singleton.Get<ViewLocator>().LandTabbed,
						null,
					countryItem,
					Singleton.Get<ViewLocator>().Land,
					Singleton.Get<ViewLocator>().Currency);
		}

		private void RefreshCountries()
		{
			if (IsRefresh)
				return;

			IsRefresh = true;

			Countries = new ObservableCollection<CountryItem>(_countryList);

			IsRefresh = false;
		}

		private void SearchCountries()
		{
			if (string.IsNullOrEmpty(Filter))
			{
				Countries = new ObservableCollection<CountryItem>(_countryList);
			}
			else
			{
				Countries = new ObservableCollection<CountryItem>(_countryList
					.Where(elem => elem.Name.ToLower().Contains(Filter)
					|| elem.Capital.ToLower().Contains(Filter)));
			}
		}

		#endregion Commands

		#region Services

		private async Task<bool> LoadCountries()
		{
			_countryList = await ApiService.RequestAsync<List<CountryItem>>(new WebServiceShare.ServiceContext.RequestContext()
			{
				MethodType = WebServiceShare.WebConfig.WebMethodType.GET,
				ServiceUrl = "https://restcountries.eu/rest/v2/all"
			});

			if (_countryList == null)
				return false;

			Countries = new ObservableCollection<CountryItem>(_countryList);

			return true;
		}

		#endregion Services
	}
}