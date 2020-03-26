using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Services.MessagingCenterMessageType;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
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
    public class FootballBookmarkMatchesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            BookmarkedMatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>(GetBookmarkedMatchesAsync);

            MessagingCenter.Subscribe<FootballMatchDetailViewModel, FootballMatchInfo>(this, FootballMessageType.Update_Bookmark_Match.ToString(), (s, e) => BookmarkMessageHandler(s, e));

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            var timeSpan = DateTime.UtcNow - _lastUpdateTime;

#if DEBUG
            if (_matchList?.Count > 0 && timeSpan.TotalMinutes < 1) // 1분 마다 갱신
                return;
#else
            if (_matchList?.Count > 0 && timeSpan.TotalMinutes < 15) // 15분 마다 갱신
                return;
#endif

            BookmarkedMatchesTaskLoaderNotifier.Load();
        }

        #endregion NavigableViewModel

        #region Services

        private ISQLiteService _sqliteService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _bookmarkedMatchesTaskLoaderNotifier;
        private List<FootballMatchInfo> _matchList;
        private ObservableCollection<FootballMatchInfo> _bookmarkedMatches;
        private DateTime _lastUpdateTime;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> BookmarkedMatchesTaskLoaderNotifier { get => _bookmarkedMatchesTaskLoaderNotifier; set => SetValue(ref _bookmarkedMatchesTaskLoaderNotifier, value); }
        public ObservableCollection<FootballMatchInfo> BookmarkedMatches { get => _bookmarkedMatches; set => SetValue(ref _bookmarkedMatches, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectMatchCommand { get => new RelayCommand<FootballMatchInfo>((e) => SelectMatch(e)); }

        private async void SelectMatch(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballBookmarkMatchesViewModel(
            FootballBookmarkMatchesPage coupledPage
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

        private async Task<IReadOnlyCollection<FootballMatchInfo>> GetBookmarkedMatchesAsync()
        {
            await Task.Delay(300);

            _matchList = await _sqliteService.SelectAllAsync<FootballMatchInfo>();

            // Need Refrash MatchInfo
            var needRefrashMatchIndexes = _matchList.Where(elem => elem.MatchTime < DateTime.Now
                && elem.MatchStatus != FootballMatchStatusType.FT
                && elem.MatchStatus != FootballMatchStatusType.AET
                && elem.MatchStatus != FootballMatchStatusType.PEN)
                .Select(elem => elem.Id).ToList();

            //if (needRefrashMatchIndexes.Count > 0)
            //{
            //    // call server
            //    List<FootballFixtureDetail> fixtureDetails = new List<FootballFixtureDetail>();

            //    // Update Matches
            //    foreach (var fixtureDetail in fixtureDetails)
            //    {
            //        var foundMatchInfo = _matchList.Find(elem => elem.Id == fixtureDetail.FixtureId);

            //        foundMatchInfo.MatchStatus = fixtureDetail.MatchStatus;
            //        foundMatchInfo.MatchTime = fixtureDetail.MatchTime;
            //        foundMatchInfo.HomeScore = fixtureDetail.HomeTeam.Score;
            //        foundMatchInfo.AwayScore = fixtureDetail.AwayTeam.Score;

            //        await _sqliteService.InsertOrUpdateAsync<FootballMatchInfo>(foundMatchInfo);
            //        needRefrashMatchIndexes.Remove(foundMatchInfo.Id);
            //    }

            //    // Delete Invalid Matches
            //    foreach (var deletedIndex in needRefrashMatchIndexes)
            //    {
            //        var foundMatchInfo = _matchList.Find(elem => elem.Id == deletedIndex);

            //        await _sqliteService.DeleteAsync<FootballMatchInfo>(foundMatchInfo.PrimaryKey);
            //        _matchList.Remove(foundMatchInfo);
            //    }
            //}

            _matchList = _matchList.OrderBy(elem => elem.MatchTime).ToList();
            BookmarkedMatches = new ObservableCollection<FootballMatchInfo>(_matchList);

            _lastUpdateTime = DateTime.UtcNow;

            return _matchList;
        }

        private void BookmarkMessageHandler(BaseViewModel sender, FootballMatchInfo item)
        {
            if (_matchList?.Count > 0)
            {
                if (item.IsBookmarked)
                {
                    _matchList.Add(item);
                    _matchList = _matchList.OrderBy(elem => elem.MatchTime).ToList();

                    BookmarkedMatches = new ObservableCollection<FootballMatchInfo>(_matchList);
                }
                else
                {
                    var foundItem = _matchList.Where(elem => elem.PrimaryKey == item.PrimaryKey).FirstOrDefault();
                    Debug.Assert(foundItem != null);

                    _matchList.Remove(foundItem);
                    BookmarkedMatches.Remove(foundItem);
                }
            }
        }

        #endregion Methods
    }
}