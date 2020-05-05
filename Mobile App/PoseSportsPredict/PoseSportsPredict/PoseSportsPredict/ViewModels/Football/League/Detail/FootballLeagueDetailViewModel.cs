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
using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
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

            OverviewModel = ShinyHost.Resolve<FootballLeagueDetailOverviewModel>().SetLeagueInfo(leagueInfo);
            FinishedMatchesViewModel = ShinyHost.Resolve<FootballLeagueDetailFinishedMatchesViewModel>().SetLeagueInfo(leagueInfo);
            ScheduledMatchesViewModel = ShinyHost.Resolve<FootballLeagueDetailScheduledMatchesViewModel>().SetLeagueInfo(leagueInfo); ;

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

        private FootballLeagueInfo _leagueInfo;
        private int _selectedViewIndex;
        private FootballLeagueDetailOverviewModel _overviewModel;
        private FootballLeagueDetailFinishedMatchesViewModel _finishedMatchesViewModel;
        private FootballLeagueDetailScheduledMatchesViewModel _scheduledMatchesViewModel;
        private List<BaseViewModel> _tabContents;

        #endregion Fields

        #region Properties

        public FootballLeagueInfo LeagueInfo { get => _leagueInfo; set => SetValue(ref _leagueInfo, value); }
        public int SelectedViewIndex { get => _selectedViewIndex; set => SetValue(ref _selectedViewIndex, value); }
        public FootballLeagueDetailOverviewModel OverviewModel { get => _overviewModel; set => SetValue(ref _overviewModel, value); }
        public FootballLeagueDetailFinishedMatchesViewModel FinishedMatchesViewModel { get => _finishedMatchesViewModel; set => SetValue(ref _finishedMatchesViewModel, value); }
        public FootballLeagueDetailScheduledMatchesViewModel ScheduledMatchesViewModel { get => _scheduledMatchesViewModel; set => SetValue(ref _scheduledMatchesViewModel, value); }

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

            this.CoupledPage.FindByName<SfTabView>("_tabView").SelectionChanged
                += (s, e) => _tabContents[SelectedViewIndex].OnAppearing();
        }

        #endregion Constructors
    }
}