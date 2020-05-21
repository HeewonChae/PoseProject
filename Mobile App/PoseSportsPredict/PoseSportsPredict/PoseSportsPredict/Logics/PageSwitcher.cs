using PoseSportsPredict.ViewModels.Base;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

namespace PoseSportsPredict.Logics
{
    public static class PageSwitcher
    {
        public static async Task<bool> SwitchMainPageAsync(NavigableViewModel viewModel, bool isNavPage = false, params object[] prepareData)
        {
            if (!await viewModel.OnPrepareViewAsync(prepareData))
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
            if (!await viewModel.OnPrepareViewAsync(prepareData))
                throw new Exception($"Failed OnInitializeView, viewModel: {viewModel.GetType().FullName}");

            await Application.Current.MainPage.Navigation.PushAsync(viewModel.CoupledPage, true);

            return true;
        }

        public static async Task PopNavPageAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        //public static async Task<bool> PushModalPageAsync(NavigableViewModel viewModel, params object[] prepareData)
        //{
        //    if (!await viewModel.OnPrepareViewAsync(prepareData))
        //        throw new Exception($"Failed OnInitializeView, viewModel: {viewModel.GetType().FullName}");

        //    await Application.Current.MainPage.Navigation.PushModalAsync(new MaterialNavigationPage(viewModel.CoupledPage));

        //    return true;
        //}

        //public static async Task PopModalPageAsync()
        //{
        //    await Application.Current.MainPage.Navigation.PopModalAsync();
        //}

        public static async Task<bool> PushPopupAsync(NavigableViewModel viewModel, params object[] prepareData)
        {
            if (!await viewModel.OnPrepareViewAsync(prepareData))
                throw new Exception($"Failed OnInitializeView, viewModel: {viewModel.GetType().FullName}");

            if (viewModel.CoupledPage is PopupPage popupPage)
                await Application.Current.MainPage.Navigation.PushPopupAsync(popupPage);
            else
                return false;

            return true;
        }

        public static async Task PopPopupAsync()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();
        }
    }
}