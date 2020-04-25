using Microsoft.Extensions.DependencyInjection;
using PoseCrypto;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels;
using PoseSportsPredict.ViewModels.Common;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.ViewModels.Football.Bookmark;
using PoseSportsPredict.ViewModels.Football.League;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Match;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.ViewModels.Football.Standings;
using PoseSportsPredict.ViewModels.Football.Team;
using PoseSportsPredict.Views;
using PoseSportsPredict.Views.Common;
using PoseSportsPredict.Views.Football;
using PoseSportsPredict.Views.Football.Bookmark;
using PoseSportsPredict.Views.Football.League;
using PoseSportsPredict.Views.Football.League.Detail;
using PoseSportsPredict.Views.Football.Match;
using PoseSportsPredict.Views.Football.Match.Detail;
using PoseSportsPredict.Views.Football.Team;
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

            RegisterComparer(services);
            RegisterServices(services);
            RegisterConverters(services);
            MatchViewModels(services);
        }

        private void RegisterComparer(IServiceCollection services)
        {
            services.AddSingleton<StoredData_BasicComparer>();
            services.AddSingleton<StoredData_InverseDateComparer>();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IWebApiService, WebApiService>();
            services.AddSingleton<ISQLiteService, SQLiteService>();
            services.AddSingleton<IOAuthService, ExternOAuthService>();
            services.AddSingleton<IBookmarkService, BookmarkService>();
            services.AddSingleton<INotificationService, NotificationService>();
        }

        private void RegisterConverters(IServiceCollection services)
        {
            services.AddSingleton<FixtureDetailToMatchInfoConverter>();
            services.AddSingleton<CoverageLeagueToLeagueInfoConverter>();
            services.AddSingleton<MatchInfoToLeagueInfoConverter>();
            services.AddSingleton<MatchInfoToTeamInfoConverter>();
            services.AddSingleton<FootballMatchStatisticsConverter>();
            services.AddSingleton<FixtureDetailToLastFormConverter>();
            services.AddSingleton<StandingsDetailToStandingsInfo>();
        }

        private void MatchViewModels(IServiceCollection services)
        {
            services.AddSingleton<LoadingPage>();
            services.AddSingleton<LoadingViewModel>();

            // Login
            services.AddSingleton<LoginPage>();
            services.AddSingleton<LoginViewModel>();

            // MasterPage
            services.AddSingleton<AppMasterPage>();
            services.AddSingleton<AppMasterViewModel>();
            services.AddSingleton<AppMasterMenuPage>();
            services.AddSingleton<AppMasterMenuViewModel>();
            services.AddSingleton<BookmarkMenuListViewModel>();

            // Common - Settings
            services.AddSingleton<SettingsPage>();
            services.AddSingleton<SettingsViewModel>();

            // Football
            services.AddSingleton<FootballMainPage>();
            services.AddSingleton<FootballMainViewModel>();
            // Football - Match
            services.AddTransient<FootballMatchesTabViewModel>();
            services.AddTransient<FootballMatchesPage>();
            services.AddTransient<FootballMatchesViewModel>();
            services.AddTransient<FootballMatchListViewModel>();
            services.AddTransient<FootballMatchDetailPage>();
            services.AddTransient<FootballMatchDetailViewModel>();

            services.AddTransient<FootballMatchDetailOverview>();
            services.AddTransient<FootballMatchDetailOverviewModel>();
            services.AddTransient<FootballMatchDetailH2HView>();
            services.AddTransient<FootballMatchDetailH2HViewModel>();
            services.AddTransient<FootballMatchDetailPredictionsView>();
            services.AddTransient<FootballMatchDetailPredictionsViewModel>();
            services.AddTransient<FootballMatchDetailOddsView>();
            services.AddTransient<FootballMatchDetailOddsViewModel>();
            // Football - League
            services.AddTransient<FootballLeaguesPage>();
            services.AddTransient<FootballLeaguesViewModel>();
            services.AddTransient<FootballLeagueListViewModel>();
            services.AddTransient<FootballLeagueDetailPage>();
            services.AddTransient<FootballLeagueDetailViewModel>();
            //Football - Team
            services.AddTransient<FootballTeamDetailPage>();
            services.AddTransient<FootballTeamDetailViewModel>();
            // Football - Bookmark
            services.AddTransient<FootballBookmarksTabViewModel>();
            services.AddTransient<FootballBookmarkMatchesPage>();
            services.AddTransient<FootballBookmarkMatchesViewModel>();
            services.AddTransient<FootballBookmarkLeaguesPage>();
            services.AddTransient<FootballBookmarkLeaguesViewModel>();
            services.AddTransient<FootballBookmarkTeamsPage>();
            services.AddTransient<FootballBookmarkTeamsViewModel>();
            services.AddSingleton<FootballBookmarkSearchPage>();
            services.AddSingleton<FootballBookmarkSearchViewModel>();

            // Football - Standings
            services.AddTransient<FootballStandingsViewModel>();
        }
    }
}