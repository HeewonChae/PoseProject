using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Utilities.ExternOAuth.Providers;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.ViewModels;
using Shiny;
using System;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Auth;
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
                        _authenticatedUser = await _webApiService.EncryptRequestAsync<ExternAuthUser>(new WebRequestContext
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

                        if (_authenticatedUser != null)
                        {
                            // PoseLogin
                            if (!await ShinyHost.Resolve<LoginViewModel>().PoseLogin())
                            {
                                _authenticatedUser = null;
                            }
                            else // Login success
                            {
                                LocalStorage.Storage.AddOrUpdateValue(LocalStorageKey.SavedAuthenticatedUser, _authenticatedUser);
                                _isAuthenticated = true;
                            }
                        }
                    }
                };

                // Error
                authenticator.Error += async (sender, eventArgs) =>
                {
                    await MaterialDialog.Instance.AlertAsync($"OAuth error: {eventArgs.Message}");
                };

                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(authenticator);

                // 로그인 폼 닫힘
                presenter.Completed += (sender, eventArgs) =>
                {
                    ShinyHost.Resolve<LoginViewModel>().SetIsBusy(false);
                };
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync($"OAuth exception: {ex.Message}");
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
            // 유저데이터 받아오기 실패
            if (authUser == null)
                return _isAuthenticated;

            authUser.ExpiresIn = _authenticatedUser.ExpiresIn;
            _authenticatedUser = authUser;

            return _isAuthenticated = true;
        }

        public async Task Logout()
        {
            _isAuthenticated = false;
            _authenticatedUser = null;
            LocalStorage.Storage.Remove(LocalStorageKey.SavedAuthenticatedUser);

            await PageSwitcher.SwitchMainPageAsync(ShinyHost.Resolve<LoginViewModel>());
        }
    }
}