using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using Shiny;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Football.League
{
    public class FootballLeagueListViewModel : BaseViewModel
    {
        #region Fields

        private IBookmarkService _bookmarkService;

        #endregion Fields

        #region Properties

        public ObservableCollection<FootballLeagueInfo> Leagues { get; set; }

        #endregion Properties

        #region Commands

        public ICommand SelectLeagueCommand { get => new RelayCommand<FootballLeagueInfo>((e) => SelectLeague(e)); }

        private async void SelectLeague(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), leagueInfo);

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand<FootballLeagueInfo>((e) => TouchBookmarkButton(e)); }

        private async void TouchBookmarkButton(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            leagueInfo.Order = 0;
            leagueInfo.StoredTime = DateTime.UtcNow;
            leagueInfo.IsBookmarked = !leagueInfo.IsBookmarked;

            // Add Bookmark
            if (leagueInfo.IsBookmarked)
                await _bookmarkService.AddBookmark<FootballLeagueInfo>(leagueInfo, SportsType.Football, BookMarkType.League);
            else
                await _bookmarkService.RemoveBookmark<FootballLeagueInfo>(leagueInfo, SportsType.Football, BookMarkType.League);

            var message = leagueInfo.IsBookmarked ? LocalizeString.Set_Bookmark : LocalizeString.Delete_Bookmark;
            UserDialogs.Instance.Toast(message);

            leagueInfo.OnPropertyChanged("IsBookmarked");

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballLeagueListViewModel(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        #endregion Constructors
    }
}