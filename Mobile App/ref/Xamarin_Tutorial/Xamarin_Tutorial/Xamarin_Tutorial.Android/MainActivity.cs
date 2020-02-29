using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using FFImageLoading.Forms.Platform;
using Plugin.LocalNotification;

namespace Xamarin_Tutorial.Droid
{
	// @style/MyTheme.Splash @style/MainTheme

	[Activity(Label = "Xamarin_Tutorial",
		Icon = "@mipmap/icon",
		Theme = "@style/MainTheme.Base",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		ScreenOrientation = ScreenOrientation.Portrait)]
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

			// Must create a Notification Channel when API >= 26
			// you can created multiple Notification Channels with different names.
			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				NotificationCenter.CreateNotificationChannel(new Plugin.LocalNotification.Platform.Droid.NotificationChannelRequest
				{
				});
			}

			LoadApplication(new App());

			NotificationCenter.NotifyNotificationTapped(Intent);

			//SetTheme(Resource.Style.MainTheme);
		}

		protected override void OnNewIntent(Intent intent)
		{
			NotificationCenter.NotifyNotificationTapped(intent);
			base.OnNewIntent(intent);
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