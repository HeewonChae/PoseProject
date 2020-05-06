using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Team;
using Shiny;
using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xam.Plugin;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Team
{
    public class FootballTeamDetailViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            string message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.Team);
            MessagingCenter.Subscribe<BookmarkService, FootballTeamInfo>(this, message, (s, e) => this.TeamBookmarkMessageHandler(e));

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

            if (!(datas[0] is FootballTeamInfo teamInfo))
                return false;

            // Check Bookmark
            var bookmarkedTeam = await _bookmarkService.GetBookmark<FootballTeamInfo>(teamInfo.PrimaryKey);
            teamInfo.IsBookmarked = bookmarkedTeam?.IsBookmarked ?? false;

            TeamInfo = teamInfo;

            OverviewModel = ShinyHost.Resolve<FootballTeamDetailOverviewModel>().SetTeamInfo(teamInfo);
            FinishedMatchesViewModel = ShinyHost.Resolve<FootballTeamDetailFinishedMatchesViewModel>().SetTeamInfo(teamInfo);
            ScheduledMatchesViewModel = ShinyHost.Resolve<FootballTeamDetailScheduledMatchesViewModel>().SetTeamInfo(teamInfo); ;

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
        private INotificationService _notificationService;

        #endregion Services

        #region Fields

        private PopupMenu _popup = new PopupMenu();
        private bool _isMoreButtonVisible;
        private FootballTeamInfo _teamInfo;
        private int _selectedViewIndex;
        private FootballTeamDetailOverviewModel _overviewModel;
        private FootballTeamDetailFinishedMatchesViewModel _finishedMatchesViewModel;
        private FootballTeamDetailScheduledMatchesViewModel _scheduledMatchesViewModel;
        private List<BaseViewModel> _tabContents;

        #endregion Fields

        #region Properties

        public IList<string> PopupMenuList { get; set; }
        public bool IsMoreButtonVisible { get => _isMoreButtonVisible; set => SetValue(ref _isMoreButtonVisible, value); }
        public FootballTeamInfo TeamInfo { get => _teamInfo; set => SetValue(ref _teamInfo, value); }
        public int SelectedViewIndex { get => _selectedViewIndex; set => SetValue(ref _selectedViewIndex, value); }
        public FootballTeamDetailOverviewModel OverviewModel { get => _overviewModel; set => SetValue(ref _overviewModel, value); }
        public FootballTeamDetailFinishedMatchesViewModel FinishedMatchesViewModel { get => _finishedMatchesViewModel; set => SetValue(ref _finishedMatchesViewModel, value); }
        public FootballTeamDetailScheduledMatchesViewModel ScheduledMatchesViewModel { get => _scheduledMatchesViewModel; set => SetValue(ref _scheduledMatchesViewModel, value); }

        #endregion Properties

        #region Commands

        public ICommand TouchBackButtonCommand { get => new RelayCommand(TouchBackButton); }

        private async void TouchBackButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            string message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.Team);
            MessagingCenter.Unsubscribe<BookmarkService, FootballTeamInfo>(this, message);

            message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.Match);
            MessagingCenter.Unsubscribe<BookmarkService, FootballMatchInfo>(this, message);

            message = _notificationService.BuildNotificationMessage(SportsType.Football, NotificationType.MatchStart);
            MessagingCenter.Unsubscribe<NotificationService, NotificationInfo>(this, message);

            await PageSwitcher.PopNavPageAsync();

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand(TouchBookmarkButton); }

        private async void TouchBookmarkButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            // Add Bookmark
            if (TeamInfo.IsBookmarked)
                await _bookmarkService.RemoveBookmark<FootballTeamInfo>(TeamInfo, SportsType.Football, BookMarkType.Team);
            else
                await _bookmarkService.AddBookmark<FootballTeamInfo>(TeamInfo, SportsType.Football, BookMarkType.Team);

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
                LocalizeString.Add_Delete_Matches_Alarm,
            };

            _popup.BindingContext = this;
            _popup.SetBinding(PopupMenu.ItemsSourceProperty, "PopupMenuList");
            _popup.ShowPopup(menuButton);

            _popup.OnItemSelected += (item) =>
            {
                if (item.Equals(PopupMenuList[0]))
                {
                    if (_tabContents[SelectedViewIndex] is FootballTeamDetailFinishedMatchesViewModel finishedMatchesViewModel)
                        finishedMatchesViewModel.EditAlarmMode();
                    else if (_tabContents[SelectedViewIndex] is FootballTeamDetailScheduledMatchesViewModel scheduledMatchesViewModel)
                        scheduledMatchesViewModel.EditAlarmMode();
                }
            };

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballTeamDetailViewModel(
            FootballTeamDetailPage page
            , IBookmarkService bookmarkService
            , INotificationService notificationService) : base(page)
        {
            _bookmarkService = bookmarkService;
            _notificationService = notificationService;

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
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

        private void TeamBookmarkMessageHandler(FootballTeamInfo item)
        {
            if (TeamInfo.PrimaryKey.Equals(item.PrimaryKey))
            {
                TeamInfo.IsBookmarked = item.IsBookmarked;
                TeamInfo.OnPropertyChanged("IsBookmarked");
            }
        }

        private void MatchBookmarkMessageHandler(FootballMatchInfo item)
        {
            if (FinishedMatchesViewModel.Matches != null)
            {
                var foundItem = FinishedMatchesViewModel.Matches.FirstOrDefault(elem => elem.PrimaryKey.Equals(item.PrimaryKey));
                if (foundItem != null)
                {
                    foundItem.IsBookmarked = item.IsBookmarked;
                    foundItem.OnPropertyChanged("IsBookmarked");

                    return;
                }
            }

            if (ScheduledMatchesViewModel.Matches != null)
            {
                var foundItem = ScheduledMatchesViewModel.Matches.FirstOrDefault(elem => elem.PrimaryKey.Equals(item.PrimaryKey));
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
            if (FinishedMatchesViewModel.Matches != null)
            {
                var foundItem = FinishedMatchesViewModel.Matches.FirstOrDefault(elem => elem.PrimaryKey.Equals(item.Id.ToString()));
                if (foundItem != null)
                {
                    foundItem.IsAlarmed = item.IsAlarmed;
                    foundItem.OnPropertyChanged("IsAlarmed");

                    return;
                }
            }

            if (ScheduledMatchesViewModel.Matches != null)
            {
                var foundItem = ScheduledMatchesViewModel.Matches.FirstOrDefault(elem => elem.PrimaryKey.Equals(item.Id.ToString()));
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