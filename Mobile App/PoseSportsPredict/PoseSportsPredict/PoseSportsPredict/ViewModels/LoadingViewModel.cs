using PoseCrypto;
using PosePacket.Proxy;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.Cache;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views;
using PoseSportsPredict.Utilities.LocalStorage;
using Shiny;
using System;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using System.Globalization;
using PoseSportsPredict.Models.Resources.Common;
using System.Linq;
using PoseSportsPredict.Logics.LocalizedRes;

namespace PoseSportsPredict.ViewModels
{
    public class LoadingViewModel : NavigableViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            return true;
        }

        public override async void OnAppearing(params object[] datas)
        {
            IsLoaded = false;

            while (!await WebApiService.CheckInternetConnection()) { }

            // Table Loader
            TableLoader.Init(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            // Check Language
            {
                LocalStorage.Storage.GetValueOrDefault<string>(LocalStorageKey.UserLanguageId, out string userLanguageId);
                if (userLanguageId == null)
                {
                    userLanguageId = CultureInfo.CurrentUICulture.Name.Split('-')[0];
                    if (!CoverageLanguage.CoverageLanguages.ContainsKey(userLanguageId))
                    {
                        // Default Language : EN
                        userLanguageId = AppConfig.DEFAULT_LANGUAGE;
                    }

                    LocalStorage.Storage.AddOrUpdateValue<string>(LocalStorageKey.UserLanguageId, userLanguageId);
                }

                CultureInfo.CurrentCulture = new CultureInfo(userLanguageId);
                CultureInfo.CurrentUICulture = new CultureInfo(userLanguageId);
            }

            // Check Timezone
            {
                LocalStorage.Storage.GetValueOrDefault<string>(LocalStorageKey.UserTimeZoneId, out string userTimeZoneId);

                // Delete ExpiredCachedData
                if (TimeZoneInfo.Local.Id.Equals(userTimeZoneId))
                    await _cacheService.DeleteExpiredCachedDataAsync();
                else
                    await _cacheService.DeleteAllCachedDataAsync();

                LocalStorage.Storage.AddOrUpdateValue<string>(LocalStorageKey.UserTimeZoneId, TimeZoneInfo.Local.Id);
            }

            string serverPubKey = await _webApiService.RequestAsync<string>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.GET,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_PUBLISH_KEY,
            });

            if (!string.IsNullOrEmpty(serverPubKey))
            {
                CryptoFacade.Instance.RSA_FromXmlString(serverPubKey);
                ClientContext.eSignature = CryptoFacade.Instance.GetEncryptedSignature();
                ClientContext.eSignatureIV = CryptoFacade.Instance.GetEncryptedSignatureIV();
            }

            // Prepare SingletonPage
            ShinyHost.Resolve<AppMasterViewModel>();

            // Notify Init
            await _notificationService.Initialize();

            if (!await _OAuthService.IsAuthenticatedAndValid()
                || !await ShinyHost.Resolve<LoginViewModel>().PoseLogin())
            {
                await _OAuthService.Logout();
            }

            IsLoaded = true;
            OnPropertyChanged("IsLoaded");
            MessagingCenter.Send(this, "LoadingComplete");
        }

        #endregion BaseViewModel

        #region Services

        private IWebApiService _webApiService;
        private IOAuthService _OAuthService;
        private INotificationService _notificationService;
        private ICacheService _cacheService;

        #endregion Services

        #region Fields

        public bool _isLoaded;

        #endregion Fields

        #region Properties

        public bool IsLoaded { get => _isLoaded; set => SetValue(ref _isLoaded, value); }

        #endregion Properties

        public LoadingViewModel(
            LoadingPage coupledPage,
            IWebApiService webApiService,
            IOAuthService OAuthService,
            INotificationService notificationService,
            ICacheService cacheService) : base(coupledPage)
        {
            _notificationService = notificationService;
            _webApiService = webApiService;
            _OAuthService = OAuthService;
            _cacheService = cacheService;

            if (OnInitializeView())
            {
                coupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }
    }
}