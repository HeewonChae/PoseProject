using Plugin.LocalNotification;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Services;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using Shiny;
using Xamarin.Forms;

namespace PoseSportsPredict
{
    public partial class App : Application
    {
        // Localize String Check
        //var culutreName = CultureInfo.CurrentUICulture.Name;
        //var str = LocalizeString.Check_Internet_Connection;
        //CultureInfo.CurrentUICulture = new CultureInfo("en");
        //culutreName = CultureInfo.CurrentUICulture.Name;
        //str = LocalizeString.Check_Internet_Connection;

        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);

            // Local Notification tap event listener
            NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;

            MainPage = ShinyHost.Resolve<LoadingPage>();
        }

        protected override async void OnStart()
        {
            if (MainPage is LoadingPage loadingPage)
                await loadingPage.LoadingAsync();
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
            if (Application.Current.MainPage is MasterDetailPage)
            {
                await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>()
                , e.Data.JsonDeserialize<FootballMatchInfo>());
            }
        }
    }
}