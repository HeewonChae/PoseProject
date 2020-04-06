using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
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

        public async Task<bool> PoseLogin(bool showIndicator = true)
        {
            SetIsBusy(true);

            O_Login loginResult = null;

            if (showIndicator)
            {
                using (UserDialogs.Instance.Loading(LocalizeString.Loginning))
                {
                    loginResult = await _webApiService.EncryptRequestAsync<O_Login>(new WebRequestContext
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
                }
            }
            else
            {
                loginResult = await _webApiService.EncryptRequestAsync<O_Login>(new WebRequestContext
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
            }

            if (loginResult == null)
            {
                SetIsBusy(false);
                return false;
            }

            // Update PoseToken, Update ExpireTime
            ClientContext.SetCredentialsFrom(loginResult.PoseToken);
            ClientContext.TokenExpireIn = DateTime.UtcNow.AddMilliseconds(loginResult.TokenExpireIn);
            ClientContext.LastLoginTime = loginResult.LastLoginTime.ToLocalTime();

            await MaterialDialog.Instance.SnackbarAsync(LocalizeString.Welcome);
            await PageSwitcher.SwitchMainPageAsync(ShinyHost.Resolve<AppMasterViewModel>(), true);

            // Setup NotiData
            LocalStorage.Storage.GetValueOrDefault<FootballMatchInfo>(LocalStorageKey.NotifyIntentData, out FootballMatchInfo notiIntentData);
            if (notiIntentData != null)
            {
                LocalStorage.Storage.Remove(LocalStorageKey.NotifyIntentData);
                await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), notiIntentData);
            }

            SetIsBusy(false);
            return true;
        }

        #endregion Methods
    }
}