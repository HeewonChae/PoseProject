using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.League.Detail;
using Sharpnado.Presentation.Forms.CustomViews.Tabs;
using Shiny;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.League.Detail
{
    public class FootballLeagueDetailViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            if (datas == null)
                return false;

            if (!(datas[0] is FootballLeagueInfo leagueInfo))
                return false;

            // Check Bookmark
            var bookmarkedLeague = await _bookmarkService.GetBookmark<FootballLeagueInfo>(leagueInfo.PrimaryKey);
            leagueInfo.IsBookmarked = bookmarkedLeague?.IsBookmarked ?? false;

            LeagueInfo = leagueInfo;

            TabContents = new ObservableCollection<BaseViewModel>
            {
                ShinyHost.Resolve<FootballLeagueDetailOverviewModel>(),
                ShinyHost.Resolve<FootballLeagueDetailFinishedMatchesViewModel>(),
                ShinyHost.Resolve<FootballLeagueDetailScheduledMatchesViewModel>(),
            };

            SelectedViewIndex = 0;

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

        private FootballLeagueInfo _leagueInfo;
        private int _selectedViewIndex;
        private ObservableCollection<BaseViewModel> _tabContents;

        #endregion Fields

        #region Properties

        public FootballLeagueInfo LeagueInfo { get => _leagueInfo; set => SetValue(ref _leagueInfo, value); }
        public int SelectedViewIndex { get => _selectedViewIndex; set => SetValue(ref _selectedViewIndex, value); }
        public ObservableCollection<BaseViewModel> TabContents { get => _tabContents; set => SetValue(ref _tabContents, value); }

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

            LeagueInfo.Order = 0;
            LeagueInfo.StoredTime = DateTime.UtcNow;
            LeagueInfo.IsBookmarked = !LeagueInfo.IsBookmarked;

            // Add Bookmark
            if (LeagueInfo.IsBookmarked)
                await _bookmarkService.AddBookmark<FootballLeagueInfo>(LeagueInfo, SportsType.Football, BookMarkType.League);
            else
                await _bookmarkService.RemoveBookmark<FootballLeagueInfo>(LeagueInfo, SportsType.Football, BookMarkType.League);

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
            , IBookmarkService bookmarkService) : base(page)
        {
            _bookmarkService = bookmarkService;

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }

            this.CoupledPage.FindByName<TabHostView>("_tabHost").SelectedTabIndexChanged += (s, e) =>
            {
                var tabContents = this.CoupledPage.FindByName<CarouselView>("_tabContents");
                tabContents.CurrentItem = TabContents[SelectedViewIndex];
            };

            this.CoupledPage.FindByName<CarouselView>("_tabContents").CurrentItemChanged += (s, e) =>
            {
                SelectedViewIndex = TabContents.IndexOf(e.CurrentItem as BaseViewModel);

                var curContent = TabContents[SelectedViewIndex];
                curContent.OnAppearing();
            };
        }

        #endregion Constructors
    }
}