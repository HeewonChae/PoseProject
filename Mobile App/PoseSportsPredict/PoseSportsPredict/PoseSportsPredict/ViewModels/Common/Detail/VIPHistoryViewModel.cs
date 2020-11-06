using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Logics.View.Converters;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Resources;
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

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class VIPHistoryViewModel : NavigableViewModel
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
            if (VIPMatchesTaskLoaderNotifier.IsNotStarted)
                VIPMatchesTaskLoaderNotifier.Load(InitVIPMatchDatas);
        }

        #endregion NavigableViewModel

        #region Services

        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballVIPMatchInfo>> _vipMatchesTaskLoaderNotifier;
        private List<FootballVIPMatchInfo> _matchList;
        private ObservableList<FootballVIPMatchInfo> _matches;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballVIPMatchInfo>> VIPMatchesTaskLoaderNotifier { get => _vipMatchesTaskLoaderNotifier; set => SetValue(ref _vipMatchesTaskLoaderNotifier, value); }
        public ObservableList<FootballVIPMatchInfo> Matches { get => _matches; set => SetValue(ref _matches, value); }

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

        #endregion Commands

        #region Constructors

        public VIPHistoryViewModel(
            VIPHistoryPage page,
            IWebApiService webApiService) : base(page)
        {
            _webApiService = webApiService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        private async Task<IReadOnlyCollection<FootballVIPMatchInfo>> InitVIPMatchDatas()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // call server
            var server_result = await _webApiService.RequestAsyncWithToken<O_GET_MATCH_VIP_HISTORY>(
                new WebRequestContext
                {
                    SerializeType = SerializeType.MessagePack,
                    MethodType = WebMethodType.GET,
                    BaseUrl = AppConfig.PoseWebBaseUrl,
                    ServiceUrl = FootballProxy.ServiceUrl,
                    SegmentGroup = FootballProxy.P_GET_MATCH_VIP_HISTORY,
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

            _matchList = _matchList.OrderByDescending(elem => elem.MatchTime).ToList();
            Matches = new ObservableList<FootballVIPMatchInfo>(_matchList);

            SetIsBusy(false);

            return _matchList;
        }

        public void NeedHistoryUpdate()
        {
            VIPMatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballVIPMatchInfo>>();
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