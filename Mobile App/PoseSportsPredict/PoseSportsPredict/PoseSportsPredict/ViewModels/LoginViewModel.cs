using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using PosePacket.Service.Enum;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.Views;
using Shiny;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels
{
    public class LoginViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            LocalStorage.Storage.GetValueOrDefault<bool>(LocalStorageKey.IsRememberAccount, out bool isRemeberAccount);
            IsRemeberAccount = isRemeberAccount;

            return base.OnPrepareViewAsync(datas);
        }

        #endregion NavigableViewModel

        #region Fields

        private bool _isRememberAccount;

        #endregion Fields

        #region Properties

        public bool IsRemeberAccount { get => _isRememberAccount; set => SetValue(ref _isRememberAccount, value); }

        #endregion Properties

        #region Services

        private IOAuthService _OAuthService;

        #endregion Services

        #region Constructors

        public LoginViewModel(LoginPage page,
            IOAuthService OAuthService) : base(page)
        {
            _OAuthService = OAuthService;

            CoupledPage.Appearing += (s, e) => OnAppearing();
        }

        #endregion Constructors

        #region Commands

        public ICommand ExternLoginCommand { get => new RelayCommand<string>(e => ExternLogin(e)); }

        private async void ExternLogin(string str_snsType)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            str_snsType.TryParseEnum<SNSProviderType>(out SNSProviderType snsProviderType);

            await _OAuthService.OAuthLoginAsync(snsProviderType);
        }

        public ICommand RemeberAccountToggleCommand { get => new RelayCommand<ToggledEventArgs>(e => RemeberAccountToggle(e)); }

        private void RemeberAccountToggle(ToggledEventArgs toggleArgs)
        {
            if (toggleArgs.Value)
            {
                LocalStorage.Storage.AddOrUpdateValue(LocalStorageKey.IsRememberAccount, true);
            }
            else
            {
                LocalStorage.Storage.Remove(LocalStorageKey.IsRememberAccount);
            }
        }

        #endregion Commands

        #region Methods

        public async Task<bool> PoseLogin(bool isAutoLogin)
        {
            var loginRet = await LoginFacade.ExternOAuthLogin();
            if (!loginRet)
                return false;

            await MaterialDialog.Instance.SnackbarAsync(LocalizeString.Welcome);

            if (isAutoLogin)
            {
                await PageSwitcher.SwitchMainPageAsync(ShinyHost.Resolve<AppMasterViewModel>(), true);
                await PageUriLinker.GoUrlLinkedPage();
            }
            else
                await PageSwitcher.PopNavPageAsync();

            // Restore PurchasedItem
            var inAppBillingService = ShinyHost.Resolve<InAppBillingService>();
            var restore_ret = await inAppBillingService.RestorePurchasedItem();

            return true;
        }

        public async Task<bool> GuestLogin()
        {
            var loginRet = await LoginFacade.GuestLogin();
            if (!loginRet)
                return false;

            await MaterialDialog.Instance.SnackbarAsync(LocalizeString.Welcome);
            await PageSwitcher.SwitchMainPageAsync(ShinyHost.Resolve<AppMasterViewModel>(), true);
            await PageUriLinker.GoUrlLinkedPage();

            return true;
        }

        public override void SetIsBusy(bool isBusy)
        {
            base.SetIsBusy(isBusy);

            if (isBusy)
                UserDialogs.Instance.ShowLoading(LocalizeString.Loading);
            else
                UserDialogs.Instance.HideLoading();
        }

        #endregion Methods
    }
}