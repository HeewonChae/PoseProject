using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services.MessagingCenterMessageType;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Team;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Team
{
    public class FootballTeamDetailViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override async Task<bool> OnInitializeViewAsync(params object[] datas)
        {
            if (datas == null)
                return false;

            if (!(datas[0] is FootballTeamInfo teamInfo))
                return false;

            // Check Bookmark
            var bookmarkedTeam = await _sqliteService.SelectAsync<FootballTeamInfo>(teamInfo.PrimaryKey);
            TeamInfo = bookmarkedTeam ?? teamInfo;

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
        }

        #endregion NavigableViewModel

        #region Services

        private ISQLiteService _sqliteService;

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

            await PageSwitcher.PopModalAsync();

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand(TouchBookmarkButton); }

        private async void TouchBookmarkButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            TeamInfo.Order = 0;
            TeamInfo.StoredTime = DateTime.Now;
            TeamInfo.IsBookmarked = !TeamInfo.IsBookmarked;

            // Add Bookmark
            if (TeamInfo.IsBookmarked)
                await _sqliteService.InsertOrUpdateAsync<FootballTeamInfo>(TeamInfo);
            else
                await _sqliteService.DeleteAsync<FootballTeamInfo>(TeamInfo.PrimaryKey);

            MessagingCenter.Send(this, FootballMessageType.Update_Bookmark_Team.ToString(), TeamInfo);

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
            , ISQLiteService sqliteService) : base(page)
        {
            _sqliteService = sqliteService;

            CoupledPage.Appearing += (s, e) => OnAppearing();
        }

        #endregion Constructors
    }
}