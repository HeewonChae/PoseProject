using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;
using Xamarin_Tutorial.Services;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LandsViewModel : BaseViewModel
	{
		#region ICarryViewPage Impl

		public override async Task<bool> PrepareView(params object[] data)
		{
			return await LoadCountries();
		}

		#endregion ICarryViewPage Impl

		#region Attributes

		public List<LandItemViewModel> _countryList;
		private ObservableCollection<LandItemViewModel> _countries;
		private string _filter;
		private bool _isRefresh;

		#endregion Attributes

		#region Properties

		public ObservableCollection<LandItemViewModel> Countries { get => _countries; set => SetValue(ref _countries, value); }
		public string Filter { get => _filter; set => SetValue(ref _filter, value); }
		public bool IsRefresh { get => _isRefresh; set => SetValue(ref _isRefresh, value); }

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

		private void RefreshCountries()
		{
			if (IsRefresh)
				return;

			IsRefresh = true;

			Countries = new ObservableCollection<LandItemViewModel>(_countryList);

			IsRefresh = false;
		}

		private void SearchCountries()
		{
			if (string.IsNullOrEmpty(Filter))
			{
				Countries = new ObservableCollection<LandItemViewModel>(_countryList);
			}
			else
			{
				Countries = new ObservableCollection<LandItemViewModel>(_countryList
					.Where(elem => elem.Name.ToLower().Contains(Filter)
					|| elem.Capital.ToLower().Contains(Filter)));
			}
		}

		#endregion Commands

		#region Services

		private async Task<bool> LoadCountries()
		{
			_countryList = await ApiService.RequestAsync<List<LandItemViewModel>>(
				WebServiceShare.WebConfig.WebMethodType.GET,
				"https://restcountries.eu/",
				"rest/v2/all");

			if (_countryList == null)
				return false;

			Countries = new ObservableCollection<LandItemViewModel>(_countryList);

			return true;
		}

		#endregion Services
	}
}