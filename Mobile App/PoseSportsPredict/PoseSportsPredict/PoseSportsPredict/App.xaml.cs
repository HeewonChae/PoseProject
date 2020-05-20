using Plugin.LocalNotification;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Services;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.ViewModels;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using Shiny;
using System.Globalization;
using Xamarin.Forms;
using XF.Material.Forms.Resources;
using XF.Material.Forms.Resources.Typography;

namespace PoseSportsPredict
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            XF.Material.Forms.Material.Init(this);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(AppConfig.SyncfusionKey);

            // Local Notification tap event listener
            NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;

            MainPage = ShinyHost.Resolve<LoadingViewModel>().CoupledPage;
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

        private async void OnLocalNotificationTapped(NotificationTappedEventArgs e)
        {
            // your code goes here
            var param = e.Data.JsonDeserialize<FootballMatchInfo>();
            if (Application.Current.MainPage is NavigationPage
                && param != null)
            {
                await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), param);
            }
            else if (param != null)
            {
                LocalStorage.Storage.AddOrUpdateValue<FootballMatchInfo>(LocalStorageKey.NotifyIntentData, param);
            }
        }
    }
}