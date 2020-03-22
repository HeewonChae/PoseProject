using Microsoft.Extensions.DependencyInjection;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels;
using PoseSportsPredict.ViewModels.Common;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.ViewModels.Football.Bookmark;
using PoseSportsPredict.ViewModels.Football.Match;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.Views;
using PoseSportsPredict.Views.Common;
using PoseSportsPredict.Views.Football;
using PoseSportsPredict.Views.Football.Bookmark;
using PoseSportsPredict.Views.Football.League;
using PoseSportsPredict.Views.Football.Match;
using PoseSportsPredict.Views.Football.Match.Detail;
using Shiny;
using System;
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
            RegisterConverters(services);
            MatchViewModels(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<CryptoService>();
            services.AddSingleton<IWebApiService, WebApiService>();
            services.AddSingleton<ISQLiteService, SQLiteService>();
            services.AddSingleton<IOAuthService, ExternOAuthService>();
        }

        private void RegisterConverters(IServiceCollection services)
        {
            services.AddSingleton<FixtureDetailToMatchInfoConverter>();
            services.AddSingleton<CoverageLeagueToLeagueInfoConverter>();
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

            // Settings
            services.AddSingleton<SettingsPage>();
            services.AddSingleton<SettingsViewModel>();

            // Football
            services.AddSingleton<FootballMainPage>();
            services.AddSingleton<FootballMainViewModel>();
            services.AddSingleton<FootballMatchesTabViewModel>();
            services.AddTransient<FootballMatchesPage>();
            services.AddTransient<FootballMatchesViewModel>();
            services.AddTransient<FootballMatchDetailPage>();
            services.AddTransient<FootballMatchDetailViewModel>();
            services.AddTransient<FootballMatchDetailOverviewModel>();
            services.AddTransient<FootballMatchDetailH2HViewModel>();
            services.AddTransient<FootballMatchDetailPredictionsViewModel>();
            services.AddTransient<FootballMatchDetailOddsViewModel>();

            services.AddSingleton<FootballLeaguesPage>();
            services.AddSingleton<FootballLeaguesViewModel>();

            services.AddSingleton<FootballBookmarksPage>();
            services.AddSingleton<FootballBookmarksViewModel>();
        }
    }
}