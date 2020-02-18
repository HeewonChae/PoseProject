using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin_Tutorial
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			//MainPage = new MainPage();
			switch (Device.Idiom)
			{
				case TargetIdiom.Desktop:
				case TargetIdiom.Tablet:
					MainPage = new MainPage_UWP();
					break;

				case TargetIdiom.Phone:
					MainPage = new MainPage_Mobile();
					break;

				default:
					MainPage = new MainPage_Mobile();
					break;
			}
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
