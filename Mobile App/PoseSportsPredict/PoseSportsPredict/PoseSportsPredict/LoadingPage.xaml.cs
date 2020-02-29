using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingPage : ContentPage
	{
		#region Members

		private bool _isLoaded;
		private IOAuthService _OAuthService;

		#endregion Members

		#region Constructors

		public LoadingPage(IOAuthService OAuthService)
		{
			InitializeComponent();

			_OAuthService = OAuthService;
			BindingContext = this;
		}

		#endregion Constructors

		#region Methods

		public async Task LoadingAsync()
		{
			if (_isLoaded)
				return;

			await _OAuthService.IsAuthenticatedAndValid();

			_isLoaded = true;
			return;
		}

		#endregion Methods
	}
}