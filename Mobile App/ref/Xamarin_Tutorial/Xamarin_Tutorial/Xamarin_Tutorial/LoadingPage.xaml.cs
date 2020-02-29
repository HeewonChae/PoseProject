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
		private IProgressDialog Progress { get; set; }

		public LoadingPage()
		{
			InitializeComponent();
		}

		public async Task AppLoadAsync()
		{
			using (Progress = UserDialogs.Instance.Progress("Please Wait..."))
			{
				// Register Singleton
				Singleton.Register<ViewLocator>();
				Singleton.Register<ExternOAuthService>();
				await UpdateProgress(20);

				// SQLite Config
				SQLiteConfig.Path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				SQLiteConfig.FileName = "xamarin_tutorial4.db3";
				await UpdateProgress(20);

				// Check Extern Auth
				await Singleton.Get<ExternOAuthService>().IsAuthenticatedAndValid();
				await UpdateProgress(30);

				await CompleteProgress();
			}

			Progress = null;
		}

		private async Task UpdateProgress(int delta)
		{
			if (Progress == null)
				return;

			Progress.PercentComplete += delta;
			await Task.Delay(1);
		}

		private async Task CompleteProgress()
		{
			if (Progress == null)
				return;

			Progress.PercentComplete = 100;
			await Task.Delay(1);
		}
	}
}