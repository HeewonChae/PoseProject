using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;
using Xamarin_Tutorial.Views;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LandTabbedViewModel : BaseViewModel
	{
		#region Attributes

		private string _countryName;

		#endregion Attributes

		#region Properties

		public string CountryName { get => _countryName; set => SetValue(ref _countryName, value); }

		#endregion Properties

		#region BaseViewModel Impl

		public override async Task<bool> PrepareView(params object[] data)
		{
			if (data == null || data.Length == 0)
				return await Task.FromResult(false);

			var tabbedPage = this.CoupledView as TabbedPage;
			tabbedPage.Children.Clear();

			var countryInfo = data[0] as CountryItem;
			if (countryInfo == null)
				return await Task.FromResult(false);

			// Setting LandPage
			if (data[1] is LandViewModel landViewModel)
			{
				CountryName = countryInfo.Name;
				if (!await landViewModel.PrepareView(countryInfo))
					return await Task.FromResult(false);

				tabbedPage.Children.Add(landViewModel.CoupledView);
			}

			// Setting CurrencyPage
			if (data[2] is CurrencyViewModel currencyViewModel)
			{
				CountryName = countryInfo.Name;
				if (!await currencyViewModel.PrepareView(countryInfo))
					return await Task.FromResult(false);

				tabbedPage.Children.Add(currencyViewModel.CoupledView);
			}

			return await Task.FromResult(true);
		}

		#endregion BaseViewModel Impl
	}
}