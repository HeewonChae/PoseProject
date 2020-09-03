using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Enum;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Football.Enums;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.ViewModels.Football.Match.RecentForm;
using Sharpnado.Presentation.Forms;
using Shiny;
using Syncfusion.SfChart.XForms;
using Syncfusion.XForms.ComboBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PoseSportsPredict.ViewModels.Football.Team.GoalStatistics
{
    public class FootballTeamGoalStatisticsViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            Ddl_selectedIndex = -1;
            TaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();
            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private FootballTeamInfo _teamInfo;
        private FootballMatchInfo _matchInfo;
        private Dictionary<FootballLeagueInfo, List<FootballMatchInfo>> _matchesByLeague;

        private int _ddl_selectedIndex;
        private List<FootballLeagueInfo> _ddlLeagues;
        private TeamCampType _selectedTeamType;
        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _taskLoaderNotifier;
        private FootballRecentFormViewModel _recentFormViewModel;
        private ObservableList<FootballGoalLineChartData> _goalForCahrtDatas;
        private ObservableList<FootballGoalLineChartData> _goalAgainstCahrtDatas;
        private FootballGoalStatisticsType _goalStatisticsType;
        private FootballTeamStatistics _teamStatistics;
        private FootballMatchStatistics _matchStatistics;
        private string _chartCategory1;
        private string _chartCategory2;

        #endregion Fields

        #region Properties

        public int Ddl_selectedIndex { get => _ddl_selectedIndex; set => SetValue(ref _ddl_selectedIndex, value); }
        public List<FootballLeagueInfo> Ddl_Leagues { get => _ddlLeagues; set => SetValue(ref _ddlLeagues, value); }
        public TeamCampType SelectedTeamType { get => _selectedTeamType; set => SetValue(ref _selectedTeamType, value); }
        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> TaskLoaderNotifier { get => _taskLoaderNotifier; set => SetValue(ref _taskLoaderNotifier, value); }
        public FootballRecentFormViewModel RecentFormViewModel { get => _recentFormViewModel; set => SetValue(ref _recentFormViewModel, value); }
        public ObservableList<FootballGoalLineChartData> GoalForCahrtDatas { get => _goalForCahrtDatas; set => SetValue(ref _goalForCahrtDatas, value); }
        public ObservableList<FootballGoalLineChartData> GoalAgainstCahrtDatas { get => _goalAgainstCahrtDatas; set => SetValue(ref _goalAgainstCahrtDatas, value); }
        public FootballTeamStatistics TeamStatistics { get => _teamStatistics; set => SetValue(ref _teamStatistics, value); }
        public FootballMatchStatistics MatchStatistics { get => _matchStatistics; set => SetValue(ref _matchStatistics, value); }
        public FootballGoalStatisticsType GoalStatisticsType { get => _goalStatisticsType; set => SetValue(ref _goalStatisticsType, value); }
        public string ChartCategory1 { get => _chartCategory1; set => SetValue(ref _chartCategory1, value); }
        public string ChartCategory2 { get => _chartCategory2; set => SetValue(ref _chartCategory2, value); }

        #endregion Properties

        #region Commands

        public ICommand LeagueSelectionChangedCommand { get => new RelayCommand<SelectionChangedEventArgs>((e) => LeagueSelectionChanged(e)); }

        private void LeagueSelectionChanged(SelectionChangedEventArgs arg)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            TaskLoaderNotifier.Load(InitData);
        }

        public ICommand TotalClickCommand { get => new RelayCommand(TotalClick); }

        private void TotalClick()
        {
            if (IsBusy
                || SelectedTeamType == TeamCampType.Any)
                return;

            SetIsBusy(true);

            SelectedTeamType = TeamCampType.Any;
            TaskLoaderNotifier.Load(InitData);
        }

        public ICommand HomeClickCommand { get => new RelayCommand(HomeClick); }

        private void HomeClick()
        {
            if (IsBusy
                || SelectedTeamType == TeamCampType.Home)
                return;

            SetIsBusy(true);

            SelectedTeamType = TeamCampType.Home;
            TaskLoaderNotifier.Load(InitData);
        }

        public ICommand AwayClickCommand { get => new RelayCommand(AwayClick); }

        private void AwayClick()
        {
            if (IsBusy
                || SelectedTeamType == TeamCampType.Away)
                return;

            SetIsBusy(true);

            SelectedTeamType = TeamCampType.Away;
            TaskLoaderNotifier.Load(InitData);
        }

        public ICommand ChartSelectedCommand { get => new RelayCommand<ChartSelectionEventArgs>(e => ChartSelected(e)); }

        private async void ChartSelected(ChartSelectionEventArgs e)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var items = (ObservableList<FootballGoalLineChartData>)e.SelectedSeries.ItemsSource;
            int dataIdx = e.SelectedDataPointIndex;

            FootballGoalLineChartData selectedData = null;
            if (items.Count > dataIdx)
                selectedData = ((ObservableList<FootballGoalLineChartData>)e.SelectedSeries.ItemsSource).ElementAt(dataIdx);

            if (selectedData != null)
            {
                var selectedLeague = _matchesByLeague.ElementAt(Ddl_selectedIndex);
                var matchInfo = selectedLeague.Value.Find(elem => elem.Id == selectedData.FixtureId);

                await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);
            }

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballTeamGoalStatisticsViewModel()
        {
            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public void SetMember(Dictionary<FootballLeagueInfo, List<FootballMatchInfo>> matchesByLeague, FootballTeamInfo teamInfo = null, FootballMatchInfo matchInfo = null, FootballGoalStatisticsType goalStatisticsType = FootballGoalStatisticsType.Team)
        {
            _matchesByLeague = matchesByLeague;

            Ddl_Leagues = new List<FootballLeagueInfo>(
                _matchesByLeague.Select(elem => elem.Key));

            Ddl_selectedIndex = 0;
            SelectedTeamType = TeamCampType.Any;
            GoalStatisticsType = goalStatisticsType;

            if (GoalStatisticsType == FootballGoalStatisticsType.Team)
            {
                _teamInfo = teamInfo;
                ChartCategory1 = LocalizeString.Goal_For;
                ChartCategory2 = LocalizeString.Goal_Against;
            }
            else if (GoalStatisticsType == FootballGoalStatisticsType.H2H)
            {
                _matchInfo = matchInfo;
                _teamInfo = ShinyHost.Resolve<MatchInfoToTeamInfo>().Convert(_matchInfo, TeamCampType.Home);
                ChartCategory1 = _matchInfo.HomeName;
                ChartCategory2 = _matchInfo.AwayName;

                var selectedLeague = Ddl_Leagues
                    .FirstOrDefault(elem => elem.LeagueName == _matchInfo.LeagueName && elem.CountryName == _matchInfo.League_CountryName);

                if (selectedLeague != null)
                {
                    Ddl_selectedIndex = Ddl_Leagues.FindIndex(elem => elem == selectedLeague);
                }
            }

            TaskLoaderNotifier.Load(InitData);
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> InitData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            if (_matchesByLeague.Count == 0)
            {
                SetIsBusy(false);
                return null;
            }

            var selectedLeague = _matchesByLeague.ElementAt(Ddl_selectedIndex);

            // 캠프별로 경기 나누기
            var totalMatches = selectedLeague.Value.Take(6);
            var homeMatches = selectedLeague.Value.Where(elem => elem.HomeTeamId == _teamInfo.TeamId).Take(6);
            var awayMatches = selectedLeague.Value.Where(elem => elem.AwayTeamId == _teamInfo.TeamId).Take(6);

            var selectedMatches = SelectedTeamType == TeamCampType.Any ? totalMatches.ToList() :
                SelectedTeamType == TeamCampType.Home ? homeMatches.ToList() : awayMatches.ToList();

            if (selectedMatches.Count == 0)
            {
                SetIsBusy(false);
                return selectedMatches;
            }

            // Statistic
            if (GoalStatisticsType == FootballGoalStatisticsType.Team)
            {
                TeamStatistics = ShinyHost.Resolve<MatchInfoToTeamStatistics>()
                .Convert(selectedMatches, _teamInfo.TeamId, selectedMatches.Count, 0);
            }
            else if (GoalStatisticsType == FootballGoalStatisticsType.H2H)
            {
                MatchStatistics = new FootballMatchStatistics
                {
                    HomeTeamStatistics = ShinyHost.Resolve<MatchInfoToTeamStatistics>()
                    .Convert(selectedMatches, _matchInfo.HomeTeamId, selectedMatches.Count, 0),

                    AwayTeamStatistics = ShinyHost.Resolve<MatchInfoToTeamStatistics>()
                    .Convert(selectedMatches, _matchInfo.AwayTeamId, selectedMatches.Count, 0),
                };
            }

            // chart
            var (goalFors, goalAgainsts) = ShinyHost.Resolve<MatchInfoToGoalLineChartData>().Convert(selectedMatches, _teamInfo.TeamId, 6);
            GoalForCahrtDatas = new ObservableList<FootballGoalLineChartData>(goalFors);
            GoalAgainstCahrtDatas = new ObservableList<FootballGoalLineChartData>(goalAgainsts);

            // RecentForm
            RecentFormViewModel = RecentFormViewModel ?? ShinyHost.Resolve<FootballRecentFormViewModel>();
            RecentFormViewModel.SetMembers(selectedMatches, _teamInfo.TeamId);

            SetIsBusy(false);

            return selectedMatches;
        }

        #endregion Methods
    }
}