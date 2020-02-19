using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.Models;
using Xamarin_Tutorial.Services;
using Xamarin_Tutorial.Views;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LandsViewModel : BaseViewModel
	{
		#region Attributes
		private ObservableCollection<CountryInfo> _countries;
		#endregion

		#region Properties
		public ObservableCollection<CountryInfo> Countries { get => _countries; set => SetValue(ref _countries, value); }
		#endregion

		#region Constructors
		public LandsViewModel()
		{
		}
		#endregion

		#region Method
		public override async Task<Page> GetViewPage()
		{
			var loadResult = await LoadCountries();
			if (loadResult)
				return new LandsPage();

			return null;
		}
		
		private async Task<bool> LoadCountries()
		{
			if (_countries != null)
				return true;

			var result = await ApiService.RequestAsync<CountryInfo[]>(
				WebServiceShare.WebConfig.WebMethodType.GET,
				"https://restcountries.eu/",
				"rest/v2/all");

			if (result == null)
				return false;

			_countries = new ObservableCollection<CountryInfo>(result);

			return true;
		}
		#endregion
	}
}
