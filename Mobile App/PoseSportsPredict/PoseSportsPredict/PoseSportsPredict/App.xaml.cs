using PoseSportsPredict.Services;
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

            // DependencyService.Register<MockDataStore>();
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
    }
}