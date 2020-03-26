using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Match;
using Sharpnado.Presentation.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using PacketModels = PosePacket.Service.Football.Models;

namespace PoseSportsPredict.ViewModels.Football.Match
{
    public class FootballMatchesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            MatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<PacketModels.FootballFixtureDetail>>(GetMatchesAsync);
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            var timeSpan = DateTime.UtcNow - _lastUpdateTime;
#if DEBUG
            if (!MatchesTaskLoaderNotifier.IsNotStarted && timeSpan.TotalMinutes < 1) // 1분 마다 갱신
                return;
#else
            if (!MatchesTaskLoaderNotifier.IsNotStarted && timeSpan.TotalMinutes < 15) // 15분 마다 갱신
                return;
#endif
            MatchesTaskLoaderNotifier.Load();
        }

        #endregion NavigableViewModel

        #region Services

        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<PacketModels.FootballFixtureDetail>> _matchesTaskLoaderNotifier;
        private ObservableCollection<FootballMatchGroup> _matchGroups;
        private DateTime _matchDate;
        private DateTime _lastUpdateTime;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<PacketModels.FootballFixtureDetail>> MatchesTaskLoaderNotifier { get => _matchesTaskLoaderNotifier; set => SetValue(ref _matchesTaskLoaderNotifier, value); }
        public ObservableCollection<FootballMatchGroup> MatchGroups { get => _matchGroups; set => SetValue(ref _matchGroups, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectGroupHeaderCommand { get => new RelayCommand<FootballMatchGroup>((e) => SelectGroupHeader(e)); }

        private void SelectGroupHeader(FootballMatchGroup groupInfo)
        {
            groupInfo.Expanded = !groupInfo.Expanded;

            MatchGroups = new ObservableCollection<FootballMatchGroup>(MatchGroups);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchesViewModel(
            FootballMatchesPage page,
            IWebApiService webApiService) : base(page)
        {
            _webApiService = webApiService;

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }
        }

        #endregion Constructors

        #region Methods

        public FootballMatchesViewModel SetMatchDate(DateTime date)
        {
            _matchDate = date;
            return this;
        }

        private async Task<IReadOnlyCollection<PacketModels.FootballFixtureDetail>> GetMatchesAsync()
        {
            await Task.Delay(300);

            var result = await _webApiService.RequestAsyncWithToken<O_GET_FIXTURES_BY_DATE>(new WebRequestContext
            {
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_DATE,
                PostData = new I_GET_FIXTURES_BY_DATE
                {
                    StartTime = _matchDate.ToUniversalTime(),
                    EndTime = _matchDate.AddDays(1).ToUniversalTime(),
                }
            });

            if (result == null)
                throw new Exception(LocalizeString.Occur_Error);

            // Utc to Local, Check Image
            foreach (var fixture in result.Fixtures)
            {
                fixture.MatchTime = fixture.MatchTime.ToLocalTime();

                // 기본 이미지 설정
                if (string.IsNullOrEmpty(fixture.Country.Logo))
                    fixture.Country.Logo = "img_world.png";
                if (string.IsNullOrEmpty(fixture.League.Logo))
                    fixture.League.Logo = fixture.Country.Logo;
                if (string.IsNullOrEmpty(fixture.HomeTeam.Logo))
                    fixture.HomeTeam.Logo = "img_football.png";
                if (string.IsNullOrEmpty(fixture.AwayTeam.Logo))
                    fixture.AwayTeam.Logo = "img_football.png";
            }

            _lastUpdateTime = DateTime.UtcNow;

            InitializeMatcheGroups(result.Fixtures);

            return result.Fixtures;
        }

        private void InitializeMatcheGroups(List<PacketModels.FootballFixtureDetail> matchList)
        {
            ObservableCollection<FootballMatchGroup> matchGroupCollection;
            matchGroupCollection = new ObservableCollection<FootballMatchGroup>();

            var grouppingMatches = matchList.GroupBy(elem => $"{elem.Country.Name} - {elem.League.Name}");
            foreach (var grouppingMatch in grouppingMatches)
            {
                // 기존 데이터 있는지.. 있으면 Expanded값은 유지
                var foundExistData = MatchGroups?.Where(elem => elem.Title == grouppingMatch.Key).FirstOrDefault();
                matchGroupCollection.Add(new FootballMatchGroup(grouppingMatch.Key, grouppingMatch.First().Country.Logo, foundExistData?.Expanded ?? true)
                {
                    FootballMatchListViewModel = new FootballMatchListViewModel
                    {
                        Matches = new ObservableCollection<PacketModels.FootballFixtureDetail>(grouppingMatch.ToArray())
                    }
                });
            }

            MatchGroups = matchGroupCollection;
        }

        #endregion Methods
    }
}