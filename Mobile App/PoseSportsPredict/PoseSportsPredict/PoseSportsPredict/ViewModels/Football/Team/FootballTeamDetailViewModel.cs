using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Team;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

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
            TeamInfo = bookmarkedTeam ?? teamInfo;

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
        }

        #endregion NavigableViewModel

        #region Services

        private IBookmarkService _bookmarkService;

        #endregion Services

        #region Fields

        private FootballTeamInfo _teamInfo;
        private int _selectedViewIndex;

        #endregion Fields

        #region Properties

        public FootballTeamInfo TeamInfo { get => _teamInfo; set => SetValue(ref _teamInfo, value); }
        public int SelectedViewIndex { get => _selectedViewIndex; set => SetValue(ref _selectedViewIndex, value); }

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

        public ICommand SwipeLeftViewSwitcherCommad { get => new RelayCommand(SwipeLeftViewSwitcher); }

        private void SwipeLeftViewSwitcher()
        {
            if (SelectedViewIndex < 3)
                SelectedViewIndex++;
        }

        public ICommand SwipeRightViewSwitcherCommad { get => new RelayCommand(SwipeRightViewSwitcher); }

        private void SwipeRightViewSwitcher()
        {
            if (SelectedViewIndex > 0)
                SelectedViewIndex--;
        }

        #endregion Commands

        #region Constructors

        public FootballTeamDetailViewModel(
            FootballTeamDetailPage page
            , IBookmarkService bookmarkService) : base(page)
        {
            _bookmarkService = bookmarkService;

            CoupledPage.Appearing += (s, e) => OnAppearing();
        }

        #endregion Constructors
    }
}