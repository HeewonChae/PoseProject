using Acr.UserDialogs;
using PosePacket.Proxy;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Common;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels;
using Shiny;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        #region Fields

        private bool _isLoaded;

        #endregion Fields

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

        public async Task<bool> LoadingAsync()
        {
            if (_isLoaded)
                return _isLoaded;

            while (!await WebApiService.CheckInternetConnection())
            { }

            string serverPubKey = await _webApiService.RequestAsync<string>(new WebRequestContext
            {
                MethodType = WebMethodType.GET,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_PUBLISHKEY,
            },
            false);

            if (string.IsNullOrEmpty(serverPubKey))
            {
                await UserDialogs.Instance.AlertAsync(LocalizeString.Service_Not_Available);
                return _isLoaded;
            }

            _cryptoService.RSA_FromXmlString(serverPubKey);
            ClientContext.eSignature = _cryptoService.GetEncryptedSignature();
            ClientContext.eSignatureIV = _cryptoService.GetEncryptedSignatureIV();

            if (!await _OAuthService.IsAuthenticatedAndValid())
            {
                await _OAuthService.Logout();
                return _isLoaded;
            }

            await PageSwitcher.SwitchMainPageAsync(ShinyHost.Resolve<AppMasterViewModel>());
            _isLoaded = true;

            return _isLoaded;
        }

        #endregion Methods
    }
}