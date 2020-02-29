using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;

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

			CountryName = countryInfo.Name;

			// Setting LandPage
			if (data.Length > 1 && data[1] is LandViewModel landViewModel)
			{
				if (!await landViewModel.PrepareView(countryInfo))
					return await Task.FromResult(false);

				tabbedPage.Children.Add(landViewModel.CoupledView);
			}

			// Setting CurrencyPage
			if (data.Length > 2 && data[2] is CurrencyViewModel currencyViewModel)
			{
				if (!await currencyViewModel.PrepareView(countryInfo))
					return await Task.FromResult(false);

				tabbedPage.Children.Add(currencyViewModel.CoupledView);
			}

			return await Task.FromResult(true);
		}

		#endregion BaseViewModel Impl
	}
}