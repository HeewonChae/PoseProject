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
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels
{
    public class LoadingViewModel : NavigableViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            _isLoaded = false;

            return true;
        }

        public override async void OnAppearing(params object[] datas)
        {
            while (!await WebApiService.CheckInternetConnection())
            { }

            // Table Loader
            TableLoader.Init(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            string serverPubKey = await _webApiService.RequestAsync<string>(new WebRequestContext
            {
                MethodType = WebMethodType.GET,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_PUBLISHKEY,
            });

            if (string.IsNullOrEmpty(serverPubKey))
            {
                await MaterialDialog.Instance.AlertAsync(LocalizeString.Service_Not_Available,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.DefaultAlterDialogConfiguration);
                return;
            }

            _cryptoService.RSA_FromXmlString(serverPubKey);
            ClientContext.eSignature = _cryptoService.GetEncryptedSignature();
            ClientContext.eSignatureIV = _cryptoService.GetEncryptedSignatureIV();

            // Prepare SingletonPage
            ShinyHost.Resolve<AppMasterViewModel>();

            // Notify Init
            await _notificationService.Initialize();

            if (!await _OAuthService.IsAuthenticatedAndValid()
                || !await ShinyHost.Resolve<LoginViewModel>().PoseLogin())
            {
                await _OAuthService.Logout();
                return;
            }

            _isLoaded = true;
            OnPropertyChanged("IsLoaded");
        }

        #endregion BaseViewModel

        #region Services

        private IWebApiService _webApiService;
        private IOAuthService _OAuthService;
        private INotificationService _notificationService;
        private CryptoService _cryptoService;

        #endregion Services

        #region Fields

        public bool _isLoaded;

        #endregion Fields

        #region Properties

        public bool IsLoaded { get => _isLoaded; }

        #endregion Properties

        public LoadingViewModel(
            LoadingPage coupledPage,
            CryptoService cryptoService,
            IWebApiService webApiService,
            IOAuthService OAuthService,
            INotificationService notificationService) : base(coupledPage)
        {
            _notificationService = notificationService;
            _cryptoService = cryptoService;
            _webApiService = webApiService;
            _OAuthService = OAuthService;

            if (OnInitializeView())
            {
                coupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }
    }
}