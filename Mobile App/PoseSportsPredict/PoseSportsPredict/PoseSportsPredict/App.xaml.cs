using PoseSportsPredict.Logics;
using PoseSportsPredict.ViewModels;
using Shiny;
using Xamarin.Forms;

namespace PoseSportsPredict
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = ShinyHost.Resolve<LoadingPage>();
        }

        protected override async void OnStart()
        {
            if (MainPage is LoadingPage loadingPage)
            {
                if (await loadingPage.LoadingAsync())
                    await PageSwitcher.SwitchMainPageAsync(ShinyHost.Resolve<LoginViewModel>(), true);
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}