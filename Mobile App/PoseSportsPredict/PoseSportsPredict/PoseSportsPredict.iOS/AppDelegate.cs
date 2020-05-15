using Foundation;
using Shiny;
using System;
using UIKit;

namespace PoseSportsPredict.iOS
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
            new Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer();
            Naxam.Controls.Platform.iOS.TopTabbedRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();

            // Initialize Shiny
            this.InitShiny();

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();

            // Initialize extern module
            this.InitExternModule();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
        }

        private void InitExternModule()
        {
            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            XF.Material.iOS.Material.Init();
            ImageCircle.Forms.Plugin.iOS.ImageCircleRenderer.Init();
            Sharpnado.Presentation.Forms.iOS.SharpnadoInitializer.Initialize();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Xam.Plugin.iOS.PopupEffect.Init();
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();
            Syncfusion.SfGauge.XForms.iOS.SfGaugeRenderer.Init();
            Plugin.LocalNotification.NotificationCenter.AskPermission();
        }

        private void InitShiny()
        {
            Shiny.iOSShinyHost.ValidateScopes = false;
            Shiny.iOSShinyHost.Init(new ShinyAppStartup());
        }

        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            Shiny.Jobs.JobManager.OnBackgroundFetch(completionHandler);
        }
    }
}