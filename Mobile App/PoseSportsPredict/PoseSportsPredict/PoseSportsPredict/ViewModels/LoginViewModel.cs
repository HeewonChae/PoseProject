using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Common;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.Views;
using Shiny;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override async Task<bool> PrepareView(params object[] data)
        {
            return await Task.FromResult(true);
        }

        #endregion BaseViewModel

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
        }

        #endregion Constructors

        #region Commands

        public ICommand LoginFacebookCommand { get => new RelayCommand(LoginFacebook); }

        private async void LoginFacebook()
        {
            if (IsBusy)
                return;

            SetBusy(true);

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

            SetBusy(true);

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
            SetBusy(true);

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
            }, false);

            if (loginResult == null)
            {
                await _OAuthService.Logout();
                SetBusy(false);
                return false;
            }

            // Update PoseToken, Update ExpireTime
            ClientContext.SetCredentialsFrom(loginResult.PoseToken);
            LocalStorage.Storage.AddOrUpdateValue(LocalStorageKey.PoseTokenExpireTime, DateTime.UtcNow.AddMilliseconds(loginResult.TokenExpireIn));

            await PageSwitcher.SwitchMainPageAsync(ShinyHost.Resolve<AppShellViewModel>());

            SetBusy(false);
            return true;
        }

        #endregion Methods
    }
}