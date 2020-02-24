using Acr.UserDialogs;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial
{
	public partial class LoadingPage : ContentPage
	{
		public LoadingPage()
		{
			InitializeComponent();
		}

		public async Task AppLoadAsync()
		{
			await Task.Delay(1).ConfigureAwait(false);

			using (var progress = UserDialogs.Instance.Progress("Please Wait..."))
			{
				var viewLocator = new ViewLocator();
				Singleton.Register(viewLocator);
				viewLocator.Initialize();

				progress.PercentComplete = 100;
			}
		}
	}
}