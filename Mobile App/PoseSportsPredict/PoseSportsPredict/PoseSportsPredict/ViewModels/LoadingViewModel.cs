using PoseCrypto;
using PosePacket.Proxy;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views;
using Shiny;
using System;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

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

            string serverPubKey = await _webApiService.RequestAsync<string>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.GET,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_PUBLISH_KEY,
            });

            if (string.IsNullOrEmpty(serverPubKey))
            {
                await MaterialDialog.Instance.AlertAsync(LocalizeString.Service_Not_Available,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.DefaultAlterDialogConfiguration);
                return;
            }

            CryptoFacade.Instance.RSA_FromXmlString(serverPubKey);
            ClientContext.eSignature = CryptoFacade.Instance.GetEncryptedSignature();
            ClientContext.eSignatureIV = CryptoFacade.Instance.GetEncryptedSignatureIV();

            // Prepare SingletonPage
            ShinyHost.Resolve<AppMasterViewModel>();

            // Notify Init
            await _notificationService.Initialize();

            if (!await _OAuthService.IsAuthenticatedAndValid()
                || !await ShinyHost.Resolve<LoginViewModel>().PoseLogin(false))
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
            INotificationService notificationService) : base(coupledPage)
        {
            _notificationService = notificationService;
            _webApiService = webApiService;
            _OAuthService = OAuthService;

            if (OnInitializeView())
            {
                coupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }
    }
}