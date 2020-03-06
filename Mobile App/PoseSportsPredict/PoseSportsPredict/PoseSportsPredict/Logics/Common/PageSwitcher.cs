using PoseSportsPredict.InfraStructure;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Common
{
    public static class PageSwitcher
    {
        public static async Task SwitchMainPageAsync(BaseViewModel viewModel, bool isNavPage = false, params object[] prepareData)
        {
            if (!await viewModel.PrepareView(prepareData))
                throw new Exception("[Fail] Prepare main page");

            if (isNavPage)
                Application.Current.MainPage = new NavigationPage(viewModel.CoupledPage);
            else
                Application.Current.MainPage = viewModel.CoupledPage;

            await viewModel.ShowedView();
        }

        public static async Task PushNavPageAsync(BaseViewModel viewModel, Page errorPage = null, params object[] prepareData)
        {
            if (Application.Current.MainPage is MasterDetailPage masterPage)
            {
                if (masterPage.Detail.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                if (!await viewModel.PrepareView(prepareData))
                    await masterPage.Detail.Navigation.PushAsync(errorPage);

                await masterPage.Detail.Navigation.PushAsync(viewModel.CoupledPage);
            }
            else
            {
                if (Application.Current.MainPage.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                if (!await viewModel.PrepareView(prepareData))
                    await Application.Current.MainPage.Navigation.PushAsync(errorPage);

                await Application.Current.MainPage.Navigation.PushAsync(viewModel.CoupledPage);
            }

            await viewModel.ShowedView();
        }

        public static async Task PushModalPageAsync(BaseViewModel viewModel, Page errorPage = null, params object[] prepareData)
        {
            if (Application.Current.MainPage is MasterDetailPage masterPage)
            {
                if (masterPage.Detail.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                if (!await viewModel.PrepareView(prepareData))
                    await masterPage.Detail.Navigation.PushAsync(errorPage);

                await masterPage.Detail.Navigation.PushModalAsync(new NavigationPage(viewModel.CoupledPage));
            }
            else
            {
                if (Application.Current.MainPage.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                if (!await viewModel.PrepareView(prepareData))
                    await Application.Current.MainPage.Navigation.PushAsync(errorPage);

                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(viewModel.CoupledPage));
            }

            await viewModel.ShowedView();
        }

        public static async Task PopModalAsync()
        {
            if (Application.Current.MainPage is MasterDetailPage masterPage)
            {
                if (masterPage.Detail.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                await masterPage.Detail.Navigation.PopModalAsync();
            }
            else
            {
                if (Application.Current.MainPage.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
    }
}