using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Services.MessagingCenterMessageType;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Team;
using PoseSportsPredict.Views.Football.Bookmark;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarkTeamsViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            BookmarkedTeamsTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballTeamInfo>>();

            MessagingCenter.Subscribe<FootballTeamDetailViewModel, FootballTeamInfo>(this, FootballMessageType.Update_Bookmark_Team.ToString(), (s, e) => BookmarkMessageHandler(s, e));

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (_teamList?.Count > 0)
                return;

            BookmarkedTeamsTaskLoaderNotifier.Load(GetBookmarkedTeamsAsync);
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

        private TaskLoaderNotifier<IReadOnlyCollection<FootballTeamInfo>> _bookmarkedTeamsTaskLoaderNotifier;
        private List<FootballTeamInfo> _teamList;
        private ObservableCollection<FootballTeamInfo> _bookmarkedteams;
        private bool _IsEditMode;
        private readonly List<FootballTeamInfo> _DeleteTeamList = new List<FootballTeamInfo>();

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballTeamInfo>> BookmarkedTeamsTaskLoaderNotifier { get => _bookmarkedTeamsTaskLoaderNotifier; set => SetValue(ref _bookmarkedTeamsTaskLoaderNotifier, value); }
        public ObservableCollection<FootballTeamInfo> BookmarkedTeams { get => _bookmarkedteams; set => SetValue(ref _bookmarkedteams, value); }

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

        public ICommand SelectTeamCommand { get => new RelayCommand<FootballTeamInfo>((e) => SelectTeam(e)); }

        private async void SelectTeam(FootballTeamInfo teamInfo)
        {
            if (IsBusy || IsEditMode)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>(), teamInfo);

            SetIsBusy(false);
        }

        public ICommand EditButtonClickCommand { get => new RelayCommand(EditButtonClick); }

        private void EditButtonClick()
        {
            if (IsBusy)
                return;

            _DeleteTeamList.Clear();
            IsEditMode = true;
        }

        public ICommand CancelButtonClickCommand { get => new RelayCommand(CancelButton); }

        private void CancelButton()
        {
            if (IsBusy || !IsEditMode)
                return;

            IsEditMode = false;

            _DeleteTeamList.Clear();
            BookmarkedTeams = new ObservableCollection<FootballTeamInfo>(_teamList);
        }

        public ICommand SaveButtonClickCommand { get => new RelayCommand(SaveButtonClick); }

        private void SaveButtonClick()
        {
            if (IsBusy || !IsEditMode)
                return;

            IsEditMode = false;
            BookmarkedTeamsTaskLoaderNotifier.Load(UpdateBookmarkedTeamsAsync);
        }

        public ICommand DeleteTeamCommand { get => new RelayCommand<FootballTeamInfo>((e) => DeleteTeam(e)); }

        private void DeleteTeam(FootballTeamInfo teamInfo)
        {
            if (IsBusy || teamInfo == null)
                return;

            _DeleteTeamList.Add(teamInfo);
            BookmarkedTeams.Remove(teamInfo);
        }

        #endregion Commands

        #region Constructors

        public FootballBookmarkTeamsViewModel(
            FootballBookmarkTeamsPage coupledPage
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

        private async Task<IReadOnlyCollection<FootballTeamInfo>> GetBookmarkedTeamsAsync()
        {
            await Task.Delay(300);

            _teamList = await _sqliteService.SelectAllAsync<FootballTeamInfo>();
            _teamList.Sort(ShinyHost.Resolve<StoredDataComparer>());

            BookmarkedTeams = new ObservableCollection<FootballTeamInfo>(_teamList);

            IsEditMode = false;
            return _teamList;
        }

        private async Task<IReadOnlyCollection<FootballTeamInfo>> UpdateBookmarkedTeamsAsync()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // Delete Leauge
            foreach (var deleteTeamInfo in _DeleteTeamList)
            {
                await _sqliteService.DeleteAsync<FootballTeamInfo>(deleteTeamInfo.PrimaryKey);
            }

            _DeleteTeamList.Clear();

            // Update Order
            int order = BookmarkedTeams.Count;
            foreach (var BookmarkedLeague in BookmarkedTeams)
            {
                BookmarkedLeague.Order = order;
                await _sqliteService.InsertOrUpdateAsync<FootballTeamInfo>(BookmarkedLeague);

                order--;
            }

            _teamList = await _sqliteService.SelectAllAsync<FootballTeamInfo>();
            _teamList.Sort(ShinyHost.Resolve<StoredDataComparer>());

            BookmarkedTeams = new ObservableCollection<FootballTeamInfo>(_teamList);

            IsEditMode = false;

            SetIsBusy(false);

            return _teamList;
        }

        private void BookmarkMessageHandler(BaseViewModel sender, FootballTeamInfo item)
        {
            if (_teamList?.Count > 0)
            {
                if (item.IsBookmarked)
                {
                    _teamList.Add(item);
                    BookmarkedTeams.Add(item);
                }
                else
                {
                    var foundItem = _teamList.Where(elem => elem.PrimaryKey == item.PrimaryKey).FirstOrDefault();
                    Debug.Assert(foundItem != null);

                    _teamList.Remove(foundItem);
                    BookmarkedTeams.Remove(foundItem);
                }
            }
        }

        #endregion Methods
    }
}