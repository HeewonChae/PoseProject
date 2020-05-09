using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Team;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using ComboBox = Syncfusion.XForms.ComboBox;

namespace PoseSportsPredict.ViewModels.Football.Standings
{
    public class FootballStandingsViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            Ddl_selectedIndex = -1;

            TaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballStandingsInfo>>();

            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private Dictionary<string, List<FootballStandingsInfo>> _standingsByGroup;

        private bool _isViewForm;
        private int _ddl_selectedIndex;
        private ObservableList<string> _ddl_Groups;
        private TaskLoaderNotifier<IReadOnlyCollection<FootballStandingsInfo>> _taskLoaderNotifier;
        private ObservableCollection<FootballStandingsInfo> _standingsInfos;
        private ObservableCollection<FootballStandingsDescription> _standingsDescription;

        #endregion Fields

        #region Properties

        public bool IsViewForm { get => _isViewForm; set => SetValue(ref _isViewForm, value); }
        public int Ddl_selectedIndex { get => _ddl_selectedIndex; set => SetValue(ref _ddl_selectedIndex, value); }
        public ObservableList<string> Ddl_Groups { get => _ddl_Groups; set => SetValue(ref _ddl_Groups, value); }
        public TaskLoaderNotifier<IReadOnlyCollection<FootballStandingsInfo>> TaskLoaderNotifier { get => _taskLoaderNotifier; set => SetValue(ref _taskLoaderNotifier, value); }
        public ObservableCollection<FootballStandingsInfo> StandingsInfos { get => _standingsInfos; set => SetValue(ref _standingsInfos, value); }
        public ObservableCollection<FootballStandingsDescription> StandingsDescription { get => _standingsDescription; set => SetValue(ref _standingsDescription, value); }

        #endregion Properties

        #region Commands

        public ICommand ViewFormClickCommand { get => new RelayCommand(ViewFormClick); }

        private void ViewFormClick()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            IsViewForm = !IsViewForm;
            TaskLoaderNotifier.Load(InitData);

            SetIsBusy(false);
        }

        public ICommand TeamNameClickCommand { get => new RelayCommand<FootballStandingsInfo>(e => TeamNameClick(e)); }

        private async void TeamNameClick(FootballStandingsInfo standings)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>(), standings.TeamInfo);

            SetIsBusy(false);
        }

        public ICommand LeagueSelectionChangedCommand { get => new RelayCommand<ComboBox.SelectionChangedEventArgs>((e) => LeagueSelectionChanged(e)); }

        private void LeagueSelectionChanged(ComboBox.SelectionChangedEventArgs arg)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            TaskLoaderNotifier.Load(InitData);

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballStandingsViewModel()
        {
            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public void SetMember(Dictionary<string, List<FootballStandingsInfo>> standingsByGroup)
        {
            _standingsByGroup = standingsByGroup;

            Ddl_Groups = new ObservableList<string>(
                _standingsByGroup.Select(elem => elem.Key).OrderBy(elem => elem));

            Ddl_selectedIndex = 0;

            TaskLoaderNotifier.Load(InitData);
        }

        public async Task<IReadOnlyCollection<FootballStandingsInfo>> InitData()
        {
            if (_standingsByGroup.Count == 0)
                return null;

            await Task.Delay(300);

            var standingsInfos = _standingsByGroup.ElementAt(Ddl_selectedIndex).Value.OrderBy(elem => elem.Rank).ToList();

            // Description Group
            var descGroups = standingsInfos.GroupBy(elem => elem.Description);
            int positiveDescCnt = 0;
            int negativeDescCnt = 0;
            int neutralDescCnt = 0;

            StandingsDescription = new ObservableCollection<FootballStandingsDescription>();
            foreach (var descGroup in descGroups)
            {
                if (string.IsNullOrEmpty(descGroup.Key))
                    continue;

                var descCategoryType = StandginsDescription.GetDescCategory(descGroup.Key);
                Color descColor;
                if (descCategoryType == StandingsDescCategoryType.Positive)
                {
                    descColor = StandingsRankColor.GetRankColor(descCategoryType, positiveDescCnt);
                    positiveDescCnt++;
                }
                else if (descCategoryType == StandingsDescCategoryType.Negative)
                {
                    descColor = StandingsRankColor.GetRankColor(descCategoryType, negativeDescCnt);
                    negativeDescCnt++;
                }
                else if (descCategoryType == StandingsDescCategoryType.Neutral)
                {
                    descColor = StandingsRankColor.GetRankColor(descCategoryType, neutralDescCnt);
                    neutralDescCnt++;
                }
                else
                {
                    descColor = StandingsRankColor.GetRankColor(StandingsDescCategoryType.None, 0);
                }

                foreach (var standings in descGroup)
                {
                    standings.RankColor = descColor;
                }

                StandingsDescription.Add(new FootballStandingsDescription
                {
                    Description = descGroup.Key,
                    DescColor = descColor,
                });
            }

            StandingsInfos = new ObservableCollection<FootballStandingsInfo>(standingsInfos);

            return StandingsInfos;
        }

        #endregion Methods
    }
}