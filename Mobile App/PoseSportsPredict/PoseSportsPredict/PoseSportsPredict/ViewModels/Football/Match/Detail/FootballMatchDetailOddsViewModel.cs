using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Match.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballMatchDetailOddsViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            OddsTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballOddsInfo>>();
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (!OddsTaskLoaderNotifier.IsNotStarted)
                return;

            OddsTaskLoaderNotifier.Load(InitOddsData);
        }

        #endregion BaseViewModel

        #region Services

        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private FootballMatchInfo _matchInfo;

        private TaskLoaderNotifier<IReadOnlyCollection<FootballOddsInfo>> _oddsTaskLoaderNotifier;
        private ObservableList<FootballOddsInfo> _oddsInfos;
        private double _oddsBookmakerHeight;
        private double _oddsGaugeHeight;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballOddsInfo>> OddsTaskLoaderNotifier { get => _oddsTaskLoaderNotifier; set => SetValue(ref _oddsTaskLoaderNotifier, value); }
        public ObservableList<FootballOddsInfo> OddsInfos { get => _oddsInfos; set => SetValue(ref _oddsInfos, value); }
        public double OddsBookmakerHeight { get => _oddsBookmakerHeight; set => SetValue(ref _oddsBookmakerHeight, value); }
        public double OddsGaugeHeight { get => _oddsGaugeHeight; set => SetValue(ref _oddsGaugeHeight, value); }

        #endregion Properties

        #region Constructors

        public FootballMatchDetailOddsViewModel(
            FootballMatchDetailOddsView view,
            IWebApiService webApiService) : base(view)
        {
            _webApiService = webApiService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public FootballMatchDetailOddsViewModel SetMatchInfo(FootballMatchInfo matchInfo)
        {
            _matchInfo = matchInfo;
            return this;
        }

        private async Task<IReadOnlyCollection<FootballOddsInfo>> InitOddsData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // call server
            var server_result = await _webApiService.RequestAsyncWithToken<O_GET_MATCH_ODDS>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_MATCH_ODDS,
                PostData = new I_GET_MATCH_ODDS
                {
                    FixtureId = _matchInfo.Id,
                }
            });

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            if (server_result.OddsDetails.Length == 0)
            {
                SetIsBusy(false);
                return null;
            }

            var oddsInfos = new List<FootballOddsInfo>();
            foreach (var oddsDetail in server_result.OddsDetails)
            {
                oddsInfos.Add(ShinyHost.Resolve<OddsDetailToOddsInfo>().Convert(oddsDetail));
            }

            var screenHelper = DependencyService.Resolve<IScreenHelper>();
            OddsBookmakerHeight = screenHelper.ScreenSize.Width / 4.1;

            OddsInfos = new ObservableList<FootballOddsInfo>(oddsInfos);

            SetIsBusy(false);

            return oddsInfos;
        }

        #endregion Methods
    }
}