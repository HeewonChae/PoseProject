namespace Xamarin_Tutorial
{
	using Plugin.LocalNotification;
	using Xamarin.Forms;
	using Xamarin_Tutorial.InfraStructure;
	using Xamarin_Tutorial.Utilities;

	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			NotificationCenter.Current.NotificationTapped += LoadPageFromNotification;

			MainPage = new LoadingPage();
		}

		protected override async void OnStart()
		{
			await ((LoadingPage)MainPage).AppLoadAsync();

			await PageSwitcher.SwitchMainPageAsync(Singleton.Get<ViewLocator>().Login, true);
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}

		private void LoadPageFromNotification(NotificationTappedEventArgs e)
		{
		}
	}
}