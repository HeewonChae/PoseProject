using Acr.UserDialogs;
using FFImageLoading.Forms.Platform;
using Foundation;
using UIKit;
using UserNotifications;

namespace Xamarin_Tutorial.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
			//Shiny.iOSShinyHost.Init(new XamarinShinyStartup());

			CachedImageRenderer.Init();

			// for local notification
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				// Ask the user for permission to get notifications on iOS 10.0+
				UNUserNotificationCenter.Current.RequestAuthorization(
						UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
						(approved, error) => { });
			}
			else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				// Ask the user for permission to get notifications on iOS 8.0+
				var settings = UIUserNotificationSettings.GetSettingsForTypes(
						UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
						new NSSet());

				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			}

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

		public override void WillEnterForeground(UIApplication uiApplication)
		{
			Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
		}
	}
}