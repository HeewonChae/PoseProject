using Acr.UserDialogs;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logic.ExternOAuth.Providers;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.ViewModels;
using Shiny;
using System;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Auth;

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

        public async Task OAuthLoginAsync(SNSProviderType provider)
        {
            try
            {
                if (!await WebApiService.CheckInternetConnection())
                {
                    _isAuthenticated = false;
                    return;
                }

                var oAuth = OAuthProviderFactory.CreateProvider(provider);
                var authenticator = new OAuth2Authenticator(
                    oAuth.ClientId,
                    oAuth.Scope,
                    new Uri(oAuth.AuthorizationUrl),
                    new Uri(oAuth.RedirectUrl));

                // Completed
                authenticator.Completed += async (sender, eventArgs) =>
                {
                    if (eventArgs.IsAuthenticated)
                    {
                        string token = eventArgs.Account.Properties["access_token"];

                        // P_E_CheckVaildOAuthUser
                        _authenticatedUser = await _webApiService.EncrpytRequestAsyncWithToken<ExternAuthUser>(new WebRequestContext
                        {
                            MethodType = WebMethodType.POST,
                            BaseUrl = AppConfig.PoseWebBaseUrl,
                            ServiceUrl = AuthProxy.ServiceUrl,
                            SegmentGroup = AuthProxy.P_E_CheckVaildOAuthUser,
                            PostData = new I_CheckVaildOAuthUser
                            {
                                SNSProvider = provider,
                                AccessToken = token,
                            }
                        });
                        if (_authenticatedUser == null)
                            return;

                        await UserDialogs.Instance.AlertAsync("OAuth Completed");

                        // PoseWebLogin
                        var loginResult = await ShinyHost.Resolve<LoginViewModel>().PoseWebLogin();
                        if (!loginResult)
                        {
                            _authenticatedUser = null;
                            return;
                        }

                        LocalStorage.Storage.AddOrUpdateValue(LocalStorageKey.SavedAuthenticatedUser, _authenticatedUser);
                        _isAuthenticated = true;
                    }
                };

                // Error
                authenticator.Error += async (sender, eventArgs) =>
                {
                    await UserDialogs.Instance.AlertAsync($"OAuth error: {eventArgs.Message}");
                };

                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(authenticator);

                // 로그인 폼 닫힘
                presenter.Completed += (sender, eventArgs) =>
                {
                    ShinyHost.Resolve<LoginViewModel>().SetBusy(false);
                };
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync($"OAuth exception: {ex.Message}");
            }
        }

        public async Task<bool> IsAuthenticatedAndValid()
        {
            _isAuthenticated = false;

            LocalStorage.Storage.GetValueOrDefault(LocalStorageKey.SavedAuthenticatedUser, out _authenticatedUser);

            // 저장된 인증 유저 없음
            if (_authenticatedUser == null)
                return _isAuthenticated;

            // 토큰 기한 만료
            if (_authenticatedUser.ExpiresIn < DateTime.UtcNow)
                return _isAuthenticated;

            var oAuth = OAuthProviderFactory.CreateProvider(_authenticatedUser.SNSProvider);
            var authUser = await oAuth.GetUserInfoAsync(_authenticatedUser.Token);
            authUser.ExpiresIn = _authenticatedUser.ExpiresIn;
            _authenticatedUser = authUser;

            // 유저데이터 받아오기 실패
            if (_authenticatedUser == null)
                return _isAuthenticated;

            return _isAuthenticated = true;
        }

        public void Logout()
        {
            _isAuthenticated = false;
            _authenticatedUser = null;
            LocalStorage.Storage.Remove(LocalStorageKey.SavedAuthenticatedUser);
        }
    }
}