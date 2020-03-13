using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football;
using Sharpnado.Presentation.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
            if (!MatchesTaskLoaderNotifier.IsNotStarted && timeSpan.TotalMinutes < 15) // 15분 마다 갱신
                return;

            MatchesTaskLoaderNotifier.Load(GetMatchesAsync);
        }

        #endregion NavigableViewModel

        #region Services

        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo>> _MatchesTaskLoaderNotifier;
        private List<O_GET_FIXTURES_BY_DATE.FixtureInfo> _matchList;
        private ObservableCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo> _matches;
        private DateTime _matchDate;
        private DateTime _lastUpdateTime;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo>> MatchesTaskLoaderNotifier { get => _MatchesTaskLoaderNotifier; set => SetValue(ref _MatchesTaskLoaderNotifier, value); }
        public ObservableCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo> Matches { get => _matches; set => SetValue(ref _matches, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectMatchCommand { get => new RelayCommand<O_GET_FIXTURES_BY_DATE.FixtureInfo>((e) => SelectMatch(e)); }

        private void SelectMatch(O_GET_FIXTURES_BY_DATE.FixtureInfo matchInfo)
        {
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
            await Task.Delay(1000);

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

            // Utc to Local
            foreach (var fixture in result.Fixtures)
            {
                fixture.MatchTime = fixture.MatchTime.ToLocalTime();
            }

            // Binding Match datas
            _matchList = result.Fixtures;
            Matches = new ObservableCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo>(_matchList);

            _lastUpdateTime = DateTime.UtcNow;

            return result.Fixtures;
        }

        #endregion Methods
    }
}