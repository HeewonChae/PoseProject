using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Enum;
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
    public class FootballLeagueListViewModel : BaseViewModel, IExpandable, ITempletable
    {
        #region IExpandable

        private bool _expanded;
        private string _title;
        private string _titleLogo;

        public string Title { get => _title; set => SetValue(ref _title, value); }
        public string StateIcon => Expanded ? "ic_expanded.png" : "ic_collapsed.png";
        public string TitleLogo { get => _titleLogo; set => SetValue(ref _titleLogo, value); }

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged(nameof(Expanded));
                    OnPropertyChanged(nameof(StateIcon));
                }
            }
        }

        #endregion IExpandable

        #region Fields

        private IBookmarkService _bookmarkService;

        #endregion Fields

        #region Properties

        public bool IsRecommendLeagues;
        public MatchGroupType GroupType { get; set; }
        public AdsBannerType AdsBannerType { get; set; }
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

            // Add Bookmark
            if (leagueInfo.IsBookmarked)
                await _bookmarkService.RemoveBookmark<FootballLeagueInfo>(leagueInfo, SportsType.Football, BookMarkType.League);
            else
                await _bookmarkService.AddBookmark<FootballLeagueInfo>(leagueInfo, SportsType.Football, BookMarkType.League);

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