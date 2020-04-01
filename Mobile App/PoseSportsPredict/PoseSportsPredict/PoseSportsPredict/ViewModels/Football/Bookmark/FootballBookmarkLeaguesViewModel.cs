using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.Views.Football.Bookmark;
using Sharpnado.Presentation.Forms;
using Shiny;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarkLeaguesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            BookmarkedLeaguesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>>();

            string message = _bookmarkService.BuildBookmarkMessage(Models.SportsType.Football, Models.BookMarkType.Bookmark_League);
            MessagingCenter.Subscribe<BookmarkService, FootballLeagueInfo>(this, message, (s, e) => BookmarkMessageHandler(e));

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (_leagueList?.Count > 0)
                return;

            BookmarkedLeaguesTaskLoaderNotifier.Load(GetBookmarkedLeaguesAsync);
        }

        public override void OnDisAppearing(params object[] datas)
        {
            CancelButton();
        }

        #endregion NavigableViewModel

        #region Services

        private IBookmarkService _bookmarkService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>> _bookmarkedLeaguesTaskLoaderNotifier;
        private List<FootballLeagueInfo> _leagueList;
        private ObservableCollection<FootballLeagueInfo> _bookmarkedLeagues;
        private bool _IsEditMode;
        private readonly List<FootballLeagueInfo> _DeleteLeagueList = new List<FootballLeagueInfo>();

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>> BookmarkedLeaguesTaskLoaderNotifier { get => _bookmarkedLeaguesTaskLoaderNotifier; set => SetValue(ref _bookmarkedLeaguesTaskLoaderNotifier, value); }
        public ObservableCollection<FootballLeagueInfo> BookmarkedLeagues { get => _bookmarkedLeagues; set => SetValue(ref _bookmarkedLeagues, value); }

        public bool IsEditMode
        {
            get => _IsEditMode;

            set
            {
                if (CoupledPage.Parent?.BindingContext is FootballBookmarksTabViewModel tabViewModel)
                {
                    tabViewModel.IsSearchIconVisible = !value;
                }

                SetValue(ref _IsEditMode, value);
            }
        }

        #endregion Properties

        #region Commands

        public ICommand SelectLeagueCommand { get => new RelayCommand<FootballLeagueInfo>((e) => SelectLeague(e)); }

        private async void SelectLeague(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy || IsEditMode)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), leagueInfo);

            SetIsBusy(false);
        }

        public ICommand EditButtonClickCommand { get => new RelayCommand(EditButtonClick); }

        private void EditButtonClick()
        {
            if (IsBusy)
                return;

            Debug.Assert(_DeleteLeagueList.Count == 0);
            IsEditMode = true;
        }

        public ICommand CancelButtonClickCommand { get => new RelayCommand(CancelButton); }

        private void CancelButton()
        {
            if (IsBusy || !IsEditMode)
                return;

            IsEditMode = false;

            _DeleteLeagueList.Clear();
            BookmarkedLeagues = new ObservableCollection<FootballLeagueInfo>(_leagueList);
        }

        public ICommand SaveButtonClickCommand { get => new RelayCommand(SaveButtonClick); }

        private void SaveButtonClick()
        {
            if (IsBusy || !IsEditMode)
                return;

            IsEditMode = false;
            BookmarkedLeaguesTaskLoaderNotifier.Load(UpdateBookmarkedLeaguesAsync);
        }

        public ICommand DeleteLeagueCommand { get => new RelayCommand<FootballLeagueInfo>((e) => DeleteLeague(e)); }

        private void DeleteLeague(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy || leagueInfo == null)
                return;

            _DeleteLeagueList.Add(leagueInfo);
            BookmarkedLeagues.Remove(leagueInfo);
        }

        #endregion Commands

        #region Constructors

        public FootballBookmarkLeaguesViewModel(
            FootballBookmarkLeaguesPage coupledPage
            , IBookmarkService bookmarkService) : base(coupledPage)
        {
            _bookmarkService = bookmarkService;

            if (OnInitializeView())
            {
                coupledPage.Disappearing += (s, e) => this.OnDisAppearing();
            }
        }

        #endregion Constructors

        #region Methods

        private async Task<IReadOnlyCollection<FootballLeagueInfo>> GetBookmarkedLeaguesAsync()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            _leagueList = await _bookmarkService.GetAllBookmark<FootballLeagueInfo>();
            _leagueList.Sort(ShinyHost.Resolve<StoredData_BasicComparer>());

            BookmarkedLeagues = new ObservableCollection<FootballLeagueInfo>(_leagueList);

            IsEditMode = false;

            SetIsBusy(false);

            return _leagueList;
        }

        private async Task<IReadOnlyCollection<FootballLeagueInfo>> UpdateBookmarkedLeaguesAsync()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // Delete Leauge
            foreach (var deleteLeagueInfo in _DeleteLeagueList)
            {
                deleteLeagueInfo.IsBookmarked = false;

                await _bookmarkService.RemoveBookmark<FootballLeagueInfo>(deleteLeagueInfo, Models.SportsType.Football, Models.BookMarkType.Bookmark_League);
            }

            _DeleteLeagueList.Clear();

            // Update Order
            int order = BookmarkedLeagues.Count;
            foreach (var BookmarkedLeague in BookmarkedLeagues)
            {
                BookmarkedLeague.Order = order;
                order--;

                await _bookmarkService.UpdateBookmark<FootballLeagueInfo>(BookmarkedLeague);
            }

            _leagueList = await _bookmarkService.GetAllBookmark<FootballLeagueInfo>();
            _leagueList.Sort(ShinyHost.Resolve<StoredData_BasicComparer>());

            BookmarkedLeagues = new ObservableCollection<FootballLeagueInfo>(_leagueList);

            IsEditMode = false;

            SetIsBusy(false);

            return _leagueList;
        }

        private void BookmarkMessageHandler(FootballLeagueInfo item)
        {
            SetIsBusy(true);

            if (_leagueList?.Count > 0)
            {
                if (item.IsBookmarked)
                {
                    _leagueList.Add(item);
                    BookmarkedLeagues.Add(item);
                }
                else
                {
                    var foundItem = _leagueList.FirstOrDefault(elem => elem.PrimaryKey == item.PrimaryKey);
                    Debug.Assert(foundItem != null);

                    _leagueList.Remove(foundItem);
                    BookmarkedLeagues.Remove(foundItem);
                }
            }

            SetIsBusy(false);
        }

        #endregion Methods
    }
}