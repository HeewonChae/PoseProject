namespace Xamarin_Tutorial
{
	using Xamarin.Forms;
	using Xamarin_Tutorial.InfraStructure;
	using Xamarin_Tutorial.Utilities;

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

			PageSwitcher.SwitchMainPageAsync(Singleton.Get<ViewLocator>().Login, isNavPage: true);
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}