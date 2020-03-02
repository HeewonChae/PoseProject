using Plugin.LocalNotification;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logic.Utilities;
using PoseSportsPredict.ViewModels;
using Shiny;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = ShinyHost.Resolve<LoadingPage>();
		}

		protected override async void OnStart()
		{
			bool lodingSuccess = false;
			if (MainPage is LoadingPage loadingPage)
			{
				lodingSuccess = await loadingPage.LoadingAsync();
			}

			if (lodingSuccess)
			{
				await PageSwitcher.SwitchMainPageAsync(ShinyHost.Resolve<LoginViewModel>(), true);
			}
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}