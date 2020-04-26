using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League;
using PoseSportsPredict.Views.Football.League;
using Sharpnado.Presentation.Forms;
using Shiny;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballLeaguesViewModel : NavigableViewModel, IIconChange
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            IsSelected = false;

            LeaguesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>>();

            string message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.League);
            MessagingCenter.Subscribe<BookmarkService, FootballLeagueInfo>(this, message, (s, e) => this.BookmarkMessageHandler(e));

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            IsSelected = true;

            if (!LeaguesTaskLoaderNotifier.IsNotStarted)
                return;

            LeaguesTaskLoaderNotifier.Load(InitLeaguesAsync);
        }

        public override void OnDisAppearing(params object[] datas)
        {
            IsSelected = false;
        }

        #endregion NavigableViewModel

        #region IIconChange

        private bool _isSelected;

        public NavigationPage NavigationPage { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    NavigationPage.IconImageSource = CurrentIcon;
                }
            }
        }

        public string CurrentIcon { get => IsSelected ? "ic_trophy_selected.png" : "ic_trophy_unselected.png"; }

        #endregion IIconChange

        #region Services

        private IBookmarkService _bookmarkService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>> _leaguesTaskLoaderNotifier;
        private List<FootballLeagueInfo> _leagueList;
        private ObservableCollection<FootballLeagueListViewModel> _orgLeagueGroups;
        private ObservableCollection<FootballLeagueListViewModel> _leagueGroups;
        private string _searchText;

        #endregion Fields

        #region Properties

        public bool IsSearchEnable { get => _leagueList?.Count > 0; set => OnPropertyChanged("IsSearchEnable"); }
        public TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>> LeaguesTaskLoaderNotifier { get => _leaguesTaskLoaderNotifier; set => SetValue(ref _leaguesTaskLoaderNotifier, value); }
        public ObservableCollection<FootballLeagueListViewModel> LeagueGroups { get => _leagueGroups; set => SetValue(ref _leagueGroups, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectGroupHeaderCommand { get => new RelayCommand<FootballLeagueListViewModel>((e) => SelectGroupHeader(e)); }

        private void SelectGroupHeader(FootballLeagueListViewModel groupInfo)
        {
            groupInfo.Expanded = !groupInfo.Expanded;
        }

        public ICommand SearchBarTextChangedCommand { get => new RelayCommand<TextChangedEventArgs>((e) => SearchBarTextChanged(e)); }

        private async void SearchBarTextChanged(TextChangedEventArgs eventArgs)
        {
            _searchText = eventArgs.NewTextValue;

            await Task.Delay(700);

            if (_searchText == eventArgs.NewTextValue)
                LeaguesTaskLoaderNotifier.Load(() => SearchLeaguesAsync(_searchText));
        }

        public ICommand SearchCommand { get => new RelayCommand(Search); }

        private void Search()
        {
        }

        #endregion Commands

        #region Constructors

        public FootballLeaguesViewModel(
            FootballLeaguesPage page,
            IBookmarkService bookmarkService) : base(page)
        {
            _bookmarkService = bookmarkService;

            NavigationPage = new MaterialNavigationPage(this.CoupledPage)
            {
                Title = LocalizeString.Leagues,
                IconImageSource = CurrentIcon,
            };

            if (OnInitializeView())
            {
                this.CoupledPage.Disappearing += (s, e) => OnDisAppearing();
            }
        }

        #endregion Constructors

        #region Methods

        private async Task<IReadOnlyCollection<FootballLeagueInfo>> InitLeaguesAsync()
        {
            await Task.Delay(300);

            _leagueList = CoverageLeague.CoverageLeagues.Values.ToList();

            var bookmarkedLeagues = await _bookmarkService.GetAllBookmark<FootballLeagueInfo>();
            foreach (var league in _leagueList)
            {
                var bookmarkedLeague = bookmarkedLeagues.Find(elem => elem.PrimaryKey == league.PrimaryKey);

                league.IsBookmarked = bookmarkedLeague?.IsBookmarked ?? false;
            }

            UpdateLeagueGroups(_leagueList, false);

            _orgLeagueGroups = LeagueGroups;

            IsSearchEnable = true;

            return _leagueList;
        }

        private async Task<IReadOnlyCollection<FootballLeagueInfo>> SearchLeaguesAsync(string searchText)
        {
            await Task.Delay(300);

            List<FootballLeagueInfo> searchedLeague = _leagueList;

            if (searchText == string.Empty)
            {
                LeagueGroups = _orgLeagueGroups;
            }
            else
            {
                searchedLeague = _leagueList.Where(elem => elem.LeagueName.ToLower().Contains(searchText.ToLower())
                            || elem.CountryName.ToLower().Contains(searchText.ToLower())).ToList();

                UpdateLeagueGroups(searchedLeague, true);
            }

            return searchedLeague;
        }

        private void UpdateLeagueGroups(List<FootballLeagueInfo> leagueList, bool isAllExpanded)
        {
            if (leagueList == null || leagueList.Count == 0)
                LeagueGroups = new ObservableCollection<FootballLeagueListViewModel>();

            var leagueGroupsCollection = new ObservableCollection<FootballLeagueListViewModel>();

            // Group by country
            var leaguesGroupByCountry = leagueList.GroupBy(elem => elem.CountryName);

            // World 데이터는 1순위로 등록
            var InternationalLeagues = leaguesGroupByCountry.FirstOrDefault(elem => elem.Key == "World");
            if (InternationalLeagues != null)
            {
                var leagueListViewModel = ShinyHost.Resolve<FootballLeagueListViewModel>();
                leagueListViewModel.Title = InternationalLeagues.Key;
                leagueListViewModel.TitleLogo = InternationalLeagues.First().CountryLogo;
                leagueListViewModel.Leagues = new ObservableCollection<FootballLeagueInfo>(InternationalLeagues.ToArray());
                leagueListViewModel.Expanded = isAllExpanded;

                leagueGroupsCollection.Add(leagueListViewModel);
            }

            foreach (var grouppingLeague in leaguesGroupByCountry)
            {
                if (grouppingLeague.Key == "World")
                    continue;

                var leagueListViewModel = ShinyHost.Resolve<FootballLeagueListViewModel>();
                leagueListViewModel.Title = grouppingLeague.Key;
                leagueListViewModel.TitleLogo = grouppingLeague.First().CountryLogo;
                leagueListViewModel.Leagues = new ObservableCollection<FootballLeagueInfo>(grouppingLeague.ToArray());
                leagueListViewModel.Expanded = isAllExpanded;

                leagueGroupsCollection.Add(leagueListViewModel);
            }

            LeagueGroups = leagueGroupsCollection;
        }

        private void BookmarkMessageHandler(FootballLeagueInfo item)
        {
            var foundLeague = _leagueList?.FirstOrDefault(elem => elem.PrimaryKey == item.PrimaryKey);
            if (foundLeague != null)
            {
                foundLeague.IsBookmarked = item.IsBookmarked;
                foundLeague.OnPropertyChanged("IsBookmarked");
            }
        }

        #endregion Methods
    }
}