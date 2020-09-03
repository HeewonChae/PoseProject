using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Enum;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.League.Detail;
using Sharpnado.Presentation.Forms.CustomViews.Tabs;
using Shiny;
using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xam.Plugin;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.League.Detail
{
    public class FootballLeagueDetailViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            string message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.League);
            MessagingCenter.Subscribe<BookmarkService, FootballLeagueInfo>(this, message, (s, e) => this.LeagueBookmarkMessageHandler(e));

            message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.Match);
            MessagingCenter.Subscribe<BookmarkService, FootballMatchInfo>(this, message, (s, e) => MatchBookmarkMessageHandler(e));

            message = _notificationService.BuildNotificationMessage(SportsType.Football, NotificationType.MatchStart);
            MessagingCenter.Subscribe<NotificationService, NotificationInfo>(this, message, (s, e) => NotificationMessageHandler(e));

            return true;
        }

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
            IsMoreButtonVisible = false;

            return true;
        }

        public override void OnPagePoped()
        {
            string message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.League);
            MessagingCenter.Unsubscribe<BookmarkService, FootballLeagueInfo>(this, message);

            message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.Match);
            MessagingCenter.Unsubscribe<BookmarkService, FootballMatchInfo>(this, message);

            message = _notificationService.BuildNotificationMessage(SportsType.Football, NotificationType.MatchStart);
            MessagingCenter.Unsubscribe<NotificationService, NotificationInfo>(this, message);
        }

        public override void OnAppearing(params object[] datas)
        {
            _tabContents[SelectedViewIndex].OnAppearing();
        }

        public override void OnDisAppearing(params object[] datas)
        {
        }

        #endregion NavigableViewModel

        #region Services

        private IBookmarkService _bookmarkService;
        private INotificationService _notificationService;

        #endregion Services

        #region Fields

        private PopupMenu _popup = new PopupMenu();
        private bool _isMoreButtonVisible;
        private FootballLeagueInfo _leagueInfo;
        private int _selectedViewIndex;
        private FootballLeagueDetailOverviewModel _overviewModel;
        private FootballLeagueDetailFinishedMatchesViewModel _finishedMatchesViewModel;
        private FootballLeagueDetailScheduledMatchesViewModel _scheduledMatchesViewModel;
        private List<BaseViewModel> _tabContents;

        #endregion Fields

        #region Properties

        public IList<string> PopupMenuList { get; set; }
        public bool IsMoreButtonVisible { get => _isMoreButtonVisible; set => SetValue(ref _isMoreButtonVisible, value); }
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
            if (IsPageSwitched)
                return;

            SetIsPageSwitched(true);

            await PageSwitcher.PopNavPageAsync();
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand(TouchBookmarkButton); }

        private async void TouchBookmarkButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            // Add Bookmark
            if (LeagueInfo.IsBookmarked)
                await _bookmarkService.RemoveBookmark<FootballLeagueInfo>(LeagueInfo, SportsType.Football, BookMarkType.League);
            else
                await _bookmarkService.AddBookmark<FootballLeagueInfo>(LeagueInfo, SportsType.Football, BookMarkType.League);

            SetIsBusy(false);
        }

        public ICommand MoreButtonCommand { get => new RelayCommand(MoreButton); }

        private void MoreButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var menuButton = CoupledPage.FindByName<ContentView>("_moreButton");

            PopupMenuList = new List<string>()
            {
                LocalizeString.Edit_Bookmark,
                LocalizeString.Expand_All_Matches,
                LocalizeString.Collapse_All_Matches,
            };

            _popup.BindingContext = this;
            _popup.SetBinding(PopupMenu.ItemsSourceProperty, "PopupMenuList");
            _popup.ShowPopup(menuButton);

            _popup.OnItemSelected += (item) =>
            {
                if (item.Equals(PopupMenuList[0]))
                {
                    if (_tabContents[SelectedViewIndex] is FootballLeagueDetailFinishedMatchesViewModel finishedMatchesViewModel)
                        finishedMatchesViewModel.AlarmEditMode();
                    else if (_tabContents[SelectedViewIndex] is FootballLeagueDetailScheduledMatchesViewModel scheduledMatchesViewModel)
                        scheduledMatchesViewModel.AlarmEditMode();
                }
                else if (item.Equals(PopupMenuList[1]))
                {
                    if (_tabContents[SelectedViewIndex] is FootballLeagueDetailFinishedMatchesViewModel finishedMatchesViewModel)
                        finishedMatchesViewModel.ExpandAllMatches();
                    else if (_tabContents[SelectedViewIndex] is FootballLeagueDetailScheduledMatchesViewModel scheduledMatchesViewModel)
                        scheduledMatchesViewModel.ExpandAllMatches();
                }
                else if (item.Equals(PopupMenuList[2]))
                {
                    if (_tabContents[SelectedViewIndex] is FootballLeagueDetailFinishedMatchesViewModel finishedMatchesViewModel)
                        finishedMatchesViewModel.CollapseAllMatches();
                    else if (_tabContents[SelectedViewIndex] is FootballLeagueDetailScheduledMatchesViewModel scheduledMatchesViewModel)
                        scheduledMatchesViewModel.CollapseAllMatches();
                }
            };

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballLeagueDetailViewModel(
            FootballLeagueDetailPage page
            , IBookmarkService bookmarkService
            , INotificationService notificationService) : base(page)
        {
            _bookmarkService = bookmarkService;
            _notificationService = notificationService;

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
                //CoupledPage.Disappearing += (s, e) => OnDisAppearing();
            }

            this.CoupledPage.FindByName<SfTabView>("_tabView").SelectionChanged
                += (s, e) =>
                {
                    if (SelectedViewIndex == 1 || SelectedViewIndex == 2)
                        IsMoreButtonVisible = true;
                    else
                        IsMoreButtonVisible = false;

                    _tabContents[SelectedViewIndex].OnAppearing();
                };
        }

        #endregion Constructors

        #region Methods

        private void LeagueBookmarkMessageHandler(FootballLeagueInfo item)
        {
            if (LeagueInfo.PrimaryKey == item.PrimaryKey)
            {
                LeagueInfo.IsBookmarked = item.IsBookmarked;
                LeagueInfo.OnPropertyChanged("IsBookmarked");
            }
        }

        private void MatchBookmarkMessageHandler(FootballMatchInfo item)
        {
            if (FinishedMatchesViewModel._matchList != null)
            {
                var foundItem = FinishedMatchesViewModel._matchList.FirstOrDefault(elem => elem.PrimaryKey.Equals(item.PrimaryKey));
                if (foundItem != null)
                {
                    foundItem.IsBookmarked = item.IsBookmarked;
                    foundItem.OnPropertyChanged("IsBookmarked");

                    return;
                }
            }

            if (ScheduledMatchesViewModel._matchList != null)
            {
                var foundItem = ScheduledMatchesViewModel._matchList.FirstOrDefault(elem => elem.PrimaryKey.Equals(item.PrimaryKey));
                if (foundItem != null)
                {
                    foundItem.IsBookmarked = item.IsBookmarked;
                    foundItem.OnPropertyChanged("IsBookmarked");

                    return;
                }
            }
        }

        private void NotificationMessageHandler(NotificationInfo item)
        {
            if (FinishedMatchesViewModel._matchList != null)
            {
                var foundItem = FinishedMatchesViewModel._matchList.FirstOrDefault(elem => elem.PrimaryKey.Equals(item.Id.ToString()));
                if (foundItem != null)
                {
                    foundItem.IsAlarmed = item.IsAlarmed;
                    foundItem.OnPropertyChanged("IsAlarmed");

                    return;
                }
            }

            if (ScheduledMatchesViewModel._matchList != null)
            {
                var foundItem = ScheduledMatchesViewModel._matchList.FirstOrDefault(elem => elem.PrimaryKey.Equals(item.Id.ToString()));
                if (foundItem != null)
                {
                    foundItem.IsAlarmed = item.IsAlarmed;
                    foundItem.OnPropertyChanged("IsAlarmed");

                    return;
                }
            }
        }

        #endregion Methods
    }
}