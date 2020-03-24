using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views;
using Shiny;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels
{
    public class LoginViewModel : NavigableViewModel
    {
        #region Services

        private IOAuthService _OAuthService;
        private IWebApiService _webApiService;

        #endregion Services

        #region Constructors

        public LoginViewModel(LoginPage page,
            IOAuthService OAuthService,
            IWebApiService webApiService) : base(page)
        {
            _OAuthService = OAuthService;
            _webApiService = webApiService;

            CoupledPage.Appearing += (s, e) => OnAppearing();
        }

        #endregion Constructors

        #region Commands

        public ICommand LoginFacebookCommand { get => new RelayCommand(LoginFacebook); }

        private async void LoginFacebook()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            if (!_OAuthService.IsAuthenticated
                || _OAuthService.AuthenticatedUser.SNSProvider != SNSProviderType.Facebook)
                await _OAuthService.OAuthLoginAsync(SNSProviderType.Facebook);
            else
                await PoseLogin();
        }

        public ICommand LoginGoogleCommand { get => new RelayCommand(LoginGoogle); }

        private async void LoginGoogle()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            if (!_OAuthService.IsAuthenticated
                || _OAuthService.AuthenticatedUser.SNSProvider != SNSProviderType.Google)
                await _OAuthService.OAuthLoginAsync(SNSProviderType.Google);
            else
                await PoseLogin();
        }

        #endregion Commands

        #region Methods

        public async Task<bool> PoseLogin()
        {
            SetIsBusy(true);

            using (await MaterialDialog.Instance.LoadingSnackbarAsync(LocalizeString.Welcome))
            {
                var loginResult = await _webApiService.EncryptRequestAsync<O_Login>(new WebRequestContext
                {
                    MethodType = WebMethodType.POST,
                    BaseUrl = AppConfig.PoseWebBaseUrl,
                    ServiceUrl = AuthProxy.ServiceUrl,
                    SegmentGroup = AuthProxy.P_E_Login,
                    PostData = new I_Login
                    {
                        PlatformId = _OAuthService.AuthenticatedUser.Id,
                    }
                });

                if (loginResult == null)
                {
                    SetIsBusy(false);
                    return false;
                }

                // Update PoseToken, Update ExpireTime
                ClientContext.SetCredentialsFrom(loginResult.PoseToken);
                ClientContext.TokenExpireIn = DateTime.UtcNow.AddMilliseconds(loginResult.TokenExpireIn);
            }

            await PageSwitcher.SwitchMainPageAsync(ShinyHost.Resolve<AppMasterViewModel>());

            SetIsBusy(false);
            return true;
        }

        public override void SetIsBusy(bool isBusy)
        {
            base.SetIsBusy(isBusy);

            if (isBusy)
                UserDialogs.Instance.ShowLoading(LocalizeString.Loginning);
            else
                UserDialogs.Instance.HideLoading();
        }

        #endregion Methods
    }
}