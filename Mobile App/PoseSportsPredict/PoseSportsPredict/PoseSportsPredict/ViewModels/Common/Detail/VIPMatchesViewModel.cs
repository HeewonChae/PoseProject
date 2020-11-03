using PanCardView.Extensions;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Football;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Common.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class VIPMatchesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            VIPMatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballVIPMatchInfo>>();

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (VIPMatchesTaskLoaderNotifier.IsNotStarted)
                VIPMatchesTaskLoaderNotifier.Load(InitVIPMatchDatas);
        }

        #endregion NavigableViewModel

        #region Services

        private MembershipService _membershipService;
        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private DateTime _lastUpdateTime;
        private bool _isListViewRefrashing;
        private TaskLoaderNotifier<IReadOnlyCollection<FootballVIPMatchInfo>> _vipMatchesTaskLoaderNotifier;
        private List<FootballVIPMatchInfo> _matchList;
        private ObservableList<FootballVIPMatchInfo> _matches;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballVIPMatchInfo>> VIPMatchesTaskLoaderNotifier { get => _vipMatchesTaskLoaderNotifier; set => SetValue(ref _vipMatchesTaskLoaderNotifier, value); }
        public ObservableList<FootballVIPMatchInfo> Matches { get => _matches; set => SetValue(ref _matches, value); }
        public bool IsListViewRefrashing { get => _isListViewRefrashing; set => SetValue(ref _isListViewRefrashing, value); }

        #endregion Properties

        #region Constructors

        public VIPMatchesViewModel(
            VIPMatchesPage page,
            MembershipService membershipService,
            IWebApiService webApiService) : base(page)
        {
            _membershipService = membershipService;
            _webApiService = webApiService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        private async Task<IReadOnlyCollection<FootballVIPMatchInfo>> InitVIPMatchDatas()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            if (_membershipService.MemberRoleType < PosePacket.Service.Enum.MemberRoleType.Promotion)
            {
                await MaterialDialog.Instance.AlertAsync(LocalizeString.For_VIP_User,
                                    LocalizeString.App_Title,
                                    LocalizeString.Ok,
                                    DialogConfiguration.AppTitleAlterDialogConfiguration);
                SetIsBusy(false);
                throw new Exception(LocalizeString.Occur_Error);
            }

            // call server
            var server_result = await _webApiService.RequestAsyncWithToken<O_GET_MATCH_VIP>(
                new WebRequestContext
                {
                    SerializeType = SerializeType.MessagePack,
                    MethodType = WebMethodType.GET,
                    BaseUrl = AppConfig.PoseWebBaseUrl,
                    ServiceUrl = FootballProxy.ServiceUrl,
                    SegmentGroup = FootballProxy.P_GET_MATCH_VIP,
                });

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            _matchList = new List<FootballVIPMatchInfo>();
            foreach (var vipFixture in server_result.VIPFixtureDetails)
            {
            }

            Matches = new ObservableList<FootballVIPMatchInfo>(_matchList);

            _lastUpdateTime = DateTime.UtcNow;

            SetIsBusy(false);

            return _matchList;
        }

        private async Task RefreshMatchesAsync()
        {
            this.SetIsBusy(true);

            await Task.Delay(300);

            var needRefrashMatchIndexes = _matchList.Where(elem => elem.MatchTime < DateTime.Now
                   && elem.MatchStatus != FootballMatchStatusType.FT
                   && elem.MatchStatus != FootballMatchStatusType.AET
                   && elem.MatchStatus != FootballMatchStatusType.PEN)
                   .Select(elem => elem.Id).ToList();

            if (needRefrashMatchIndexes.Count() == 0)
            {
                _lastUpdateTime = DateTime.UtcNow;
                this.SetIsBusy(false);
                return;
            }

            var updatedMatches = await RefreshMatchInfos.Execute(needRefrashMatchIndexes.ToArray());
            foreach (var match in updatedMatches)
            {
                var vipMatch = _matchList.Find(elem => elem.Id == match.Id);
                vipMatch.MatchStatus = match.MatchStatus;
                vipMatch.HomeScore = match.HomeScore;
                vipMatch.AwayScore = match.AwayScore;
                vipMatch.MatchTime = match.MatchTime;

                needRefrashMatchIndexes.Remove(match.Id);
            }

            foreach (var remainIndex in needRefrashMatchIndexes)
            {
                var foundIdx = _matchList.FindIndex(elem => elem.Id == remainIndex);
                _matchList.RemoveAt(foundIdx);
            }

            Matches = new ObservableList<FootballVIPMatchInfo>(_matchList);

            _lastUpdateTime = DateTime.UtcNow;

            this.SetIsBusy(false);
        }

        #endregion Methods
    }
}