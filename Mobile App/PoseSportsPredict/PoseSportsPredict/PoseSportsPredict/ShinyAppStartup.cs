using Microsoft.Extensions.DependencyInjection;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.ViewModels.Test;
using PoseSportsPredict.Views;
using PoseSportsPredict.Views.Football;
using PoseSportsPredict.Views.Test;
using Shiny;
using WebServiceShare.ExternAuthentication;

namespace PoseSportsPredict
{
    public class ShinyAppStartup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // this is where you'll load things like BLE, GPS, etc - those are covered in other sections
            // things like the jobs, environment, power, are all installed automatically
            RegisterServices(services);
            MatchViewModels(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<CryptoService>();
            services.AddSingleton<IWebApiService, WebApiService>();
            services.AddSingleton<ISQLiteService, SQLiteService>();
            services.AddSingleton<IOAuthService, ExternOAuthService>();
        }

        private void MatchViewModels(IServiceCollection services)
        {
            services.AddSingleton<LoadingPage>();
            services.AddSingleton<LoginPage>();
            services.AddSingleton<LoginViewModel>();

            // MasterPage
            services.AddSingleton<AppMasterPage>();
            services.AddSingleton<AppMasterViewModel>();
            services.AddSingleton<AppMasterMenuPage>();
            services.AddSingleton<AppMasterMenuViewModel>();

            // Football
            services.AddSingleton<FootballMainPage>();
            services.AddSingleton<FootballMainViewModel>();
            services.AddSingleton<FootballMatchesTabViewModel>();
            services.AddSingleton<FootballLeaguesPage>();
            services.AddSingleton<FootballLeaguesViewModel>();
            services.AddTransient<FootballMatchesPage>();
            services.AddTransient<FootballMatchesViewModel>();

            // Test
            services.AddSingleton<AppShellPage>();
            services.AddSingleton<AppShellViewModel>();
            services.AddSingleton<NewItemPage>();
            services.AddSingleton<NewItemViewModel>();
            services.AddTransient<ItemDetailPage>();
            services.AddTransient<ItemDetailViewModel>();
        }
    }
}