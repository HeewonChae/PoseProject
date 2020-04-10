using PosePacket.Proxy;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using Sharpnado.Presentation.Forms;
using System;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballMatchDetailOverviewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            _overviewTaskLoaderNotifier = new TaskLoaderNotifier();

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (!_overviewTaskLoaderNotifier.IsNotStarted)
                return;

            _overviewTaskLoaderNotifier.Load(InitOverviewData);
        }

        #endregion BaseViewModel

        #region Services

        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private FootballMatchInfo _matchInfo;
        private TaskLoaderNotifier _overviewTaskLoaderNotifier;

        #endregion Fields

        #region Properties

        public bool IsCup => MatchInfo.LeagueType == FootballLeagueType.Cup;
        public FootballMatchInfo MatchInfo { get => _matchInfo; set => SetValue(ref _matchInfo, value); }
        public TaskLoaderNotifier OverviewTaskLoaderNotifier { get => _overviewTaskLoaderNotifier; set => SetValue(ref _overviewTaskLoaderNotifier, value); }

        #endregion Properties

        #region Constructors

        public FootballMatchDetailOverviewModel(IWebApiService webApiService)
        {
            _webApiService = webApiService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public FootballMatchDetailOverviewModel SetMatchInfo(FootballMatchInfo matchInfo)
        {
            _matchInfo = matchInfo;
            return this;
        }

        private async Task InitOverviewData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // call server
            var server_result = await _webApiService.RequestAsyncWithToken<O_GET_MATCH_OVERVIEW>(new WebRequestContext
            {
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_MATCH_OVERVIEW,
                PostData = new I_GET_MATCH_OVERVIEW
                {
                    FixtureId = _matchInfo.Id,
                }
            });

            // 기본정보 (평균 득실점, 회복기간)

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            SetIsBusy(false);
        }

        #endregion Methods
    }
}