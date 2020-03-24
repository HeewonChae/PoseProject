using PosePacket.Proxy;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels;
using Shiny;
using System;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        #region Services

        private IWebApiService _webApiService;
        private IOAuthService _OAuthService;
        private CryptoService _cryptoService;

        #endregion Services

        #region Constructors

        public LoadingPage(
            CryptoService cryptoService,
            IWebApiService webApiService,
            IOAuthService OAuthService)
        {
            InitializeComponent();

            _cryptoService = cryptoService;
            _webApiService = webApiService;
            _OAuthService = OAuthService;
            BindingContext = this;
        }

        #endregion Constructors

        #region Methods

        public async Task LoadingAsync()
        {
            while (!await WebApiService.CheckInternetConnection())
            { }

            // Table Loader
            await TableLoader.Init(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            string serverPubKey = await _webApiService.RequestAsync<string>(new WebRequestContext
            {
                MethodType = WebMethodType.GET,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_PUBLISHKEY,
            });

            if (string.IsNullOrEmpty(serverPubKey))
            {
                await MaterialDialog.Instance.AlertAsync(LocalizeString.Service_Not_Available);
                return;
            }

            _cryptoService.RSA_FromXmlString(serverPubKey);
            ClientContext.eSignature = _cryptoService.GetEncryptedSignature();
            ClientContext.eSignatureIV = _cryptoService.GetEncryptedSignatureIV();

            if (!await _OAuthService.IsAuthenticatedAndValid()
                || !await ShinyHost.Resolve<LoginViewModel>().PoseLogin())
            {
                await _OAuthService.Logout();
                return;
            }
        }

        #endregion Methods
    }
}