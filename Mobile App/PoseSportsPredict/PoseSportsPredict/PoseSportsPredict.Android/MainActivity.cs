using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Plugin.LocalNotification;
using PoseSportsPredict.Resources;
using Shiny;
using System.Linq;

namespace PoseSportsPredict.Droid
{
    [Activity(Label = "PoseSportsPredict",
        Icon = "@mipmap/icon_round",
        Theme = "@style/MyTheme.Splash",
        MainLauncher = true,
        //ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private bool doubleBackToExitPressedOnce = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //base.Window.RequestFeature(WindowFeatures.ActionBar);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Xam.Plugin.Droid.PopupEffect.Init();

            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            //Initialize shiny
            this.InitShiny();

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            XF.Material.Droid.Material.Init(this, savedInstanceState);

            // Initialize extern module
            this.InitExternModule(savedInstanceState);

            LoadApplication(new App());

            NotificationCenter.NotifyNotificationTapped(this.Intent);
        }

        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
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
            UserDialogs.Init(this);
        }

        private void InitShiny()
        {
            Shiny.AndroidShinyHost.Init(this.Application, new ShinyAppStartup());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            AndroidShinyHost.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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
                    Java.Lang.JavaSystem.Exit(0);
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