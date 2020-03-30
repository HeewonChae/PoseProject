using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.League;
using Sharpnado.Presentation.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballLeaguesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            LeaguesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>>();

            string message = BookmarkServiceHelper.BuildBookmarkMessage(null, SportsType.Football, BookMarkType.Bookmark_League);
            MessagingCenter.Subscribe<BookmarkService, FootballLeagueInfo>(this, message, (s, e) => this.BookmarkMessageHandler(e));

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (!LeaguesTaskLoaderNotifier.IsNotStarted)
                return;

            LeaguesTaskLoaderNotifier.Load(InitLeaguesAsync);
        }

        #endregion NavigableViewModel

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>> _leaguesTaskLoaderNotifier;
        private List<FootballLeagueInfo> _leagueList;
        private ObservableCollection<FootballLeagueGroup> _orgLeagueGroups;
        private ObservableCollection<FootballLeagueGroup> _leagueGroups;
        private string _searchText;

        #endregion Fields

        #region Properties

        public bool IsSearchEnable { get => _leagueList?.Count > 0; set => OnPropertyChanged("IsSearchEnable"); }
        public TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>> LeaguesTaskLoaderNotifier { get => _leaguesTaskLoaderNotifier; set => SetValue(ref _leaguesTaskLoaderNotifier, value); }
        public ObservableCollection<FootballLeagueGroup> LeagueGroups { get => _leagueGroups; set => SetValue(ref _leagueGroups, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectGroupHeaderCommand { get => new RelayCommand<FootballLeagueGroup>((e) => SelectGroupHeader(e)); }

        private void SelectGroupHeader(FootballLeagueGroup groupInfo)
        {
            groupInfo.Expanded = !groupInfo.Expanded;
            LeagueGroups = new ObservableCollection<FootballLeagueGroup>(LeagueGroups);
        }

        public ICommand SearchBarTextChangedCommand { get => new RelayCommand<TextChangedEventArgs>((e) => SearchBarTextChanged(e)); }

        private void SearchBarTextChanged(TextChangedEventArgs eventArgs)
        {
            _searchText = eventArgs.NewTextValue;

            if (eventArgs.NewTextValue == string.Empty)
                LeaguesTaskLoaderNotifier.Load(SearchLeaguesAsync);
        }

        public ICommand SearchCommand { get => new RelayCommand(Search); }

        private void Search()
        {
            LeaguesTaskLoaderNotifier.Load(SearchLeaguesAsync);
        }

        #endregion Commands

        #region Constructors

        public FootballLeaguesViewModel(
            FootballLeaguesPage page) : base(page)
        {
            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        private async Task<IReadOnlyCollection<FootballLeagueInfo>> InitLeaguesAsync()
        {
            await Task.Delay(300);

            _leagueList = CoverageLeague.CoverageLeagues.Values.ToList();

            UpdateLeagueGroups(_leagueList, false);

            _orgLeagueGroups = LeagueGroups;

            IsSearchEnable = true;

            return _leagueList;
        }

        private async Task<IReadOnlyCollection<FootballLeagueInfo>> SearchLeaguesAsync()
        {
            await Task.Delay(300);

            List<FootballLeagueInfo> searchedLeague = _leagueList;

            if (_searchText == string.Empty)
            {
                LeagueGroups = new ObservableCollection<FootballLeagueGroup>(_orgLeagueGroups);
            }
            else
            {
                searchedLeague = _leagueList.Where(elem => elem.LeagueName.ToLower().Contains(_searchText.ToLower())
                            || elem.CountryName.ToLower().Contains(_searchText.ToLower())).ToList();

                UpdateLeagueGroups(searchedLeague, true);
            }

            return searchedLeague;
        }

        private void UpdateLeagueGroups(List<FootballLeagueInfo> leagueList, bool isAllExpanded)
        {
            if (leagueList == null || leagueList.Count == 0)
                LeagueGroups = new ObservableCollection<FootballLeagueGroup>();

            var leagueGroupsCollection = new ObservableCollection<FootballLeagueGroup>();

            // Group by country
            var leaguesGroupByCountry = leagueList.GroupBy(elem => elem.CountryName);

            // World 데이터는 1순위로 등록
            var InternationalLeagues = leaguesGroupByCountry.FirstOrDefault(elem => elem.Key == "World");
            if (InternationalLeagues != null)
            {
                leagueGroupsCollection.Add(new FootballLeagueGroup(
                    InternationalLeagues.Key,
                    InternationalLeagues.First().CountryLogo,
                    InternationalLeagues.ToArray(),
                    isAllExpanded));
            }

            foreach (var grouppingLeague in leaguesGroupByCountry)
            {
                if (grouppingLeague.Key == "World")
                    continue;

                leagueGroupsCollection.Add(new FootballLeagueGroup(
                    grouppingLeague.Key,
                    grouppingLeague.First().CountryLogo,
                    grouppingLeague.ToArray(),
                    isAllExpanded));
            }

            LeagueGroups = leagueGroupsCollection;
        }

        private void BookmarkMessageHandler(FootballLeagueInfo item)
        {
            var foundLeague = _leagueList.FirstOrDefault(elem => elem.PrimaryKey == item.PrimaryKey);
            if (foundLeague != null)
            {
                foundLeague.IsBookmarked = item.IsBookmarked;
                foundLeague.OnPropertyChanged("IsBookmarked");
            }
        }

        #endregion Methods
    }
}