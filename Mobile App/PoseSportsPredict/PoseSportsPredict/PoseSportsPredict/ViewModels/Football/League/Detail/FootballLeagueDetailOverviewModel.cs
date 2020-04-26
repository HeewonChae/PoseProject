using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Standings;
using PoseSportsPredict.Views.Football.League.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.ViewModels.Football.League.Detail
{
    public class FootballLeagueDetailOverviewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            OverviewTaskLoaderNotifier = new TaskLoaderNotifier();

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (!OverviewTaskLoaderNotifier.IsNotStarted)
                return;

            OverviewTaskLoaderNotifier.Load(InitOverviewData);
        }

        #endregion BaseViewModel

        #region Fields

        private TaskLoaderNotifier _overviewTaskLoaderNotifier;
        private ObservableList<FootballStandingsViewModel> _standingsViewModels;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier OverviewTaskLoaderNotifier { get => _overviewTaskLoaderNotifier; set => SetValue(ref _overviewTaskLoaderNotifier, value); }
        public ObservableList<FootballStandingsViewModel> StandingsViewModels { get => _standingsViewModels; set => SetValue(ref _standingsViewModels, value); }

        #endregion Properties

        #region Constructors

        public FootballLeagueDetailOverviewModel(
            FootballLeagueDetailOverview view) : base(view)
        {
            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        private async Task InitOverviewData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            //// call server
            //var server_result = await _webApiService.RequestAsyncWithToken<O_GET_MATCH_OVERVIEW>(new WebRequestContext
            //{
            //    SerializeType = SerializeType.MessagePack,
            //    MethodType = WebMethodType.POST,
            //    BaseUrl = AppConfig.PoseWebBaseUrl,
            //    ServiceUrl = FootballProxy.ServiceUrl,
            //    SegmentGroup = FootballProxy.P_GET_MATCH_OVERVIEW,
            //    PostData = new I_GET_MATCH_OVERVIEW
            //    {
            //        FixtureId = _matchInfo.Id,
            //    }
            //});

            //if (server_result == null)
            //    throw new Exception(LocalizeString.Occur_Error);

            //// Set RankColor
            //var descGroups = standingsInfos.OrderBy(elem => elem.Rank).GroupBy(elem => elem.Description);
            //int positiveDescCnt = 0;
            //int negativeDescCnt = 0;
            //int neutralDescCnt = 0;
            //foreach (var descGroup in descGroups)
            //{
            //    if (string.IsNullOrEmpty(descGroup.Key))
            //        continue;

            //    var descCategoryType = StandginsDescription.GetDescCategory(descGroup.Key);
            //    Color descColor;
            //    if (descCategoryType == StandingsDescCategoryType.Positive)
            //    {
            //        descColor = StandingsRankColor.GetRankColor(descCategoryType, positiveDescCnt);
            //        positiveDescCnt++;
            //    }
            //    else if (descCategoryType == StandingsDescCategoryType.Negative)
            //    {
            //        descColor = StandingsRankColor.GetRankColor(descCategoryType, negativeDescCnt);
            //        negativeDescCnt++;
            //    }
            //    else if (descCategoryType == StandingsDescCategoryType.Neutral)
            //    {
            //        descColor = StandingsRankColor.GetRankColor(descCategoryType, neutralDescCnt);
            //        neutralDescCnt++;
            //    }
            //    else
            //    {
            //        descColor = StandingsRankColor.GetRankColor(StandingsDescCategoryType.None, 0);
            //    }

            //    foreach (var standings in descGroup)
            //    {
            //        standings.RankColor = descColor;
            //    }
            //}

            //// Groupping Standings by group data
            //var standingsGroups = standingsInfos.OrderBy(elem => elem.Rank).GroupBy(elem => elem.Group);
            //StandingsViewModels = new ObservableList<FootballStandingsViewModel>();
            //foreach (var standingsGroup in standingsGroups)
            //{
            //    var standings = standingsGroup.ToArray();
            //    if (standings.Length >= 2)
            //    {
            //        var standingsViewModel = ShinyHost.Resolve<FootballStandingsViewModel>();
            //        standingsViewModel.OnInitializeView(standings);

            //        StandingsViewModels.Add(standingsViewModel);
            //    }
            //}

            //SetIsBusy(false);
        }

        #endregion Methods
    }
}