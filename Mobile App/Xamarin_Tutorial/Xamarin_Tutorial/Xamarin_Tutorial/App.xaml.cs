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
			//		break;

			//	case TargetIdiom.Phone:
			//		break;

			//	default:
			//		break;
			//}
		}

		protected override async void OnStart()
		{
			var mainViewModel = Singleton.Get<MainViewModel>();
			mainViewModel.Initialize();
			MainPage = await mainViewModel.Login.ShowViewPage();
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
