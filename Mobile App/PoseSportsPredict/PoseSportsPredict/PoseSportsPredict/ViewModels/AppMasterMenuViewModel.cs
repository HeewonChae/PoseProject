using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Auth.Models;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities.SQLite;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Common;
using PoseSportsPredict.ViewModels.Common.Detail;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.ViewModels.Football.Bookmark;
using PoseSportsPredict.Views;
using PoseSportsPredict.Views.Football;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ExternAuthentication;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels
{
    public class AppMasterMenuViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            MessagingCenter.Subscribe<SettingsViewModel, CoverageLanguage>(this,
                AppConfig.CULTURE_CHANGED_MSG, OnUpdateCultureInfo);

            var footballPage = ShinyHost.Resolve<FootballMainViewModel>().CoupledPage;

            SportsCategories = new ObservableCollection<Models.AppMenuDetailItem>
            {
                new AppMenuDetailItem
                {
                    Title = LocalizeString.Football,
                    IconSource = "img_football.png",
                    SourcePage = footballPage,
                    SportsType = SportsType.Football,
                }
            };

            BookmarkedLeauges = new BookmarkMenuListViewModel()
            {
                Title = LocalizeString.Favorite_Leagues,
                BookMarkType = BookMarkType.League,
            };

            BookmarkedTeams = new BookmarkMenuListViewModel()
            {
                Title = LocalizeString.Favorite_Teams,
                BookMarkType = BookMarkType.Team,
            };

            _curActiveSportsType = SportsType.Football;
            _ = RefrashBookmarkedLeague();
            _ = RefrashBookmarkedTeam();

            return true;
        }

        #endregion NavigableViewModel

        #region Services

        private IOAuthService _OAuthService;
        private IBookmarkService _bookmarkService;

        #endregion Services

        #region Fields

        private SportsType _curActiveSportsType;
        private ExternAuthUser _authUser;
        private ObservableCollection<AppMenuItem> _appMenuItems;
        private ObservableCollection<AppMenuDetailItem> _sportsCategories;
        private BookmarkMenuListViewModel _bookmarkedLeauges;
        private BookmarkMenuListViewModel _bookmarkedTeams;
        private DateTime _lastLoginTime;

        #endregion Fields

        #region Properties

        public ExternAuthUser AuthUser { get => _authUser; set => SetValue(ref _authUser, value); }
        public ObservableCollection<AppMenuItem> AppMenuItems { get => _appMenuItems; set => SetValue(ref _appMenuItems, value); }
        public ObservableCollection<AppMenuDetailItem> SportsCategories { get => _sportsCategories; set => SetValue(ref _sportsCategories, value); }
        public BookmarkMenuListViewModel BookmarkedLeauges { get => _bookmarkedLeauges; set => SetValue(ref _bookmarkedLeauges, value); }
        public BookmarkMenuListViewModel BookmarkedTeams { get => _bookmarkedTeams; set => SetValue(ref _bookmarkedTeams, value); }
        public DateTime LastLoginTime { get => _lastLoginTime; set => SetValue(ref _lastLoginTime, value); }

        #endregion Properties

        #region Commands

        public ICommand LogoutCommand { get => new RelayCommand(Logout); }

        private async void Logout()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            bool? isLogout = await MaterialDialog.Instance.ConfirmAsync(
                LocalizeString.Do_You_Want_Logout,
                LocalizeString.App_Title,
                LocalizeString.Ok,
                LocalizeString.Cancel,
                DialogConfiguration.DefaultAlterDialogConfiguration);

            if (isLogout.HasValue && isLogout.Value)
                await _OAuthService.Logout();

            SetIsBusy(false);
        }

        public ICommand SelectMyProfileCommand { get => new RelayCommand(SelectMyProfile); }

        private async void SelectMyProfile()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<MyProfileViewModel>());

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public AppMasterMenuViewModel(
            AppMasterMenuPage page,
            IOAuthService OAuthService,
            IBookmarkService bookmarkService) : base(page)
        {
            _OAuthService = OAuthService;
            _bookmarkService = bookmarkService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public void RefrashUserInfo()
        {
            AuthUser = _OAuthService.AuthenticatedUser;
            if (string.IsNullOrEmpty(AuthUser.PictureUrl))
            {
                AuthUser.PictureUrl = "ic_profile.png";
            }
        }

        public async Task RefrashBookmarkedTeam()
        {
            SetIsBusy(true);

            List<IBookmarkMenuItem> teams = null;

            switch (_curActiveSportsType)
            {
                case SportsType.Football:
                    var bookmarkedTeams = await _bookmarkService.GetAllBookmark<FootballTeamInfo>();
                    teams = new List<IBookmarkMenuItem>(bookmarkedTeams);
                    teams.Sort(ShinyHost.Resolve<StoredData_BasicComparer>());
                    break;
            }

            Debug.Assert(teams != null);
            BookmarkedTeams.Items = new ObservableCollection<IBookmarkMenuItem>(teams);

            SetIsBusy(false);
        }

        public async Task RefrashBookmarkedLeague()
        {
            SetIsBusy(true);

            List<IBookmarkMenuItem> leagues = null;

            switch (_curActiveSportsType)
            {
                case SportsType.Football:
                    var bookmarkedLeagues = await _bookmarkService.GetAllBookmark<FootballLeagueInfo>();
                    leagues = new List<IBookmarkMenuItem>(bookmarkedLeagues);
                    leagues.Sort(ShinyHost.Resolve<StoredData_BasicComparer>());
                    break;
            }

            Debug.Assert(leagues != null);

            BookmarkedLeauges.Items = new ObservableCollection<IBookmarkMenuItem>(leagues);

            SetIsBusy(false);
        }

        public void OnUpdateCultureInfo(object sender, CoverageLanguage cl)
        {
            // Menu
            BookmarkedLeauges.Title = LocalizeString.Favorite_Leagues;
            BookmarkedTeams.Title = LocalizeString.Favorite_Teams;

            // Football
            SportsCategories[0].Title = LocalizeString.Football;
            var footballMainPage = SportsCategories[0].SourcePage as FootballMainPage;
            footballMainPage.Children[0].Title = LocalizeString.Matches;
            footballMainPage.Children[1].Title = LocalizeString.Leagues;
            footballMainPage.Children[2].Title = LocalizeString.Bookmarks;
            footballMainPage.Children[3].Title = LocalizeString.Settings;
        }

        #endregion Methods
    }
}