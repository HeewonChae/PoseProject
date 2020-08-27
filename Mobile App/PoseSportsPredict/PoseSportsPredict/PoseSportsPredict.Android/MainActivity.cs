﻿using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Plugin.LocalNotification;
using PoseSportsPredict.Resources;
using Shiny;

namespace PoseSportsPredict.Droid
{
    [Activity(Label = "PoseSportsPredict",
        Icon = "@mipmap/icon_round",
        Theme = "@style/LoadingTheme",
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly string[] Permissions =
        {
            Android.Manifest.Permission.Internet,
            Android.Manifest.Permission.AccessNetworkState,
            Android.Manifest.Permission.BindNotificationListenerService,
            Android.Manifest.Permission.AccessNotificationPolicy,
            Android.Manifest.Permission.ReceiveBootCompleted,
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

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            RequestPermissions(Permissions, PermissionReqId);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // Initialize extern module
            this.InitExternModule(savedInstanceState);

            LoadApplication(new App());

            NotificationCenter.NotifyNotificationTapped(this.Intent);
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
            UserDialogs.Init(this);

            NotificationCenter.CreateNotificationChannel(new Plugin.LocalNotification.Platform.Droid.NotificationChannelRequest
            {
                Id = AppConfig.Psoe_Noti_Channel_01,
                Importance = NotificationImportance.High,
                Name = "General",
                Description = "General",
            });
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
    }
}