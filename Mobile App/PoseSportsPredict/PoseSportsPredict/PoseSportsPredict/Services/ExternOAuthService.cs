using Acr.UserDialogs;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities.ExternOAuth.Providers;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.ViewModels;
using PoseSportsPredict.Views;
using Shiny;
using System;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ExternAuthentication.Providers;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Auth;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.Services
{
    public sealed class ExternOAuthService : IOAuthService
    {
        private bool _isAuthenticated;
        private ExternAuthUser _authenticatedUser;
        private IWebApiService _webApiService;

        public bool IsAuthenticated => _isAuthenticated;
        public ExternAuthUser AuthenticatedUser => _authenticatedUser;

        public ExternOAuthService(IWebApiService webApiService)
        {
            _webApiService = webApiService;
        }

        public static OAuth2Authenticator Cur_Authenticator;
        public static SNSProviderType Cur_Provider;

        public async Task OAuthLoginAsync(SNSProviderType provider)
        {
            if (!await WebApiService.CheckInternetConnection())
            {
                _isAuthenticated = false;
                return;
            }

            var oAuth = OAuthProviderFactory.CreateProvider(provider);
            Cur_Provider = oAuth.Provider;

            if (oAuth.Provider == SNSProviderType.Google)
            {
                Cur_Authenticator = new OAuth2Authenticator(
                    oAuth.ClientId,
                    null,
                    oAuth.Scope,
                    new Uri(oAuth.AuthorizationUrl),
                    new Uri(oAuth.RedirectUrl),
                    new Uri(oAuth.RequestTokenUrl),
                    null,
                    true);
            }
            else if (oAuth.Provider == SNSProviderType.Facebook)
            {
                Cur_Authenticator = new OAuth2Authenticator(
                   oAuth.ClientId,
                   oAuth.Scope,
                   new Uri(oAuth.AuthorizationUrl),
                   new Uri(oAuth.RedirectUrl));
            }

            Cur_Authenticator.Completed += OnOAuthComplete;
            Cur_Authenticator.Error += OnOAuthError;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(Cur_Authenticator);
        }

        public async void OnOAuthComplete(object sender, EventArgs args)
        {
            var eventArgs = args as AuthenticatorCompletedEventArgs;

            if (eventArgs.IsAuthenticated)
            {
                string token = eventArgs.Account.Properties["access_token"];

                using (UserDialogs.Instance.Loading(LocalizeString.Loginning))
                {
                    // P_E_CheckVaildOAuthUser
                    _authenticatedUser = await _webApiService.RequestAsync<ExternAuthUser>(new WebRequestContext
                    {
                        SerializeType = SerializeType.MessagePack,
                        MethodType = WebMethodType.POST,
                        BaseUrl = AppConfig.PoseWebBaseUrl,
                        ServiceUrl = AuthProxy.ServiceUrl,
                        SegmentGroup = AuthProxy.P_E_CheckVaildOAuthUser,
                        NeedEncrypt = true,
                        PostData = new I_CheckVaildOAuthUser
                        {
                            SNSProvider = Cur_Provider,
                            AccessToken = token,
                        }
                    });

                    if (_authenticatedUser != null)
                    {
                        // PoseLogin success
                        if (await ShinyHost.Resolve<LoginViewModel>().PoseLogin())
                        {
                            LocalStorage.Storage.GetValueOrDefault<bool>(LocalStorageKey.IsRememberAccount, out bool isRemeberAccount);
                            if (isRemeberAccount)
                            {
                                LocalStorage.Storage.AddOrUpdateValue(LocalStorageKey.SavedAuthenticatedUser, _authenticatedUser);
                            }
                        }
                        else
                        {
                            _authenticatedUser = null;
                        }
                    }
                }
            }

            ShinyHost.Resolve<LoginViewModel>().SetIsBusy(false);
        }

        public async void OnOAuthError(object sender, EventArgs args)
        {
            var eventArgs = args as AuthenticatorErrorEventArgs;

            await MaterialDialog.Instance.AlertAsync($"OAuth Error: {eventArgs.Message}",
                LocalizeString.App_Title,
                LocalizeString.Ok,
                DialogConfiguration.DefaultAlterDialogConfiguration);

            ShinyHost.Resolve<LoginViewModel>().SetIsBusy(false);
        }

        public Task<bool> IsAuthenticatedAndValid()
        {
            _isAuthenticated = false;
            LocalStorage.Storage.GetValueOrDefault<bool>(LocalStorageKey.IsRememberAccount, out bool isRemeberAccount);
            if (!isRemeberAccount)
                return Task.FromResult(_isAuthenticated);

            LocalStorage.Storage.GetValueOrDefault(LocalStorageKey.SavedAuthenticatedUser, out _authenticatedUser);

            // 저장된 인증 유저 없음
            if (_authenticatedUser == null)
                return Task.FromResult(_isAuthenticated);

            _isAuthenticated = true;

            return Task.FromResult(_isAuthenticated);
        }

        public async Task Logout()
        {
            _isAuthenticated = false;
            _authenticatedUser = null;
            LocalStorage.Storage.Remove(LocalStorageKey.SavedAuthenticatedUser);

            await PageSwitcher.SwitchMainPageAsync(ShinyHost.Resolve<LoginViewModel>(), true);
        }
    }
}