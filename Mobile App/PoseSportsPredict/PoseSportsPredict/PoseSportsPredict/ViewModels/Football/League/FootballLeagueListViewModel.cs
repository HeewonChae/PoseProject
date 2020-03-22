using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
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

        private void SelectLeague(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            //await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), leagueInfo);

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand<FootballLeagueInfo>((e) => TouchBookmarkButton(e)); }

        private void TouchBookmarkButton(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var message = leagueInfo.IsBookmarked ? LocalizeString.Delete_Bookmark : LocalizeString.Set_Bookmark;
            UserDialogs.Instance.Toast(message);

            leagueInfo.IsBookmarked = !leagueInfo.IsBookmarked;
            leagueInfo.OnPropertyChanged("IsBookmarked");

            SetIsBusy(false);
        }

        #endregion Commands
    }
}