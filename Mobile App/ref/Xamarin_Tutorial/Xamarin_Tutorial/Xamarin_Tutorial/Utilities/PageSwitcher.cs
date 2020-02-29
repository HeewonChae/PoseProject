using Acr.UserDialogs;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.Utilities
{
	public static class PageSwitcher
	{
		public static async Task SwitchMainPageAsync(BaseViewModel viewModel, bool isNavPage = false, params object[] prepareData)
		{
			if (!await viewModel.PrepareView(prepareData))
				throw new Exception("[Fail] Prepare main page");

			if (isNavPage)
				Application.Current.MainPage = new NavigationPage(viewModel.CoupledView);
			else
				Application.Current.MainPage = viewModel.CoupledView;
		}

		public static async Task PushNavPageAsync(BaseViewModel viewModel, Page errorPage = null, params object[] prepareData)
		{
			if (Application.Current.MainPage is MasterDetailPage masterPage)
			{
				if (masterPage.Detail.Navigation == null)
					throw new Exception("Root page is not navigation page");

				if (!await viewModel.PrepareView(prepareData))
					await masterPage.Detail.Navigation.PushAsync(errorPage);

				await masterPage.Detail.Navigation.PushAsync(viewModel.CoupledView);
			}
			else
			{
				if (Application.Current.MainPage.Navigation == null)
					throw new Exception("Root page is not navigation page");

				if (!await viewModel.PrepareView(prepareData))
					await Application.Current.MainPage.Navigation.PushAsync(errorPage);

				await Application.Current.MainPage.Navigation.PushAsync(viewModel.CoupledView);
			}
		}
	}
}