using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Shiny;
using Plugin.LocalNotification;
using Android.Content;

namespace PoseSportsPredict.Droid
{
    [Activity(Label = "PoseSportsPredict",
        Icon = "@mipmap/icon_round",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //Initialize shiny
            this.InitShiny();

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            XF.Material.Droid.Material.Init(this, savedInstanceState);

            // Initialize extern module
            this.InitExternModule(savedInstanceState);

            LoadApplication(new App());

            NotificationCenter.NotifyNotificationTapped(Intent);
        }

        private void InitExternModule(Bundle savedInstanceState)
        {
            Android.Glide.Forms.Init(this, debug: false);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            NotificationCenter.CreateNotificationChannel(new Plugin.LocalNotification.Platform.Droid.NotificationChannelRequest());
            Xamarin.KeyboardHelper.Platform.Droid.Effects.Init(this);
            Sharpnado.Presentation.Forms.Droid.SharpnadoInitializer.Initialize();
            ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
        }

        private void InitShiny()
        {
            Shiny.AndroidShinyHost.ValidateScopes = false;
            Shiny.AndroidShinyHost.Init(this.Application, new ShinyAppStartup());
        }

        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            AndroidShinyHost.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}