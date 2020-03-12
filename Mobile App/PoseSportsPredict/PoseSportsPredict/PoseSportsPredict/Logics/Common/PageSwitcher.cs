using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.ViewModels.Base;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.Logics.Common
{
    public static class PageSwitcher
    {
        public static bool SwitchMainPageAsync(NavigableViewModel viewModel, bool isNavPage = false, params object[] prepareData)
        {
            if (!viewModel.OnPrepareView(prepareData))
                throw new Exception("[Fail] Prepare main page");

            if (Application.Current.MainPage is MasterDetailPage masterPage)
            {
                if (isNavPage)
                    masterPage.Detail = new MaterialNavigationPage(viewModel.CoupledPage);
                else
                    masterPage.Detail = viewModel.CoupledPage;
            }
            else
            {
                if (isNavPage)
                    Application.Current.MainPage = new MaterialNavigationPage(viewModel.CoupledPage);
                else
                    Application.Current.MainPage = viewModel.CoupledPage;
            }

            return viewModel.OnApearing();
        }

        public static async Task<bool> PushNavPageAsync(NavigableViewModel viewModel, Page errorPage = null, params object[] prepareData)
        {
            if (Application.Current.MainPage is MasterDetailPage masterPage)
            {
                if (masterPage.Detail.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                if (!viewModel.OnPrepareView(prepareData))
                    await masterPage.Detail.Navigation.PushAsync(errorPage);

                await masterPage.Detail.Navigation.PushAsync(viewModel.CoupledPage);
            }
            else
            {
                if (Application.Current.MainPage.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                if (!viewModel.OnPrepareView(prepareData))
                    await Application.Current.MainPage.Navigation.PushAsync(errorPage);

                await Application.Current.MainPage.Navigation.PushAsync(viewModel.CoupledPage);
            }

            return viewModel.OnApearing();
        }

        public static async Task<bool> PushModalPageAsync(NavigableViewModel viewModel, Page errorPage = null, params object[] prepareData)
        {
            if (Application.Current.MainPage is MasterDetailPage masterPage)
            {
                if (masterPage.Detail.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                if (!viewModel.OnPrepareView(prepareData))
                    await masterPage.Detail.Navigation.PushAsync(errorPage);

                await masterPage.Detail.Navigation.PushModalAsync(new MaterialNavigationPage(viewModel.CoupledPage));
            }
            else
            {
                if (Application.Current.MainPage.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                if (!viewModel.OnPrepareView(prepareData))
                    await Application.Current.MainPage.Navigation.PushAsync(errorPage);

                await Application.Current.MainPage.Navigation.PushModalAsync(new MaterialNavigationPage(viewModel.CoupledPage));
            }

            return viewModel.OnApearing();
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