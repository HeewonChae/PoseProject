using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Team;
using Shiny;
using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Team
{
    public class FootballTeamDetailViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            if (datas == null)
                return false;

            if (!(datas[0] is FootballTeamInfo teamInfo))
                return false;

            // Check Bookmark
            var bookmarkedTeam = await _bookmarkService.GetBookmark<FootballTeamInfo>(teamInfo.PrimaryKey);
            teamInfo.IsBookmarked = bookmarkedTeam?.IsBookmarked ?? false;

            TeamInfo = teamInfo;

            OverviewModel = ShinyHost.Resolve<FootballTeamDetailOverviewModel>().SetTeamInfo(teamInfo);
            FinishedMatchesViewModel = ShinyHost.Resolve<FootballTeamDetailFinishedMatchesViewModel>().SetTeamInfo(teamInfo);
            ScheduledMatchesViewModel = ShinyHost.Resolve<FootballTeamDetailScheduledMatchesViewModel>().SetTeamInfo(teamInfo); ;

            _tabContents = new List<BaseViewModel>
            {
                OverviewModel,
                FinishedMatchesViewModel,
                ScheduledMatchesViewModel,
            };

            SelectedViewIndex = 0;

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            _tabContents[SelectedViewIndex].OnAppearing();
        }

        #endregion NavigableViewModel

        #region Services

        private IBookmarkService _bookmarkService;

        #endregion Services

        #region Fields

        private FootballTeamInfo _teamInfo;
        private int _selectedViewIndex;
        private FootballTeamDetailOverviewModel _overviewModel;
        private FootballTeamDetailFinishedMatchesViewModel _finishedMatchesViewModel;
        private FootballTeamDetailScheduledMatchesViewModel _scheduledMatchesViewModel;
        private List<BaseViewModel> _tabContents;

        #endregion Fields

        #region Properties

        public FootballTeamInfo TeamInfo { get => _teamInfo; set => SetValue(ref _teamInfo, value); }
        public int SelectedViewIndex { get => _selectedViewIndex; set => SetValue(ref _selectedViewIndex, value); }
        public FootballTeamDetailOverviewModel OverviewModel { get => _overviewModel; set => SetValue(ref _overviewModel, value); }
        public FootballTeamDetailFinishedMatchesViewModel FinishedMatchesViewModel { get => _finishedMatchesViewModel; set => SetValue(ref _finishedMatchesViewModel, value); }
        public FootballTeamDetailScheduledMatchesViewModel ScheduledMatchesViewModel { get => _scheduledMatchesViewModel; set => SetValue(ref _scheduledMatchesViewModel, value); }

        #endregion Properties

        #region Commands

        public ICommand TouchBackButtonCommand { get => new RelayCommand(TouchBackButton); }

        private async void TouchBackButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PopNavPageAsync();

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand(TouchBookmarkButton); }

        private async void TouchBookmarkButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            TeamInfo.Order = 0;
            TeamInfo.StoredTime = DateTime.UtcNow;
            TeamInfo.IsBookmarked = !TeamInfo.IsBookmarked;

            // Add Bookmark
            if (TeamInfo.IsBookmarked)
                await _bookmarkService.AddBookmark<FootballTeamInfo>(TeamInfo, SportsType.Football, BookMarkType.Team);
            else
                await _bookmarkService.RemoveBookmark<FootballTeamInfo>(TeamInfo, SportsType.Football, BookMarkType.Team);

            var message = TeamInfo.IsBookmarked ? LocalizeString.Set_Bookmark : LocalizeString.Delete_Bookmark;
            UserDialogs.Instance.Toast(message);

            TeamInfo.OnPropertyChanged("IsBookmarked");

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballTeamDetailViewModel(
            FootballTeamDetailPage page
            , IBookmarkService bookmarkService) : base(page)
        {
            _bookmarkService = bookmarkService;

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }

            this.CoupledPage.FindByName<SfTabView>("_tabView").SelectionChanged
                += (s, e) => _tabContents[SelectedViewIndex].OnAppearing();
        }

        #endregion Constructors
    }
}