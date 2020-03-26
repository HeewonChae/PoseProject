using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services.MessagingCenterMessageType;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.League.Detail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.League.Detail
{
    public class FootballLeagueDetailViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override async Task<bool> OnInitializeViewAsync(params object[] datas)
        {
            if (datas == null)
                return false;

            if (!(datas[0] is FootballLeagueInfo leagueInfo))
                return false;

            // Check Bookmark
            var bookmarkedLeague = await _sqliteService.SelectAsync<FootballLeagueInfo>(leagueInfo.PrimaryKey);
            LeagueInfo = bookmarkedLeague ?? leagueInfo;

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

        private FootballLeagueInfo _leagueInfo;
        private int _selectedViewIndex;

        #endregion Fields

        #region Properties

        public FootballLeagueInfo LeagueInfo { get => _leagueInfo; set => SetValue(ref _leagueInfo, value); }
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

            LeagueInfo.Order = 0;
            LeagueInfo.StoredTime = DateTime.Now;
            LeagueInfo.IsBookmarked = !LeagueInfo.IsBookmarked;

            // Add Bookmark
            if (LeagueInfo.IsBookmarked)
                await _sqliteService.InsertOrUpdateAsync<FootballLeagueInfo>(LeagueInfo);
            else
                await _sqliteService.DeleteAsync<FootballLeagueInfo>(LeagueInfo.PrimaryKey);

            MessagingCenter.Send(this, FootballMessageType.Update_Bookmark_League.ToString(), LeagueInfo);

            var message = LeagueInfo.IsBookmarked ? LocalizeString.Set_Bookmark : LocalizeString.Delete_Bookmark;
            UserDialogs.Instance.Toast(message);

            LeagueInfo.OnPropertyChanged("IsBookmarked");

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

        public FootballLeagueDetailViewModel(
            FootballLeagueDetailPage page
            , ISQLiteService sqliteService) : base(page)
        {
            _sqliteService = sqliteService;

            CoupledPage.Appearing += (s, e) => OnAppearing();
        }

        #endregion Constructors
    }
}