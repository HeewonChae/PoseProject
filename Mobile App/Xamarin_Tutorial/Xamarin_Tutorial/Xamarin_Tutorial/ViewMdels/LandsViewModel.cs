using GalaSoft.MvvmLight.Command;
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

namespace Xamarin_Tutorial.ViewMdels
{
	public class LandsViewModel : BaseViewModel
	{
		#region ICarryViewPage Impl

		public override async Task<Page> ShowView()
		{
			var loadResult = await LoadCountries();
			if (!loadResult) // 실패시 로그인 화면으로
				return await Singleton.Get<ViewLocator>().Login.ShowView();

			return _coupledView;
		}

		#endregion ICarryViewPage Impl

		#region Attributes

		public List<CountryItem> _countryList;
		private ObservableCollection<CountryItem> _countries;
		private string _filter;

		#endregion Attributes

		#region Properties

		public ObservableCollection<CountryItem> Countries { get => _countries; set => SetValue(ref _countries, value); }
		public string Filter { get => _filter; set => SetValue(ref _filter, value); }

		#endregion Properties

		#region Constructors

		public LandsViewModel()
		{
		}

		#endregion Constructors

		#region Commands

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

		public ICommand SelectCountryCommand
		{
			get
			{
				return new RelayCommand(SelectCountry);
			}
		}

		#endregion Commands

		#region Commands Impl

		private void RefreshCountries()
		{
			throw new NotImplementedException();
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
					.Where(elem => elem.Name.ToLower().Contains(Filter)));
			}
		}

		private void SelectCountry()
		{
			throw new NotImplementedException();
		}

		#endregion Commands Impl

		#region Services

		private async Task<bool> LoadCountries()
		{
			_countryList = await ApiService.RequestAsync<List<CountryItem>>(
				WebServiceShare.WebConfig.WebMethodType.GET,
				"https://restcountries.eu/",
				"rest/v2/all");

			if (_countryList == null)
				return false;

			Countries = new ObservableCollection<CountryItem>(_countryList);

			return true;
		}

		#endregion Services
	}
}