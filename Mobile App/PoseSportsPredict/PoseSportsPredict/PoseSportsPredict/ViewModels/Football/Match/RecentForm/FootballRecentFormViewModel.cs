using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Base;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.ViewModels.Football.Match.RecentForm
{
    public class FootballRecentFormViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            TaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();

            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _taskLoaderNotifier;
        private short _homeTeamId;
        private short _awayTeamId;
        private List<FootballMatchInfo> _homeRecentForm;
        private List<FootballMatchInfo> _awayRecentForm;

        private bool _isTeamSelectorVisible;
        private short _selectedTeamId;
        private bool _isSelectedHomeTeam;
        private ObservableList<FootballMatchInfo> _selectedRecentForm;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> TaskLoaderNotifier { get => _taskLoaderNotifier; set => SetValue(ref _taskLoaderNotifier, value); }
        public bool IsTeamSelectorVisible { get => _isTeamSelectorVisible; set => SetValue(ref _isTeamSelectorVisible, value); }
        public short SelectedTeamId { get => _selectedTeamId; set => SetValue(ref _selectedTeamId, value); }
        public bool IsSelectedHomeTeam { get => _isSelectedHomeTeam; set => SetValue(ref _isSelectedHomeTeam, value); }
        public ObservableList<FootballMatchInfo> SelectedRecentForm { get => _selectedRecentForm; set => SetValue(ref _selectedRecentForm, value); }

        #endregion Properties

        #region Constructors

        public FootballRecentFormViewModel()
        {
            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public void SetMembers(List<FootballMatchInfo> homeMatches, short homeTeamId, List<FootballMatchInfo> awayMatches = null, short awayTeamId = 0)
        {
            _homeTeamId = homeTeamId;
            _awayTeamId = awayTeamId;
            _homeRecentForm = homeMatches;
            _awayRecentForm = awayMatches;

            IsTeamSelectorVisible = _homeRecentForm != null && _awayRecentForm != null;
            IsSelectedHomeTeam = true;

            TaskLoaderNotifier.Load(InitData);
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> InitData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            SelectedTeamId = IsSelectedHomeTeam ? _homeTeamId : _awayTeamId;

            SelectedRecentForm = new ObservableList<FootballMatchInfo>(
                IsSelectedHomeTeam ? _homeRecentForm : _awayRecentForm);

            SetIsBusy(false);

            return SelectedRecentForm;

            #endregion Methods
        }
    }
}