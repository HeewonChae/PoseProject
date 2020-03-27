using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services.MessagingCenterMessageType;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League;
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
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarkLeaguesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            BookmarkedLeaguesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>>();

            MessagingCenter.Subscribe<FootballLeagueDetailViewModel, FootballLeagueInfo>(this, FootballMessageType.Update_Bookmark_League.ToString(), (s, e) => BookmarkMessageHandler(s, e));
            MessagingCenter.Subscribe<FootballLeagueListViewModel, FootballLeagueInfo>(this, FootballMessageType.Update_Bookmark_League.ToString(), (s, e) => BookmarkMessageHandler(s, e));

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

        private ISQLiteService _sqliteService;

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

            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), leagueInfo);

            SetIsBusy(false);
        }

        public ICommand EditButtonClickCommand { get => new RelayCommand(EditButtonClick); }

        private void EditButtonClick()
        {
            if (IsBusy)
                return;

            _DeleteLeagueList.Clear();
            IsEditMode = true;
        }

        public ICommand CancelButtonClickCommand { get => new RelayCommand(CancelButton); }

        private void CancelButton()
        {
            if (IsBusy || !IsEditMode)
                return;

            _DeleteLeagueList.Clear();
            BookmarkedLeagues = new ObservableCollection<FootballLeagueInfo>(_leagueList);
            IsEditMode = false;
        }

        public ICommand SaveButtonClickCommand { get => new RelayCommand(SaveButtonClick); }

        private void SaveButtonClick()
        {
            if (IsBusy)
                return;

            IsEditMode = false;
            BookmarkedLeaguesTaskLoaderNotifier.Load(UpdateBookmarkedLeaguesAsync);
        }

        public ICommand DeleteLeagueCommand { get => new RelayCommand<FootballLeagueInfo>((e) => DeleteLeague(e)); }

        private void DeleteLeague(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy)
                return;

            _DeleteLeagueList.Add(leagueInfo);
            BookmarkedLeagues.Remove(leagueInfo);
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
                coupledPage.Disappearing += (s, e) => this.OnDisAppearing();
            }
        }

        #endregion Constructors

        #region Methods

        private async Task<IReadOnlyCollection<FootballLeagueInfo>> GetBookmarkedLeaguesAsync()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            _leagueList = await _sqliteService.SelectAllAsync<FootballLeagueInfo>();
            _leagueList.Sort(ShinyHost.Resolve<StoredDataComparer>());

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
                await _sqliteService.DeleteAsync<FootballLeagueInfo>(deleteLeagueInfo.PrimaryKey);
                MessagingCenter.Send(this, FootballMessageType.Update_Bookmark_League.ToString(), deleteLeagueInfo);
            }

            _DeleteLeagueList.Clear();

            // Update Order
            int order = BookmarkedLeagues.Count;
            foreach (var BookmarkedLeague in BookmarkedLeagues)
            {
                BookmarkedLeague.Order = order;
                await _sqliteService.InsertOrUpdateAsync<FootballLeagueInfo>(BookmarkedLeague);

                order--;
            }

            _leagueList = await _sqliteService.SelectAllAsync<FootballLeagueInfo>();
            _leagueList.Sort(ShinyHost.Resolve<StoredDataComparer>());

            BookmarkedLeagues = new ObservableCollection<FootballLeagueInfo>(_leagueList);

            IsEditMode = false;

            SetIsBusy(false);

            return _leagueList;
        }

        private void BookmarkMessageHandler(BaseViewModel sender, FootballLeagueInfo item)
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
                    var foundItem = _leagueList.Where(elem => elem.PrimaryKey == item.PrimaryKey).FirstOrDefault();
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