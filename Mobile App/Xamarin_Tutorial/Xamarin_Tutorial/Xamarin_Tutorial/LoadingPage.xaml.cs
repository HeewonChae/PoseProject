using Acr.UserDialogs;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Utilities;

namespace Xamarin_Tutorial
{
	public partial class LoadingPage : ContentPage
	{
		public LoadingPage()
		{
			InitializeComponent();
		}

		private IProgressDialog progress;

		public async Task AppLoadAsync()
		{
			using (progress = UserDialogs.Instance.Progress("Please Wait..."))
			{
				var viewLocator = new ViewLocator();
				await UpdateProgress(30);

				Singleton.Register(viewLocator);
				await UpdateProgress(30);

				viewLocator.Initialize();
				await UpdateProgress(30);

				await CompleteProgress();
			}

			progress = null;
		}

		private async Task UpdateProgress(int delta)
		{
			if (progress == null)
				return;

			progress.PercentComplete += delta;
			await Task.Delay(1);
		}

		private async Task CompleteProgress()
		{
			if (progress == null)
				return;

			progress.PercentComplete = 100;
			await Task.Delay(1);
		}
	}
}