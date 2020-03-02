using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using Plugin.LocalNotification;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;

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

        public ICommand LoginFacebookCommand
        {
            get
            {
                return new RelayCommand(LoginFacebook);
            }
        }

        private async void LoginFacebook()
        {
            if (IsBusy)
                return;

            SetBusy(true);

            if (!_OAuthService.IsAuthenticated
                || _OAuthService.AuthenticatedUser.SNSProvider != SNSProviderType.Facebook)
                await _OAuthService.OAuthLoginAsync(SNSProviderType.Facebook);
            else
                await PoseWebLogin();
        }

        #endregion Commands

        #region Methods

        public async Task<bool> PoseWebLogin()
        {
            var loginResult = await _webApiService.EncrpytRequestAsyncWithToken<O_Login>(new WebRequestContext
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
                _OAuthService.Logout();
                SetBusy(false);
                return false;
            }

            ClientContext.SetCredentialsFrom(loginResult.PoseToken);
            LocalStorage.Storage.AddOrUpdateValue(LocalStorageKey.PoseTokenExpireTime, DateTime.UtcNow.AddMilliseconds(loginResult.TokenExpireIn));

            await UserDialogs.Instance.AlertAsync("Login Completed");

            SetBusy(false);
            return true;
        }

        #endregion Methods
    }
}