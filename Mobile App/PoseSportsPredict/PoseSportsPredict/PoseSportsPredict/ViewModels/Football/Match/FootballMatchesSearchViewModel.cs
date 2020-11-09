using GalaSoft.MvvmLight.Command;
using PanCardView.Extensions;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.Utilities.SQLite;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Match;
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

namespace PoseSportsPredict.ViewModels.Football.Match
{
    public class FootballMatchesSearchViewModel : NavigableViewModel
    {
        private static FootballMatchesSearchViewModel _singleton;

        public static FootballMatchesSearchViewModel Singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = ShinyHost.Resolve<FootballMatchesSearchViewModel>();

                return _singleton;
            }
        }

        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            FootballMatchesSearchTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();

            return true;
        }

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            if (datas.Length == 1
                && datas[0] is List<FootballMatchInfo> matchList)
            {
                _allMatches = matchList;
            }

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
        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _footballMatchesSearchTaskLoaderNotifier;
        private List<FootballMatchInfo> _allMatches;
        private List<FootballRecentSearch> _recentSearchList;
        private ObservableCollection<FootballRecentSearch> _recentSearches;
        private ObservableCollection<FootballMatchListViewModel> _searchMatchesViewModels;
        private string _searchText;
        private bool _isSearching;

        #endregion Field

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> FootballMatchesSearchTaskLoaderNotifier { get => _footballMatchesSearchTaskLoaderNotifier; set => SetValue(ref _footballMatchesSearchTaskLoaderNotifier, value); }
        public ObservableCollection<FootballRecentSearch> RecentSearches { get => _recentSearches; set => SetValue(ref _recentSearches, value); }
        public ObservableCollection<FootballMatchListViewModel> SearchMatchesViewModels { get => _searchMatchesViewModels; set => SetValue(ref _searchMatchesViewModels, value); }
        public bool IsSearching { get => _isSearching; set => SetValue(ref _isSearching, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectGroupHeaderCommand { get => new RelayCommand<FootballMatchListViewModel>((e) => SelectGroupHeader(e)); }

        private void SelectGroupHeader(FootballMatchListViewModel groupInfo)
        {
            if (IsBusy)
                return;

            groupInfo.Expanded = !groupInfo.Expanded;

            var itmeIdx = SearchMatchesViewModels.FindIndex(groupInfo);
            SearchMatchesViewModels.RemoveAt(itmeIdx);
            SearchMatchesViewModels.Insert(itmeIdx, groupInfo);
        }

        public ICommand SearchBarTextChangedCommand { get => new RelayCommand<TextChangedEventArgs>((e) => SearchBarTextChanged(e)); }

        private async void SearchBarTextChanged(TextChangedEventArgs eventArgs)
        {
            _searchText = eventArgs.NewTextValue;

            await Task.Delay(700);

            if (_searchText == eventArgs.NewTextValue)
                FootballMatchesSearchTaskLoaderNotifier.Load(() => SearchMatchesAsync(_searchText));
        }

        public ICommand SelectSearchKeywordCommand { get => new RelayCommand<FootballRecentSearch>(e => SelectSearchKeyword(e)); }

        private async void SelectSearchKeyword(FootballRecentSearch item)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await InsertSearchKeywordAsync(item.Keyword, item.Logo);

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

        public ICommand TouchBackButtonCommand { get => new RelayCommand(TouchBackButton); }

        private async void TouchBackButton()
        {
            if (IsPageSwitched)
                return;

            SetIsPageSwitched(true);

            await PageSwitcher.PopPopupAsync();

            SetIsPageSwitched(false);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchesSearchViewModel(
            FootballMatchesSearchPage page,
            ISQLiteService sqliteService) : base(page)
        {
            _sqliteService = sqliteService;

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }

        #endregion Constructors

        #region Methods

        private async Task<IReadOnlyCollection<FootballMatchInfo>> SearchMatchesAsync(string searchText)
        {
            await Task.Delay(300);

            if (!string.IsNullOrEmpty(searchText))
            {
                IsSearching = true;

                var searchedMatches = _allMatches.Where(elem => elem.LeagueName.ToLower().Contains(searchText.ToLower())
                            || elem.League_CountryName.ToLower().Contains(searchText.ToLower())
                            || elem.HomeName.ToLower().Contains(searchText.ToLower())
                            || elem.AwayName.ToLower().Contains(searchText.ToLower())).ToList();

                var grouppingMatches = MatchGrouping(searchedMatches);

                var matchGroupCollection = new ObservableList<FootballMatchListViewModel>();
                // 경기 그룹 리스트
                foreach (var grouppingMatch in grouppingMatches)
                {
                    var matchListViewModel = ShinyHost.Resolve<FootballMatchListViewModel>();
                    matchListViewModel.GroupType = MatchGroupType.Default;
                    matchListViewModel.Title = grouppingMatch.Key;
                    matchListViewModel.TitleLogo = grouppingMatch.GroupLogo;
                    matchListViewModel.AlarmEditMode = false;
                    matchListViewModel.Matches = new ObservableCollection<FootballMatchInfo>(grouppingMatch.Matches);
                    matchListViewModel.IsPredicted = grouppingMatch.Matches.Any(elem => elem.IsPredicted);
                    matchListViewModel.Expanded = grouppingMatches.Count > 5 ? false : true;

                    matchGroupCollection.Add(matchListViewModel);
                }

                SearchMatchesViewModels = matchGroupCollection;

                return searchedMatches;
            }
            else
                IsSearching = false;

            return null;
        }

        private List<FootballMatchGroupInfo> MatchGrouping(List<FootballMatchInfo> matchList)
        {
            var result = new List<FootballMatchGroupInfo>();

            var grouping = matchList.GroupBy(elem => new { Country = elem.League_CountryName, CountryLogo = elem.League_CountryLogo, League = elem.LeagueName });
            foreach (var data in grouping)
            {
                var speratedData = data.SeperateByCount(15);

                if (speratedData.Count > 1)
                {
                    for (int i = 0; i < speratedData.Count; i++)
                    {
                        result.Add(new FootballMatchGroupInfo
                        {
                            Country = data.Key.Country,
                            League = data.Key.League,
                            GroupLogo = data.Key.CountryLogo,
                            Matches = speratedData[i],
                            GroupSubString = (i + 1).ToString(),
                            IsExpanded = true
                        });
                    }
                }
                else
                {
                    result.Add(new FootballMatchGroupInfo
                    {
                        Country = data.Key.Country,
                        League = data.Key.League,
                        GroupLogo = data.Key.CountryLogo,
                        Matches = data.ToArray(),
                        IsExpanded = true
                    });
                }
            }

            return result;
        }

        public async void SelectedMatchInfo(FootballMatchInfo matchInfo)
        {
            await InsertSearchKeywordAsync(matchInfo.League_CountryName, matchInfo.League_CountryLogo);
            await InsertSearchKeywordAsync(matchInfo.LeagueName, matchInfo.LeagueLogo);
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

            RecentSearches = new ObservableList<FootballRecentSearch>(_recentSearchList);
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