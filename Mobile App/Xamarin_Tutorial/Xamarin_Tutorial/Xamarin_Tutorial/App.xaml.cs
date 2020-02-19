using System;
using Xamarin.Forms.Xaml;

namespace Xamarin_Tutorial
{
	using Xamarin.Forms;
    using Xamarin_Tutorial.InfraStructure;
    using Xamarin_Tutorial.ViewMdels;

    public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			//switch (Device.Idiom)
			//{
			//	case TargetIdiom.Desktop:
			//	case TargetIdiom.Tablet:
			//		MainPage = new MainPage_UWP();
			//		break;

			//	case TargetIdiom.Phone:
			//		MainPage = new MainPage_Mobile();
			//		break;

			//	default:
			//		MainPage = new MainPage_Mobile();
			//		break;
			//}
		}

		protected override async void OnStart()
		{
			MainPage = new NavigationPage(
				await Singleton.Get<MainViewModel>().Login.GetViewPage());
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
