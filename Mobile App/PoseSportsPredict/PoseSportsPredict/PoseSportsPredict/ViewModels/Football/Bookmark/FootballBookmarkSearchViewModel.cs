using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Utilities.SQLite;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.ViewModels.Football.Team;
using PoseSportsPredict.Views.Football.Bookmark;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarkSearchViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            BookmarkSearchTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<ISQLiteStorable>>();

            return true;
        }

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            if (_recentSearchList == null)
            {
                _recentSearchList = await _sqliteService.SelectAllAsync<FootballRecentSearch>();
                _recentSearchList.Sort(ShinyHost.Resolve<StoredData_InverseDateComparer>());
                RecentSearches = new ObservableCollection<FootballRecentSearch>(_recentSearchList);
            }

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            IsSearching = false;

            var searchBar = this.CoupledPage.FindByName<SearchBar>("_searchBar");
            searchBar.Text = string.Empty;
        }

        #endregion NavigableViewModel

        #region Field

        private ISQLiteService _sqliteService;
        private IBookmarkService _bookmarkService;
        private TaskLoaderNotifier<IReadOnlyCollection<ISQLiteStorable>> _bookmarkSearchTaskLoaderNotifier;
        private List<FootballRecentSearch> _recentSearchList;
        private ObservableCollection<FootballRecentSearch> _recentSearches;
        private ObservableCollection<FootballMatchInfo> _searchedMatches;
        private ObservableCollection<FootballLeagueInfo> _searchedLeagues;
        private ObservableCollection<FootballTeamInfo> _searchedTeams;
        private string _searchText;
        private bool _isSearching;

        #endregion Field

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<ISQLiteStorable>> BookmarkSearchTaskLoaderNotifier { get => _bookmarkSearchTaskLoaderNotifier; set => SetValue(ref _bookmarkSearchTaskLoaderNotifier, value); }
        public ObservableCollection<FootballRecentSearch> RecentSearches { get => _recentSearches; set => SetValue(ref _recentSearches, value); }
        public ObservableCollection<FootballMatchInfo> SearchedMatches { get => _searchedMatches; set => SetValue(ref _searchedMatches, value); }
        public ObservableCollection<FootballLeagueInfo> SearchedLeagues { get => _searchedLeagues; set => SetValue(ref _searchedLeagues, value); }
        public ObservableCollection<FootballTeamInfo> SearchedTeams { get => _searchedTeams; set => SetValue(ref _searchedTeams, value); }
        public bool IsSearching { get => _isSearching; set => SetValue(ref _isSearching, value); }

        #endregion Properties

        #region Commands

        public ICommand TouchBackButtonCommand { get => new RelayCommand(TouchBackButton); }

        private async void TouchBackButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PopPopupAsync();

            SetIsBusy(false);
        }

        public ICommand SelectSearchKeywordCommand { get => new RelayCommand<FootballRecentSearch>(e => SelectSearchKeyword(e)); }

        private void SelectSearchKeyword(FootballRecentSearch item)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var searchBar = this.CoupledPage.FindByName<SearchBar>("_searchBar");
            searchBar.Text = item.PrimaryKey;
            searchBar.Focus();

            SetIsBusy(false);
        }

        public ICommand DeleteSearchKeywordCommand { get => new RelayCommand<FootballRecentSearch>(e => DeleteSearchKeyword(e)); }

        private async void DeleteSearchKeyword(FootballRecentSearch item)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await DeleteSearchKeywordAsync(item);

            SetIsBusy(false);
        }

        public ICommand SearchBarTextChangedCommand { get => new RelayCommand<TextChangedEventArgs>((e) => SearchBarTextChanged(e)); }

        private async void SearchBarTextChanged(TextChangedEventArgs eventArgs)
        {
            _searchText = eventArgs.NewTextValue;

            await Task.Delay(700);

            if (_searchText == eventArgs.NewTextValue)
                BookmarkSearchTaskLoaderNotifier.Load(() => SearchBookmarksAsync(_searchText));
        }

        public ICommand SelectMatchCommand { get => new RelayCommand<FootballMatchInfo>((e) => SelectMatch(e)); }

        private async void SelectMatch(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PopPopupAsync();
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);

            await InsertSearchKeywordAsync(matchInfo.HomeName, matchInfo.HomeLogo);
            await InsertSearchKeywordAsync(matchInfo.AwayName, matchInfo.AwayLogo);

            SetIsBusy(false);
        }

        public ICommand SelectLeagueCommand { get => new RelayCommand<FootballLeagueInfo>((e) => SelectLeague(e)); }

        private async void SelectLeague(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PopPopupAsync();
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), leagueInfo);

            await InsertSearchKeywordAsync(leagueInfo.LeagueName, leagueInfo.LeagueLogo);

            SetIsBusy(false);
        }

        public ICommand SelectTeamCommand { get => new RelayCommand<FootballTeamInfo>((e) => SelectTeam(e)); }

        private async void SelectTeam(FootballTeamInfo teamInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PopPopupAsync();
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>(), teamInfo);

            await InsertSearchKeywordAsync(teamInfo.TeamName, teamInfo.TeamLogo);

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballBookmarkSearchViewModel(
            FootballBookmarkSearchPage page,
            ISQLiteService sqliteService,
            IBookmarkService bookmarkService) : base(page)
        {
            _sqliteService = sqliteService;
            _bookmarkService = bookmarkService;

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }

        #endregion Constructors

        #region Methods

        private async Task<IReadOnlyCollection<ISQLiteStorable>> SearchBookmarksAsync(string searchText)
        {
            await Task.Delay(300);

            if (!string.IsNullOrEmpty(searchText))
            {
                List<ISQLiteStorable> searchResult = new List<ISQLiteStorable>();
                IsSearching = true;

                var bookmarkedMatches = await _bookmarkService.GetAllBookmark<FootballMatchInfo>();
                var searchedMatches = bookmarkedMatches.Where(elem => elem.LeagueName.ToLower().Contains(searchText.ToLower())
                            || elem.League_CountryName.ToLower().Contains(searchText.ToLower())
                            || elem.Home_CountryName.ToLower().Contains(searchText.ToLower())
                            || elem.Away_CountryName.ToLower().Contains(searchText.ToLower())
                            || elem.HomeName.ToLower().Contains(searchText.ToLower())
                            || elem.AwayName.ToLower().Contains(searchText.ToLower())).ToList();

                searchResult.AddRange(searchedMatches);
                SearchedMatches = new ObservableCollection<FootballMatchInfo>(searchedMatches);

                var bookmarkeLeagues = await _bookmarkService.GetAllBookmark<FootballLeagueInfo>();
                var searchedLeagues = bookmarkeLeagues.Where(elem => elem.LeagueName.ToLower().Contains(searchText.ToLower())
                            || elem.CountryName.ToLower().Contains(searchText.ToLower())).ToList();

                searchResult.AddRange(searchedLeagues);
                SearchedLeagues = new ObservableCollection<FootballLeagueInfo>(searchedLeagues);

                var bookmarkedTeams = await _bookmarkService.GetAllBookmark<FootballTeamInfo>();
                var searchedTeams = bookmarkedTeams.Where(elem => elem.CountryName.ToLower().Contains(searchText.ToLower())
                            || elem.TeamName.ToLower().Contains(searchText.ToLower())).ToList();

                searchResult.AddRange(searchedTeams);
                SearchedTeams = new ObservableCollection<FootballTeamInfo>(searchedTeams);

                return searchResult;
            }
            else
                IsSearching = false;

            return _recentSearchList;
        }

        private async Task InsertSearchKeywordAsync(string keyword, string logo)
        {
            var searchKeyword = new FootballRecentSearch()
            {
                Keyword = keyword,
                Logo = logo,
                StoredTime = DateTime.UtcNow,
            };

            // 중복 체크
            var foundItem = _recentSearchList.Find(elem => elem.PrimaryKey == searchKeyword.PrimaryKey);
            if (foundItem != null)
                _recentSearchList.Remove(foundItem);

            await _sqliteService.InsertOrUpdateAsync<FootballRecentSearch>(searchKeyword);

            _recentSearchList.Add(searchKeyword);
            _recentSearchList.Sort(ShinyHost.Resolve<StoredData_InverseDateComparer>());

            // 최대 저장개수 초과할 경우
            if (_recentSearchList.Count > 20)
            {
                for (int i = _recentSearchList.Count; i > 20; i--)
                {
                    var deletedItem = _recentSearchList[i - 1];

                    _recentSearchList.Remove(deletedItem);
                    await _sqliteService.DeleteAsync<FootballRecentSearch>(deletedItem.PrimaryKey);
                }
            }

            RecentSearches = new ObservableCollection<FootballRecentSearch>(_recentSearchList);
        }

        private async Task DeleteSearchKeywordAsync(FootballRecentSearch item)
        {
            _recentSearchList.Remove(item);
            RecentSearches.Remove(item);
            await _sqliteService.DeleteAsync<FootballRecentSearch>(item.PrimaryKey);
        }

        #endregion Methods
    }
}