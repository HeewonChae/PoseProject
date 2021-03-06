﻿using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Enum;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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
        private TeamCampType _selectedTeamType;
        private ObservableList<FootballMatchInfo> _selectedRecentForm;
        private MatchResultType _lastMatchResult;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> TaskLoaderNotifier { get => _taskLoaderNotifier; set => SetValue(ref _taskLoaderNotifier, value); }
        public bool IsTeamSelectorVisible { get => _isTeamSelectorVisible; set => SetValue(ref _isTeamSelectorVisible, value); }
        public short SelectedTeamId { get => _selectedTeamId; set => SetValue(ref _selectedTeamId, value); }
        public TeamCampType SelectedTeamType { get => _selectedTeamType; set => SetValue(ref _selectedTeamType, value); }
        public ObservableList<FootballMatchInfo> SelectedRecentForm { get => _selectedRecentForm; set => SetValue(ref _selectedRecentForm, value); }
        public MatchResultType LastMatchResult { get => _lastMatchResult; set => SetValue(ref _lastMatchResult, value); }

        #endregion Properties

        #region Commands

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

        public ICommand SelectMatchCommand { get => new RelayCommand<FootballMatchInfo>((e) => SelectMatch(e)); }

        private async void SelectMatch(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);

            SetIsBusy(false);
        }

        public ICommand SelectMatch_LongTapCommand { get => new RelayCommand<FootballMatchInfo>((e) => SelectMatch_LongTap(e)); }

        private void SelectMatch_LongTap(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            MatchInfoLongTapPopup.Execute(matchInfo);

            SetIsBusy(false);
        }

        #endregion Commands

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
            _homeRecentForm = homeMatches.OrderByDescending(elem => elem.MatchTime).ToList();
            _awayRecentForm = awayMatches?.OrderByDescending(elem => elem.MatchTime).ToList() ?? null;

            IsTeamSelectorVisible = _homeRecentForm != null && _awayRecentForm != null;
            SelectedTeamType = TeamCampType.Home;
            TaskLoaderNotifier.Load(InitData);
        }

        private Task<IReadOnlyCollection<FootballMatchInfo>> InitData()
        {
            SetIsBusy(true);

            SelectedTeamId = SelectedTeamType == TeamCampType.Home ? _homeTeamId : _awayTeamId;

            var selectedRecentForm = SelectedTeamType == TeamCampType.Home ? _homeRecentForm : _awayRecentForm;

            if (selectedRecentForm?.Count > 0)
            {
                selectedRecentForm.ForEach(elem => elem.IsLastMatch = false);

                var lastMatch = selectedRecentForm.First();
                lastMatch.IsLastMatch = true;

                bool isDraw = lastMatch.HomeScore == lastMatch.AwayScore;
                bool isWin = lastMatch.HomeTeamId == SelectedTeamId ? lastMatch.HomeScore > lastMatch.AwayScore : lastMatch.HomeScore < lastMatch.AwayScore;

                LastMatchResult =
                    isDraw ?
                    MatchResultType.Draw :
                    isWin ? MatchResultType.Win : MatchResultType.Lose;
            }

            SelectedRecentForm = new ObservableList<FootballMatchInfo>(selectedRecentForm);

            SetIsBusy(false);

            return Task.FromResult(SelectedRecentForm as IReadOnlyCollection<FootballMatchInfo>);

            #endregion Methods
        }
    }
}