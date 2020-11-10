using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Firebase;
using Google.Android.Vending.Licensing;
using Plugin.InAppBilling;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using Shiny;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppLinks;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.Droid
{
    [Activity(Label = "@string/App_Title",
        Icon = "@mipmap/icon",
        RoundIcon = "@mipmap/icon_round",
        Theme = "@style/LoadingTheme",
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ILicenseCheckerCallback
    {
        private const string ApiKey =
            "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsjE7ifeef2FeH4CWxxdiUD51iOMUcfyaIgoniFlJeeyN+ju1rPtSkG9jt" +
            "/ffB2jpZYl/L5Km6N2J9WvByg37ge4Z4xkVKXdEFqBawqQLX4AcTv+4hiQ95YIkZSeZwjmryxqe4lTL8M6OPkkeNh3TTEWcPS" +
            "04B/zGdT9+yLiEIDab9AiEURwFnbEsbdqxc9dZ7j21DKhNBibEQd5SELRux7Z0fxtE3LceTQbEOkp1Hjm4UTHa86lmLN4R3m7g6" +
            "hPAPv5n+GawMg04ZYti41mvtarvOaTZWGP5pOTLynFiIKaMxqpjwg5KsDAHxe2wT1JH5rwAlykO/S25+iN7cxxA1wIDAQAB";

        private readonly string[] Permissions =
        {
            Android.Manifest.Permission.Internet,
            Android.Manifest.Permission.AccessNetworkState,
            Android.Manifest.Permission.BindNotificationListenerService,
            Android.Manifest.Permission.AccessNotificationPolicy,
        };

        private int PermissionReqId = 0;

        private bool doubleBackToExitPressedOnce = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Xam.Plugin.Droid.PopupEffect.Init();

            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            MobileAds.Initialize(ApplicationContext, "ca-app-pub-3381862928005780~9770007298");

            //Initialize shiny
            this.InitShiny();

            // Global exception handler
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            RequestPermissions(Permissions, PermissionReqId);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            DisplayCrashReport();

            // Initialize extern module
            this.InitExternModule(savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;

            CheckLVL();

            //NotificationCenter.NotifyNotificationTapped(this.Intent);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 99) // google oauth
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if (result.IsSuccess)
                {
                    GoogleSignInAccount account = result.SignInAccount;

                    var properties = new Dictionary<string, string>();
                    properties.Add("access_token", account.IdToken);

                    ShinyHost.Resolve<IOAuthService>().OnOAuthComplete(null,
                        new AuthenticatorCompletedEventArgs(new Account(account.DisplayName, properties)));
                }
                else
                {
                    if (result.Status.StatusCode == 12501)
                        ShinyHost.Resolve<IOAuthService>().OnOAuthError(null, null);
                    else
                        ShinyHost.Resolve<IOAuthService>().OnOAuthError(null, new AuthenticatorErrorEventArgs($"Google OAuth ErrorCode: {result.Status.StatusCode}"));
                }
            }
            else
            {
                InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
            }
        }

        //protected override void OnNewIntent(Intent intent)
        //{
        //    NotificationCenter.NotifyNotificationTapped(intent);
        //    base.OnNewIntent(intent);
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            AndroidShinyHost.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void InitShiny()
        {
            Shiny.AndroidShinyHost.ValidateScopes = false;
            Shiny.AndroidShinyHost.Init(this.Application, new ShinyAppStartup());
        }

        private void InitExternModule(Bundle savedInstanceState)
        {
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            Android.Glide.Forms.Init(this, debug: false);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            global::Xamarin.Auth.CustomTabsConfiguration.CustomTabsClosingMessage = null;
            //global::Xamarin.Auth.WebViewConfiguration.Android.UserAgent = "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Mobile Safari/537.36";
            Sharpnado.Presentation.Forms.Droid.SharpnadoInitializer.Initialize();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            FirebaseApp.InitializeApp(this);
            AndroidAppLinks.Init(this);
            UserDialogs.Init(this);

            //NotificationCenter.CreateNotificationChannel(new Plugin.LocalNotification.Platform.Droid.NotificationChannelRequest
            //{
            //    Id = AppConfig.Psoe_Noti_Channel_01,
            //    Importance = NotificationImportance.High,
            //    Name = "General",
            //    Description = "General",
            //});

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
        }

        public async override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed())
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                if (Xamarin.Forms.Application.Current.MainPage.Navigation is Xamarin.Forms.INavigation nav
                    && nav.NavigationStack.Count > 1)
                {
                    await nav.PopAsync();

                    return;
                }

                if (doubleBackToExitPressedOnce)
                {
                    base.OnBackPressed();
                    //Java.Lang.JavaSystem.Exit(0);
                    return;
                }

                this.doubleBackToExitPressedOnce = true;
                string message = LocalizeString.PressBack_Twice_To_Exit;
                Toast.MakeText(this, message, ToastLength.Short).Show();

                new Handler().PostDelayed(() =>
                {
                    doubleBackToExitPressedOnce = false;
                }, 2000);
            }
        }

        private void CheckLVL()
        {
            var salt = new byte[] { 28, 65, 30, 128, 103, 87, 74, 64, 51, 88, 95, 45, 91, 121, 36 };

            // create a app-unique identifer to prevent other apps from decrypting the responses
            var deviceInfoHelper = DependencyService.Resolve<IDeviceInfoHelper>();
            deviceInfoHelper.AppPackageName = this.PackageName;
            deviceInfoHelper.DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            deviceInfoHelper.AppVersionName = this.ApplicationContext.PackageManager.GetPackageInfo(PackageName, 0).VersionName;

            // create the obfuscator that will read and write the saved responses,
            // passing the salt, the package name and the device identifier
            var obfuscator = new AESObfuscator(salt, deviceInfoHelper.AppPackageName, deviceInfoHelper.DeviceId);

            // create the policy, passing a Context and the obfuscator
            var policy = new ServerManagedPolicy(this, obfuscator);

            // create the checker
            var checker = new LicenseChecker(this, policy, ApiKey);

            // start the actual check, passing the callback
            checker.CheckAccess(this);
        }

        public void Allow([GeneratedEnum] PolicyResponse reason)
        {
            var deviceInfoHelper = DependencyService.Resolve<IDeviceInfoHelper>();
            deviceInfoHelper.IsLicensed = true;

            this.RunOnUiThread(() =>
            {
                LoadApplication(new App());
            });
        }

        public void ApplicationError([GeneratedEnum] LicenseCheckerErrorCode errorCode)
        {
            var deviceInfoHelper = DependencyService.Resolve<IDeviceInfoHelper>();
            deviceInfoHelper.IsLicensed = false;
        }

        public void DontAllow([GeneratedEnum] PolicyResponse reason)
        {
            // Play has determined that the app should not be available to the user,
            // either because they haven't paid for it or it is not a valid app

            // However, there may have been a problem when Play tried to connect,
            // so if this is the case, allow the user to try again
            var deviceInfoHelper = DependencyService.Resolve<IDeviceInfoHelper>();
            deviceInfoHelper.IsLicensed = false;

            if (reason == PolicyResponse.Retry)
            {
                // try the check again
                CheckLVL();
            }
        }

        //‬ Error handling
        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                var errorMessage = $"{exception}";
                File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        private void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(this)
                .SetPositiveButton("Clear", (sender, args) =>
                {
                    File.Delete(errorFilePath);
                })
                .SetMessage(errorText)
                .SetTitle("Crash Report")
                .Show();
        }
    }
}