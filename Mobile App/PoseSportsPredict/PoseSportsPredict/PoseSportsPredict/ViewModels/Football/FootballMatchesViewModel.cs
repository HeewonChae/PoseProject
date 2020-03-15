using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football;
using Sharpnado.Presentation.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballMatchesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            MatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo>>();
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            var timeSpan = DateTime.UtcNow - _lastUpdateTime;
#if DEBUG
            if (!MatchesTaskLoaderNotifier.IsNotStarted && timeSpan.TotalSeconds < 30) // 30초 마다 갱신
                return;
#else
            if (!MatchesTaskLoaderNotifier.IsNotStarted && timeSpan.TotalMinutes < 15) // 15분 마다 갱신
                return;
#endif
            MatchesTaskLoaderNotifier.Load(GetMatchesAsync);
        }

        #endregion NavigableViewModel

        #region Services

        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo>> _MatchesTaskLoaderNotifier;
        private List<O_GET_FIXTURES_BY_DATE.FixtureInfo> _matchList;
        private ObservableCollection<MatchGroup> _matchGroups;
        private DateTime _matchDate;
        private DateTime _lastUpdateTime;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo>> MatchesTaskLoaderNotifier { get => _MatchesTaskLoaderNotifier; set => SetValue(ref _MatchesTaskLoaderNotifier, value); }
        public ObservableCollection<MatchGroup> MatchGroups { get => _matchGroups; set => SetValue(ref _matchGroups, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectGroupHeaderCommand { get => new RelayCommand<MatchGroup>((e) => SelectGroupHeader(e)); }

        private void SelectGroupHeader(MatchGroup groupInfo)
        {
            groupInfo.Expanded = !groupInfo.Expanded;
            //OnPropertyChanged("MatchGroups");
            UpdateMatches(_matchList);
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

        private async Task<IReadOnlyCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo>> GetMatchesAsync()
        {
            await Task.Delay(500);

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

            if (result.Fixtures.Count == 0)
                throw new Exception(LocalizeString.Occur_Error);

            if (result == null)
                throw new Exception(LocalizeString.Occur_Error);

            // Utc to Local
            foreach (var fixture in result.Fixtures)
            {
                fixture.MatchTime = fixture.MatchTime.ToLocalTime();
            }

            _lastUpdateTime = DateTime.UtcNow;

            UpdateMatches(result.Fixtures);

            return result.Fixtures;
        }

        private void UpdateMatches(List<O_GET_FIXTURES_BY_DATE.FixtureInfo> matchList)
        {
            _matchList = matchList;
            if (_matchList.Count == 0)
                return;

            var matchGroupCollection = new ObservableCollection<MatchGroup>();

            var grouppingMatchs = _matchList.GroupBy(elem => $"{elem.Country.Name} - {elem.League.Name}");
            foreach (var grouppingMatch in grouppingMatchs)
            {
                // 기존 데이터 있는지.. 있으면 Expanded값은 유지
                var foundExistData = MatchGroups?.Where(elem => elem.Title == grouppingMatch.Key).FirstOrDefault();

                var firstItem = grouppingMatch.First();
                var matchGroup = new MatchGroup(grouppingMatch.Key, firstItem.Country.Logo, foundExistData?.Expanded ?? true)
                {
                    FootballMatchListViewModel = new FootballMatchListViewModel
                    {
                        Matches = new ObservableCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo>(grouppingMatch.ToArray())
                    }
                };

                matchGroupCollection.Add(matchGroup);
            }

            MatchGroups = matchGroupCollection;
        }

        #endregion Methods
    }
}