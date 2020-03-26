using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services.MessagingCenterMessageType;
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

            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), leagueInfo);

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand<FootballLeagueInfo>((e) => TouchBookmarkButton(e)); }

        private async void TouchBookmarkButton(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            leagueInfo.Order = 0;
            leagueInfo.StoredTime = DateTime.Now;
            leagueInfo.IsBookmarked = !leagueInfo.IsBookmarked;

            // Add Bookmark
            if (leagueInfo.IsBookmarked)
                await ShinyHost.Resolve<ISQLiteService>().InsertOrUpdateAsync<FootballLeagueInfo>(leagueInfo);
            else
                await ShinyHost.Resolve<ISQLiteService>().DeleteAsync<FootballLeagueInfo>(leagueInfo.PrimaryKey);

            MessagingCenter.Send(this, FootballMessageType.Update_Bookmark_League.ToString(), leagueInfo);

            var message = leagueInfo.IsBookmarked ? LocalizeString.Set_Bookmark : LocalizeString.Delete_Bookmark;
            UserDialogs.Instance.Toast(message);

            leagueInfo.OnPropertyChanged("IsBookmarked");

            SetIsBusy(false);
        }

        #endregion Commands
    }
}