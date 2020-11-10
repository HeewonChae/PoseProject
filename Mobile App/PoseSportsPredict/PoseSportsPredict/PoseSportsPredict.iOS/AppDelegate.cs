using Foundation;
using Google.MobileAds;
using Shiny;
using System;
using System.IO;
using System.Threading.Tasks;
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

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();

            // Initialize extern module
            this.InitExternModule();

            DisplayCrashReport();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            //Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
        }

        private void InitExternModule()
        {
            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            XF.Material.iOS.Material.Init();
            Sharpnado.Presentation.Forms.iOS.SharpnadoInitializer.Initialize();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Xam.Plugin.iOS.PopupEffect.Init();
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();
            Syncfusion.SfGauge.XForms.iOS.SfGaugeRenderer.Init();
            //Plugin.LocalNotification.NotificationCenter.AskPermission();
            MobileAds.SharedInstance.Start(CompletionHandler);
        }

        private void CompletionHandler(InitializationStatus status)
        {
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
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        private void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            var alertView = new UIAlertView("Crash Report", errorText, null, "Close", "Clear") { UserInteractionEnabled = true };
            alertView.Clicked += (sender, args) =>
            {
                if (args.ButtonIndex != 0)
                {
                    File.Delete(errorFilePath);
                }
            };
            alertView.Show();
        }
    }
}