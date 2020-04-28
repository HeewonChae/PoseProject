﻿using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Standings;
using PoseSportsPredict.Views.Football.Match.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballMatchDetailOverviewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            OverviewTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (!OverviewTaskLoaderNotifier.IsNotStarted)
                return;

            OverviewTaskLoaderNotifier.Load(InitOverviewData);
        }

        #endregion BaseViewModel

        #region Services

        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private FootballMatchInfo _matchInfo;
        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _overviewTaskLoaderNotifier;
        private List<FootballMatchInfo> _allForm;
        private FootballMatchStatistics _matchStatistics;
        private ObservableList<FootballLastForm> _homeRecentForm;
        private ObservableList<FootballLastForm> _awayRecentForm;
        private ObservableList<FootballStandingsViewModel> _standingsViewModels;

        #endregion Fields

        #region Properties

        public FootballMatchInfo MatchInfo { get => _matchInfo; set => SetValue(ref _matchInfo, value); }
        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> OverviewTaskLoaderNotifier { get => _overviewTaskLoaderNotifier; set => SetValue(ref _overviewTaskLoaderNotifier, value); }
        public FootballMatchStatistics MatchStatistics { get => _matchStatistics; set => SetValue(ref _matchStatistics, value); }
        public ObservableList<FootballLastForm> HomeRecentForm { get => _homeRecentForm; set => SetValue(ref _homeRecentForm, value); }
        public ObservableList<FootballLastForm> AwayRecentForm { get => _awayRecentForm; set => SetValue(ref _awayRecentForm, value); }
        public ObservableList<FootballStandingsViewModel> StandingsViewModels { get => _standingsViewModels; set => SetValue(ref _standingsViewModels, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectFormComaand { get => new RelayCommand<int>(e => SelectForm(e)); }

        private async void SelectForm(int fixtureId)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var foundMatch = _allForm.Where(elem => elem.Id == fixtureId).FirstOrDefault();

            if (foundMatch != null)
            {
                await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), foundMatch);
            }

            SetIsBusy(false);
        }

        public ICommand LeagueNameClickCommand { get => new RelayCommand<FootballMatchInfo>((e) => LeagueNameClick(e)); }

        private async void LeagueNameClick(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>()
                , ShinyHost.Resolve<MatchInfoToLeagueInfoConverter>().Convert(matchInfo, null, null, null));

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchDetailOverviewModel(
            FootballMatchDetailOverview view,
            IWebApiService webApiService) : base(view)
        {
            _webApiService = webApiService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public FootballMatchDetailOverviewModel SetMatchInfo(FootballMatchInfo matchInfo)
        {
            MatchInfo = matchInfo;
            return this;
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> InitOverviewData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // call server
            var server_result = await _webApiService.RequestAsyncWithToken<O_GET_MATCH_OVERVIEW>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_MATCH_OVERVIEW,
                PostData = new I_GET_MATCH_OVERVIEW
                {
                    FixtureId = _matchInfo.Id,
                }
            });

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            // All Form (별 의미 없음)
            _allForm = new List<FootballMatchInfo>();
            foreach (var fixture in server_result.HomeRecentFixtures)
            {
                _allForm.Add(ShinyHost.Resolve<FixtureDetailToMatchInfoConverter>().Convert(fixture, null, null, null) as FootballMatchInfo);
            }
            foreach (var fixture in server_result.AwayRecentFixtures)
            {
                _allForm.Add(ShinyHost.Resolve<FixtureDetailToMatchInfoConverter>().Convert(fixture, null, null, null) as FootballMatchInfo);
            }

            // 기본정보 (평균 득실점, 회복기간)
            MatchStatistics = ShinyHost.Resolve<FootballMatchStatisticsConverter>()
                .Convert(server_result, null, MatchInfo, null) as FootballMatchStatistics;

            // 최근 결과
            List<FootballLastForm> homeRecentForm = new List<FootballLastForm>();
            int loop = 0;
            foreach (var fixture in server_result.HomeRecentFixtures)
            {
                loop++;

                var lastform = ShinyHost.Resolve<FixtureDetailToLastFormConverter>().Convert(fixture, null, MatchInfo.HomeTeamId, null) as FootballLastForm;

                if (loop == 1)
                    lastform.IsLastMatch = true;

                homeRecentForm.Add(lastform);
            }

            List<FootballLastForm> awayRecentForm = new List<FootballLastForm>();
            loop = 0;
            foreach (var fixture in server_result.AwayRecentFixtures)
            {
                loop++;

                var lastform = ShinyHost.Resolve<FixtureDetailToLastFormConverter>().Convert(fixture, null, MatchInfo.AwayTeamId, null) as FootballLastForm;

                if (loop == 1)
                    lastform.IsLastMatch = true;

                awayRecentForm.Add(lastform);
            }

            HomeRecentForm = new ObservableList<FootballLastForm>(homeRecentForm);
            AwayRecentForm = new ObservableList<FootballLastForm>(awayRecentForm);

            // League Standings Table
            List<FootballStandingsInfo> standingsInfos = new List<FootballStandingsInfo>();
            foreach (var standingsDetail in server_result.StandingsDetails)
            {
                standingsInfos.Add(ShinyHost.Resolve<StandingsDetailToStandingsInfo>()
                    .Convert(standingsDetail, null, null, null) as FootballStandingsInfo);
            }

            // Set RankColor
            var descGroups = standingsInfos.OrderBy(elem => elem.Rank).GroupBy(elem => elem.Description);
            int positiveDescCnt = 0;
            int negativeDescCnt = 0;
            int neutralDescCnt = 0;
            foreach (var descGroup in descGroups)
            {
                if (string.IsNullOrEmpty(descGroup.Key))
                    continue;

                var descCategoryType = StandginsDescription.GetDescCategory(descGroup.Key);
                Color descColor;
                if (descCategoryType == StandingsDescCategoryType.Positive)
                {
                    descColor = StandingsRankColor.GetRankColor(descCategoryType, positiveDescCnt);
                    positiveDescCnt++;
                }
                else if (descCategoryType == StandingsDescCategoryType.Negative)
                {
                    descColor = StandingsRankColor.GetRankColor(descCategoryType, negativeDescCnt);
                    negativeDescCnt++;
                }
                else if (descCategoryType == StandingsDescCategoryType.Neutral)
                {
                    descColor = StandingsRankColor.GetRankColor(descCategoryType, neutralDescCnt);
                    neutralDescCnt++;
                }
                else
                {
                    descColor = StandingsRankColor.GetRankColor(StandingsDescCategoryType.None, 0);
                }

                foreach (var standings in descGroup)
                {
                    standings.RankColor = descColor;
                }
            }

            // Groupping Standings by group data
            var standingsGroups = standingsInfos.OrderBy(elem => elem.Rank).GroupBy(elem => elem.Group);
            var standingsViewModels = new List<FootballStandingsViewModel>();
            foreach (var standingsGroup in standingsGroups)
            {
                var standings = standingsGroup.ToArray();
                if (standings.Length >= 2)
                {
                    var standingsViewModel = ShinyHost.Resolve<FootballStandingsViewModel>();
                    standingsViewModel.OnInitializeView(standings);

                    standingsViewModels.Add(standingsViewModel);
                }
            }

            StandingsViewModels = new ObservableList<FootballStandingsViewModel>(
                standingsViewModels.OrderBy(elem => elem.LeagueTitle).ToArray());

            SetIsBusy(false);

            return _allForm;
        }

        #endregion Methods
    }
}