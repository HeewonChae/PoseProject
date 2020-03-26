using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Services.MessagingCenterMessageType;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.Views.Football.Bookmark;
using Sharpnado.Presentation.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Shiny;
using PoseSportsPredict.ViewModels.Football.League;
using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.Logics;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarkLeaguesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            BookmarkedLeaguesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>>(GetBookmarkedLeaguesAsync);

            MessagingCenter.Subscribe<FootballLeagueDetailViewModel, FootballLeagueInfo>(this, FootballMessageType.Update_Bookmark_League.ToString(), (s, e) => BookmarkMessageHandler(s, e));
            MessagingCenter.Subscribe<FootballLeagueListViewModel, FootballLeagueInfo>(this, FootballMessageType.Update_Bookmark_League.ToString(), (s, e) => BookmarkMessageHandler(s, e));

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (_leagueList?.Count > 0)
                return;

            BookmarkedLeaguesTaskLoaderNotifier.Load();
        }

        #endregion NavigableViewModel

        #region Services

        private ISQLiteService _sqliteService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>> _bookmarkedLeaguesTaskLoaderNotifier;
        private List<FootballLeagueInfo> _leagueList;
        private ObservableCollection<FootballLeagueInfo> _bookmarkedLeagues;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>> BookmarkedLeaguesTaskLoaderNotifier { get => _bookmarkedLeaguesTaskLoaderNotifier; set => SetValue(ref _bookmarkedLeaguesTaskLoaderNotifier, value); }
        public ObservableCollection<FootballLeagueInfo> BookmarkedLeagues { get => _bookmarkedLeagues; set => SetValue(ref _bookmarkedLeagues, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectLeagueCommand { get => new RelayCommand<FootballLeagueInfo>((e) => SelectLeague(e)); }

        private async void SelectLeague(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), leagueInfo);

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballBookmarkLeaguesViewModel(
            FootballBookmarkLeaguesPage coupledPage
            , ISQLiteService sqliteService) : base(coupledPage)
        {
            _sqliteService = sqliteService;

            if (OnInitializeView())
            {
                coupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }

        #endregion Constructors

        #region Methods

        private async Task<IReadOnlyCollection<FootballLeagueInfo>> GetBookmarkedLeaguesAsync()
        {
            await Task.Delay(300);

            _leagueList = await _sqliteService.SelectAllAsync<FootballLeagueInfo>();
            _leagueList.Sort(ShinyHost.Resolve<StoredDataComparer>());

            BookmarkedLeagues = new ObservableCollection<FootballLeagueInfo>(_leagueList);

            return _leagueList;
        }

        private void BookmarkMessageHandler(BaseViewModel sender, FootballLeagueInfo item)
        {
            if (_leagueList?.Count > 0)
            {
                if (item.IsBookmarked)
                {
                    _leagueList.Add(item);
                    BookmarkedLeagues.Add(item);
                }
                else
                {
                    var foundItem = _leagueList.Where(elem => elem.PrimaryKey == item.PrimaryKey).FirstOrDefault();
                    Debug.Assert(foundItem != null);

                    _leagueList.Remove(foundItem);
                    BookmarkedLeagues.Remove(foundItem);
                }
            }
        }

        #endregion Methods
    }
}