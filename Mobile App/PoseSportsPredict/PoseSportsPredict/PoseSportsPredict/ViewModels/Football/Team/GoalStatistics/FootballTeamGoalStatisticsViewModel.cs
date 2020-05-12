using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
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
        private Dictionary<FootballLeagueInfo, List<FootballMatchInfo>> _matchesByLeague;

        private int _ddl_selectedIndex;
        private ObservableList<FootballLeagueInfo> _ddlLeagues;
        private TeamCampType _selectedTeamType;
        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _taskLoaderNotifier;
        private FootballRecentFormViewModel _recentFormViewModel;
        private string _strGFScore;
        private string _strGaScore;
        private double _totalScoreAvg;
        private double _GFScoreAvg;
        private double _GaScoreAvg;
        private ObservableList<FootballGoalLineChartData> _goalForCahrtDatas;
        private ObservableList<FootballGoalLineChartData> _goalAgainstCahrtDatas;

        #endregion Fields

        #region Properties

        public int Ddl_selectedIndex { get => _ddl_selectedIndex; set => SetValue(ref _ddl_selectedIndex, value); }
        public ObservableList<FootballLeagueInfo> Ddl_Leagues { get => _ddlLeagues; set => SetValue(ref _ddlLeagues, value); }
        public TeamCampType SelectedTeamType { get => _selectedTeamType; set => SetValue(ref _selectedTeamType, value); }
        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> TaskLoaderNotifier { get => _taskLoaderNotifier; set => SetValue(ref _taskLoaderNotifier, value); }
        public FootballRecentFormViewModel RecentFormViewModel { get => _recentFormViewModel; set => SetValue(ref _recentFormViewModel, value); }
        public string StrGFScore { get => _strGFScore; set => SetValue(ref _strGFScore, value); }
        public string StrGAScore { get => _strGaScore; set => SetValue(ref _strGaScore, value); }
        public double TotalScoreAvg { get => _totalScoreAvg; set => SetValue(ref _totalScoreAvg, value); }
        public double GFScoreAvg { get => _GFScoreAvg; set => SetValue(ref _GFScoreAvg, value); }
        public double GAScoreAvg { get => _GaScoreAvg; set => SetValue(ref _GaScoreAvg, value); }
        public ObservableList<FootballGoalLineChartData> GoalForCahrtDatas { get => _goalForCahrtDatas; set => SetValue(ref _goalForCahrtDatas, value); }
        public ObservableList<FootballGoalLineChartData> GoalAgainstCahrtDatas { get => _goalAgainstCahrtDatas; set => SetValue(ref _goalAgainstCahrtDatas, value); }

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
                || SelectedTeamType == TeamCampType.Total)
                return;

            SetIsBusy(true);

            SelectedTeamType = TeamCampType.Total;
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

        public void SetMember(Dictionary<FootballLeagueInfo, List<FootballMatchInfo>> matchesByLeague, FootballTeamInfo teamInfo)
        {
            _matchesByLeague = matchesByLeague;
            _teamInfo = teamInfo;

            Ddl_Leagues = new ObservableList<FootballLeagueInfo>(
                _matchesByLeague.Select(elem => elem.Key));

            Ddl_selectedIndex = 0;
            SelectedTeamType = TeamCampType.Total;

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

            // TeamStatistic
            var teamStatistics = ShinyHost.Resolve<MatchInfoToTeamStatistics>()
                .Convert(selectedLeague.Value, _teamInfo.TeamId, 6, 6);

            // 평균 득점
            StrGFScore = SelectedTeamType == TeamCampType.Total ? teamStatistics.TotalGoalFor.Text :
                    SelectedTeamType == TeamCampType.Home ? teamStatistics.HomeGoalFor.Text : teamStatistics.AwayGoalFor.Text;
            StrGAScore = SelectedTeamType == TeamCampType.Total ? teamStatistics.TotalGoalAgainst.Text :
                SelectedTeamType == TeamCampType.Home ? teamStatistics.HoemGoalAgainst.Text : teamStatistics.AwayGoalAgainst.Text;
            TotalScoreAvg = SelectedTeamType == TeamCampType.Total ? teamStatistics.TotalGoalAvg :
                SelectedTeamType == TeamCampType.Home ? teamStatistics.TotalHomeGoalAvg : teamStatistics.TotalAwayGoalAvg;
            GFScoreAvg = SelectedTeamType == TeamCampType.Total ? teamStatistics.TotalGoalFor.Avg :
                SelectedTeamType == TeamCampType.Home ? teamStatistics.HomeGoalFor.Avg : teamStatistics.AwayGoalFor.Avg;
            GAScoreAvg = SelectedTeamType == TeamCampType.Total ? teamStatistics.TotalGoalAgainst.Avg :
                SelectedTeamType == TeamCampType.Home ? teamStatistics.HoemGoalAgainst.Avg : teamStatistics.AwayGoalAgainst.Avg;

            var selectedMatches = SelectedTeamType == TeamCampType.Total ? totalMatches.ToList() :
                SelectedTeamType == TeamCampType.Home ? homeMatches.ToList() : awayMatches.ToList();

            if (selectedMatches.Count == 0)
            {
                SetIsBusy(false);
                return selectedMatches;
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