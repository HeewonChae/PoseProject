using PoseSportsPredict.ViewModels.Base;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.Logics
{
    public static class PageSwitcher
    {
        public static async Task<bool> SwitchMainPageAsync(NavigableViewModel viewModel, bool isNavPage = false, params object[] prepareData)
        {
            if (!await viewModel.OnInitializeViewAsync(prepareData))
                throw new Exception($"Failed OnInitializeView, viewModel: {viewModel.GetType().FullName}");

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

            return true;
        }

        public static async Task<bool> PushNavPageAsync(NavigableViewModel viewModel, params object[] prepareData)
        {
            if (!await viewModel.OnInitializeViewAsync(prepareData))
                throw new Exception($"Failed OnInitializeView, viewModel: {viewModel.GetType().FullName}");

            if (Application.Current.MainPage is MasterDetailPage masterPage)
            {
                if (masterPage.Detail.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                await masterPage.Detail.Navigation.PushAsync(viewModel.CoupledPage);
            }
            else
            {
                if (Application.Current.MainPage.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                await Application.Current.MainPage.Navigation.PushAsync(viewModel.CoupledPage);
            }

            return true;
        }

        public static async Task<bool> PushModalPageAsync(NavigableViewModel viewModel, params object[] prepareData)
        {
            if (!await viewModel.OnInitializeViewAsync(prepareData))
                throw new Exception($"Failed OnInitializeView, viewModel: {viewModel.GetType().FullName}");

            if (Application.Current.MainPage is MasterDetailPage masterPage)
            {
                if (masterPage.Detail.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                await masterPage.Detail.Navigation.PushModalAsync(new MaterialNavigationPage(viewModel.CoupledPage));
            }
            else
            {
                if (Application.Current.MainPage.Navigation == null)
                    throw new Exception("Root page is not navigation page");

                await Application.Current.MainPage.Navigation.PushModalAsync(new MaterialNavigationPage(viewModel.CoupledPage));
            }

            return true;
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