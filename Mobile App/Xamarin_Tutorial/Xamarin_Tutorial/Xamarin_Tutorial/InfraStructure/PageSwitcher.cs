using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin_Tutorial.InfraStructure
{
	public static class PageSwitcher
	{
		public static async Task SwitchMainPageAsync(BaseViewModel viewModel, Page errorPage = null, bool isNavPage = false, params object[] prepareData)
		{
			if (!await viewModel.PrepareView(prepareData))
				Application.Current.MainPage = errorPage;

			if (isNavPage)
				Application.Current.MainPage = new NavigationPage(viewModel.CoupledView);
			else
				Application.Current.MainPage = viewModel.CoupledView;
		}

		public static async Task PushNavPageAsync(BaseViewModel viewModel, Page errorPage = null, params object[] prepareData)
		{
			if (Application.Current.MainPage.Navigation == null)
				throw new Exception("Root page is not navigation page");

			if (!await viewModel.PrepareView(prepareData))
				await Application.Current.MainPage.Navigation.PushAsync(errorPage);

			await Application.Current.MainPage.Navigation.PushAsync(viewModel.CoupledView);
		}
	}
}