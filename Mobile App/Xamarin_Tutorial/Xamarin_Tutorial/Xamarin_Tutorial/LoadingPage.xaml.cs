using Acr.UserDialogs;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Services.Authentication;
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
				// Register Singleton
				Singleton.Register<ViewLocator>();
				Singleton.Register<ExternOAuthService>();
				await UpdateProgress(20);

				// SQLite Config
				SQLiteConfig.Path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				SQLiteConfig.FileName = "xamarin_tutorial3.db3";
				await UpdateProgress(10);

				// Check Extern Auth
				await Singleton.Get<ExternOAuthService>().IsAuthenticatedAndValid();
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