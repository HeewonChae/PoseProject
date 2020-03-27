using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using PoseSportsPredict.Services.MessagingCenterMessageType;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Bookmark;
using PoseSportsPredict.ViewModels.Football.League;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.Views.Football;
using PoseSportsPredict.Views.Football.League;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
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
            _leagueList = CoverageLeague.CoverageLeagues.Values.ToList();

            MessagingCenter.Subscribe<FootballLeagueDetailViewModel, FootballLeagueInfo>(this, FootballMessageType.Update_Bookmark_League.ToString(), (s, e) => this.BookmarkMessageHandler(s, e));
            MessagingCenter.Subscribe<FootballBookmarkLeaguesViewModel, FootballLeagueInfo>(this, FootballMessageType.Update_Bookmark_League.ToString(), (s, e) => this.BookmarkMessageHandler(s, e));

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            var searchBar = CoupledPage.FindByName<SearchBar>("_searchBar");
            if (string.IsNullOrEmpty(searchBar.Text)
                && _orgLeagueGroups == null)
            {
                UpdateLeagueGroups(_leagueList, false);
                _orgLeagueGroups = LeagueGroups;
            }
        }

        #endregion NavigableViewModel

        #region Services

        private ISQLiteService _sqliteService;

        #endregion Services

        #region Fields

        private List<FootballLeagueInfo> _leagueList;
        private ObservableCollection<FootballLeagueGroup> _orgLeagueGroups;
        private ObservableCollection<FootballLeagueGroup> _leagueGroups;

        #endregion Fields

        #region Properties

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
            if (eventArgs.NewTextValue == string.Empty)
            {
                LeagueGroups = new ObservableCollection<FootballLeagueGroup>(_orgLeagueGroups);
                return;
            }

            var searchedLeague = _leagueList.Where(elem => elem.LeagueName.ToLower().Contains(eventArgs.NewTextValue.ToLower())
                            || elem.CountryName.ToLower().Contains(eventArgs.NewTextValue.ToLower())).ToList();

            UpdateLeagueGroups(searchedLeague, true);
        }

        #endregion Commands

        #region Constructors

        public FootballLeaguesViewModel(
            FootballLeaguesPage page
            , ISQLiteService sqliteService) : base(page)
        {
            if (OnInitializeView())
            {
                _sqliteService = sqliteService;
            }
        }

        #endregion Constructors

        #region Methods

        private void UpdateLeagueGroups(List<FootballLeagueInfo> leagueList, bool isAllExpanded)
        {
            if (leagueList == null || leagueList.Count == 0)
                LeagueGroups = new ObservableCollection<FootballLeagueGroup>();

            var leagueGroupsCollection = new ObservableCollection<FootballLeagueGroup>();

            // Group by country
            var leaguesGroupByCountry = leagueList.GroupBy(elem => elem.CountryName);

            // World 데이터는 1순위로 등록
            var InternationalLeagues = leaguesGroupByCountry.Where(elem => elem.Key == "World").FirstOrDefault();
            if (InternationalLeagues != null)
            {
                leagueGroupsCollection.Add(new FootballLeagueGroup(InternationalLeagues.Key, InternationalLeagues.First().CountryLogo, isAllExpanded)
                {
                    FootballLeagueListViewModel = new FootballLeagueListViewModel
                    {
                        Leagues = new ObservableCollection<FootballLeagueInfo>(InternationalLeagues.ToArray())
                    }
                });
            }

            foreach (var grouppingLeague in leaguesGroupByCountry)
            {
                if (grouppingLeague.Key == "World")
                    continue;

                leagueGroupsCollection.Add(new FootballLeagueGroup(grouppingLeague.Key, grouppingLeague.First().CountryLogo, isAllExpanded)
                {
                    FootballLeagueListViewModel = new FootballLeagueListViewModel
                    {
                        Leagues = new ObservableCollection<FootballLeagueInfo>(grouppingLeague.ToArray())
                    }
                });
            }

            LeagueGroups = leagueGroupsCollection;
        }

        private void BookmarkMessageHandler(BaseViewModel sender, FootballLeagueInfo item)
        {
            var foundLeague = _leagueList.Where(elem => elem.PrimaryKey == item.PrimaryKey).FirstOrDefault();
            if (foundLeague != null)
            {
                foundLeague.IsBookmarked = item.IsBookmarked;
                foundLeague.OnPropertyChanged("IsBookmarked");
            }
        }

        #endregion Methods
    }
}