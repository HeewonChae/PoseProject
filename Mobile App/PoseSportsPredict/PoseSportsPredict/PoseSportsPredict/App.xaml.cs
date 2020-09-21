using MarcTron.Plugin;
using PoseSportsPredict.Logics;
using PoseSportsPredict.ViewModels;
using Shiny;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using Xamarin.Forms;

namespace PoseSportsPredict
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            XF.Material.Forms.Material.Init(this);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(AppConfig.SyncfusionKey);

            CrossMTAdmob.Current.UserPersonalizedAds = false; // 사용자 맞춤광고
            CrossMTAdmob.Current.AdsId = AppConfig.ADMOB_BANNER_ADS_ID;

            // Local Notification tap event listener
            // NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;

            MainPage = ShinyHost.Resolve<LoadingViewModel>().CoupledPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override async void OnResume()
        {
            if (ClientContext.eCredentials.Length > 0)
            {
                await PageUriLinker.GoUrlLinkedPage();
            }
        }

        //private async void OnLocalNotificationTapped(NotificationTappedEventArgs e)
        //{
        //    // your code goes here
        //    var param = e.Data.JsonDeserialize<FootballMatchInfo>();
        //    if (Application.Current.MainPage is NavigationPage
        //        && param != null)
        //    {
        //        await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), param);
        //    }
        //    else if (param != null)
        //    {
        //        LocalStorage.Storage.AddOrUpdateValue<FootballMatchInfo>(LocalStorageKey.NotifyIntentData, param);
        //    }
        //}
    }
}