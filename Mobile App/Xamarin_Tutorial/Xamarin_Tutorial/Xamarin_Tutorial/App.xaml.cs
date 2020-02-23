namespace Xamarin_Tutorial
{
	using Xamarin.Forms;
	using Xamarin_Tutorial.InfraStructure;

	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
			MainPage = new LoadingPage();
		}

		protected override async void OnStart()
		{
			await ((LoadingPage)MainPage).AppLoadAsync();

			await PageSwitcher.SwitchMainPageAsync(Singleton.Get<ViewLocator>().Login);
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}