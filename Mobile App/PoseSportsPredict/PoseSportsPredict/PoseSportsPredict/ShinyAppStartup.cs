using Microsoft.Extensions.DependencyInjection;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Services;
using PoseSportsPredict.Utilities.SQLite;
using PoseSportsPredict.ViewModels;
using PoseSportsPredict.ViewModels.Common;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.ViewModels.Football.Bookmark;
using PoseSportsPredict.ViewModels.Football.League;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Match;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.ViewModels.Football.Match.RecentForm;
using PoseSportsPredict.ViewModels.Football.League.Standings;
using PoseSportsPredict.ViewModels.Football.Team;
using PoseSportsPredict.ViewModels.Football.Team.GoalStatistics;
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
using System.Threading.Tasks;
using Shiny.Jobs;
using System.Threading;
using PoseSportsPredict.Services.Cache;
using PoseSportsPredict.InfraStructure.Cache;
using PoseSportsPredict.ViewModels.Common.Detail;
using PoseSportsPredict.Views.Common.Detail;
using PoseSportsPredict.ViewModels.Football.Match.PredictionPick;

namespace PoseSportsPredict
{
    //public class LocalNotificationJob : Shiny.Jobs.IJob
    //{
    //    private readonly INotificationService _notificationService;

    //    public LocalNotificationJob(INotificationService notificationService)
    //    {
    //        _notificationService = notificationService;
    //    }

    //    public async Task<bool> Run(JobInfo jobInfo, CancellationToken cancelToken)
    //    {
    //        // Notify Init
    //        await _notificationService.Initialize();

    //        return true;
    //    }
    //}

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

            //var notificationjob = new JobInfo(typeof(LocalNotificationJob), "LocalNotificationJob")
            //{
            //    // these are criteria that must be met in order for your job to run
            //    RequiredInternetAccess = InternetAccess.Any,
            //    Repeat = false //defaults to true, set to false to run once OR set it inside a job to cancel further execution
            //};

            //services.RegisterJob(notificationjob);
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
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<InAppBillingService>();
            services.AddSingleton<MembershipService>();
        }

        private void RegisterConverters(IServiceCollection services)
        {
            services.AddSingleton<FixtureDetailToMatchInfo>();
            services.AddSingleton<MatchInfoToFormInfo>();
            services.AddSingleton<FixtureDetailToTeamStatistics>();
            services.AddSingleton<StandingsDetailToStandingsInfo>();
            services.AddSingleton<LeagueDetailToLeagueInfo>();
            services.AddSingleton<TeamDetailToTeamInfo>();
            services.AddSingleton<OddsDetailToOddsInfo>();

            services.AddSingleton<MatchInfoToLeagueInfo>();
            services.AddSingleton<MatchInfoToTeamInfo>();
            services.AddSingleton<MatchInfoToTeamStatistics>();
            services.AddSingleton<MatchInfoToGoalLineChartData>();
            services.AddSingleton<PredictionDetailsToPredictionGroup>();
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

            // Settings
            services.AddSingleton<SettingsPage>();
            services.AddSingleton<SettingsViewModel>();
            // Settings - My Profile
            services.AddSingleton<MyProfilePage>();
            services.AddSingleton<MyProfileViewModel>();
            // Settings - VIP Lounge
            services.AddSingleton<VIPLoungeTabPageViewModel>();
            services.AddTransient<VIPSubscribePage>();
            services.AddTransient<VIPSubscribeViewModel>();
            services.AddTransient<VIPMatchesPage>();
            services.AddTransient<VIPMatchesViewModel>();
            services.AddTransient<VIPHistoryPage>();
            services.AddTransient<VIPHistoryViewModel>();
            // Settings - Check For Updates
            services.AddSingleton<CheckForUpdatesPage>();
            services.AddSingleton<CheckForUpdatesViewModel>();

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

            // Football - Prediction Pupup
            services.AddSingleton<FootballPredictionFinalScorePage>();
            services.AddSingleton<FootballPredictionFinalScoreViewModel>();
            services.AddSingleton<FootballPredictionMatchWinnerPage>();
            services.AddSingleton<FootballPredictionMatchWinnerViewModel>();
            services.AddSingleton<FootballPredictionBothToScorePage>();
            services.AddSingleton<FootballPredictionBothToScoreViewModel>();
            services.AddSingleton<FootballPredictionUnderOverPage>();
            services.AddSingleton<FootballPredictionUnderOverViewModel>();

            // Football - RecentForm
            services.AddTransient<FootballRecentFormViewModel>();

            // Football - PredictionPick
            services.AddTransient<FootballPredictionPickViewModel>();

            // Football - League
            services.AddTransient<FootballLeaguesPage>();
            services.AddTransient<FootballLeaguesViewModel>();
            services.AddTransient<FootballLeagueListViewModel>();
            services.AddTransient<FootballLeagueDetailPage>();
            services.AddTransient<FootballLeagueDetailViewModel>();
            services.AddTransient<FootballLeagueDetailOverview>();
            services.AddTransient<FootballLeagueDetailOverviewModel>();
            services.AddTransient<FootballLeagueDetailFinishedMatchesView>();
            services.AddTransient<FootballLeagueDetailFinishedMatchesViewModel>();
            services.AddTransient<FootballLeagueDetailScheduledMatchesView>();
            services.AddTransient<FootballLeagueDetailScheduledMatchesViewModel>();
            // Football - Standings
            services.AddTransient<FootballStandingsViewModel>();
            //Football - Team
            services.AddTransient<FootballTeamDetailPage>();
            services.AddTransient<FootballTeamDetailViewModel>();
            services.AddTransient<FootballTeamDetailOverview>();
            services.AddTransient<FootballTeamDetailOverviewModel>();
            services.AddTransient<FootballTeamDetailFinishedMatchesView>();
            services.AddTransient<FootballTeamDetailFinishedMatchesViewModel>();
            services.AddTransient<FootballTeamDetailScheduledMatchesView>();
            services.AddTransient<FootballTeamDetailScheduledMatchesViewModel>();
            // Football - GoalStatistics
            services.AddTransient<FootballTeamGoalStatisticsViewModel>();
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
        }
    }
}