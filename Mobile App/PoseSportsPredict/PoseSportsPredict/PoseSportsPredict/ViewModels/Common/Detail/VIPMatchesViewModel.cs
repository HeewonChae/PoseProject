using GalaSoft.MvvmLight.Command;
using PanCardView.Extensions;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Logics.View.Converters;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.Views.Common.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class VIPMatchesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            VIPMatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballVIPMatchInfo>>();
            MessagingCenter.Subscribe<SettingsViewModel, CoverageLanguage>(this, AppConfig.CULTURE_CHANGED_MSG, OnCultureChanged);

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (VIPMatchesTaskLoaderNotifier.IsSuccessfullyCompleted)
            {
                PullToRefresh();
                return;
            }

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

        #region Commands

        public ICommand SelectMatchCommand { get => new RelayCommand<FootballVIPMatchInfo>((e) => SelectMatch(e)); }

        private async void SelectMatch(FootballVIPMatchInfo vipMatchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            FootballMatchInfo matchInfo = ShinyHost.Resolve<VIPMatchInfoToMatchInfo>().Convert(vipMatchInfo);
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);

            SetIsBusy(false);
        }

        public ICommand SelectMatch_LongTapCommand { get => new RelayCommand<FootballVIPMatchInfo>((e) => SelectMatch_LongTap(e)); }

        private void SelectMatch_LongTap(FootballVIPMatchInfo vipMatchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            FootballMatchInfo matchInfo = ShinyHost.Resolve<VIPMatchInfoToMatchInfo>().Convert(vipMatchInfo);
            MatchInfoLongTapPopup.Execute(matchInfo);

            SetIsBusy(false);
        }

        public ICommand PullToRefreshCommand { get => new RelayCommand(PullToRefresh); }

        private async void PullToRefresh()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            IsListViewRefrashing = true;

            var timeSpan = DateTime.UtcNow - _lastUpdateTime;
            if (timeSpan.TotalMinutes > 1) // 1분 마다 갱신
                await RefreshMatchesAsync();

            IsListViewRefrashing = false;
            SetIsBusy(false);
        }

        #endregion Commands

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

            var group_vipFixtures = server_result.VIPFixtureDetails.GroupBy(elem => elem.FixtureId);

            _matchList = new List<FootballVIPMatchInfo>();
            foreach (var group_vipFixture in group_vipFixtures)
            {
                var vipFixtures = group_vipFixture.ToArray();
                var vipMatchInfo = ShinyHost.Resolve<VIPFixtureDetailToVIPMatchInfo>().Convert(vipFixtures);
                _matchList.Add(vipMatchInfo);
            }

            _matchList = _matchList.OrderBy(elem => elem.MatchTime).ToList();
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
            bool isNeedHistoryUpdate = false;
            foreach (var match in updatedMatches)
            {
                var vipMatch = _matchList.Find(elem => elem.Id == match.Id);
                vipMatch.MatchStatus = match.MatchStatus;
                vipMatch.HomeScore = match.HomeScore;
                vipMatch.AwayScore = match.AwayScore;
                vipMatch.MatchTime = match.MatchTime;

                if (vipMatch.MatchStatus == FootballMatchStatusType.FT
                   || vipMatch.MatchStatus == FootballMatchStatusType.AET
                   || vipMatch.MatchStatus == FootballMatchStatusType.PEN)
                {
                    isNeedHistoryUpdate = true;
                }
                else
                {
                    needRefrashMatchIndexes.Remove(match.Id);
                }
            }

            foreach (var remainIndex in needRefrashMatchIndexes)
            {
                var foundIdx = _matchList.FindIndex(elem => elem.Id == remainIndex);
                _matchList.RemoveAt(foundIdx);
            }

            if (isNeedHistoryUpdate)
            {
                // VIP History 페이지로 전달
                VIPHistoryViewModel.Singleton.NeedHistoryUpdate();
            }

            Matches = new ObservableList<FootballVIPMatchInfo>(_matchList);

            _lastUpdateTime = DateTime.UtcNow;

            this.SetIsBusy(false);
        }

        private void OnCultureChanged(object sender, CoverageLanguage cl)
        {
            if (_matchList == null || _matchList.Count == 0)
                return;

            var predTitleConverter = ShinyHost.Resolve<PredictionLabelToString>();

            foreach (var vipMmatch in _matchList)
            {
                foreach (var pick in vipMmatch.PredictionPicks)
                {
                    pick.Title = predTitleConverter.Convert(pick.MainLabel, pick.SubLabel);
                }
            }
            Matches = new ObservableList<FootballVIPMatchInfo>(_matchList);
        }

        #endregion Methods
    }
}