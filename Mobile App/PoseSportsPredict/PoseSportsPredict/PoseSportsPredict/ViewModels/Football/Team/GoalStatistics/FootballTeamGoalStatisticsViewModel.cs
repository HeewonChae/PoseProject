using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Base;
using Sharpnado.Presentation.Forms;
using Shiny;
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
        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _taskLoaderNotifier;

        #endregion Fields

        #region Properties

        public int Ddl_selectedIndex { get => _ddl_selectedIndex; set => SetValue(ref _ddl_selectedIndex, value); }
        public ObservableList<FootballLeagueInfo> Ddl_Leagues { get => _ddlLeagues; set => SetValue(ref _ddlLeagues, value); }
        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> TaskLoaderNotifier { get => _taskLoaderNotifier; set => SetValue(ref _taskLoaderNotifier, value); }

        #endregion Properties

        #region Commands

        public ICommand LeagueSelectionChangedCommand { get => new RelayCommand<SelectionChangedEventArgs>((e) => LeagueSelectionChanged(e)); }

        private void LeagueSelectionChanged(SelectionChangedEventArgs arg)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            TaskLoaderNotifier.Load(InitData);

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

            TaskLoaderNotifier.Load(InitData);
        }

        private Task<IReadOnlyCollection<FootballMatchInfo>> InitData()
        {
            if (_matchesByLeague.Count == 0)
                return null;

            var selectedLeague = _matchesByLeague.ElementAt(Ddl_selectedIndex);

            return Task.FromResult(selectedLeague.Value as IReadOnlyCollection<FootballMatchInfo>);
        }

        #endregion Methods
    }
}