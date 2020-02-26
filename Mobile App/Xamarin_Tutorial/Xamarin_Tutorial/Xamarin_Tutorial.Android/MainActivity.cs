using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using FFImageLoading.Forms.Platform;

namespace Xamarin_Tutorial.Droid
{
	[Activity(Label = "Xamarin_Tutorial", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		private bool doubleBackToExitPressedOnce = false;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);

			CachedImageRenderer.Init(true);
			UserDialogs.Init(this);

			LoadApplication(new App());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		public override void OnBackPressed()
		{
			if (doubleBackToExitPressedOnce)
			{
				base.OnBackPressed();
				Java.Lang.JavaSystem.Exit(0);
				return;
			}

			this.doubleBackToExitPressedOnce = true;
			string message = GetString(Resource.String.PressBackTwiceToExit);
			Toast.MakeText(this, message, ToastLength.Short).Show();

			new Handler().PostDelayed(() =>
			{
				doubleBackToExitPressedOnce = false;
			}, 2000);
		}
	}
}