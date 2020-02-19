using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;
using Xamarin_Tutorial.Services;
using Xamarin_Tutorial.Views;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LandsViewModel : BaseViewModel
	{
		#region Attributes
		private ObservableCollection<CountryItem> _countries;
		#endregion

		#region Properties
		public ObservableCollection<CountryItem> Countries { get => _countries; set => SetValue(ref _countries, value); }
		#endregion

		#region Constructors
		public LandsViewModel() {}
		#endregion

		#region Methods
		public override async Task<Page> ShowViewPage()
		{
			var loadResult = await LoadCountries();
			if (loadResult)
				return _coupledViewPage;

			return await Singleton.Get<MainViewModel>().Login.ShowViewPage();
		}
		#endregion

		#region Services
		private async Task<bool> LoadCountries()
		{
			if (Countries != null)
				return true;

			var result = await ApiService.RequestAsync<CountryItem[]>(
				WebServiceShare.WebConfig.WebMethodType.GET,
				"https://restcountries.eu/",
				"rest/v2/all");

			if (result == null)
				return false;

			Countries = new ObservableCollection<CountryItem>(result);

			return true;
		} 
		#endregion
	}
}
