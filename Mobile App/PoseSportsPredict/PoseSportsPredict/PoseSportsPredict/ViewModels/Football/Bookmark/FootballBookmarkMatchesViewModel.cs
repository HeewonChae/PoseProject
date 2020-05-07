using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.Views.Football.Bookmark;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarkMatchesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            BookmarkedMatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();

            string message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.Match);
            MessagingCenter.Subscribe<BookmarkService, FootballMatchInfo>(this, message, (s, e) => BookmarkMessageHandler(e));

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            var timeSpan = DateTime.UtcNow - _lastUpdateTime;
            IsEditMode = false;

#if DEBUG
            if (_matchList?.Count > 0 && timeSpan.TotalMinutes < 1) // 1분 마다 갱신
                return;
#else
            if (_matchList?.Count > 0 && timeSpan.TotalMinutes < 15) // 15분 마다 갱신
                return;
#endif

            BookmarkedMatchesTaskLoaderNotifier.Load(InitBookmarkedMatchesAsync);
        }

        public override void OnDisAppearing(params object[] datas)
        {
            CancelButton();
        }

        #endregion NavigableViewModel

        #region Services

        private IBookmarkService _bookmarkService;
        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _bookmarkedMatchesTaskLoaderNotifier;
        private List<FootballMatchInfo> _matchList;
        private ObservableCollection<FootballMatchInfo> _bookmarkedMatches;
        private DateTime _lastUpdateTime;
        private bool _IsEditMode;
        private readonly List<FootballMatchInfo> _DeleteMatchList = new List<FootballMatchInfo>();

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> BookmarkedMatchesTaskLoaderNotifier { get => _bookmarkedMatchesTaskLoaderNotifier; set => SetValue(ref _bookmarkedMatchesTaskLoaderNotifier, value); }
        public ObservableCollection<FootballMatchInfo> BookmarkedMatches { get => _bookmarkedMatches; set => SetValue(ref _bookmarkedMatches, value); }

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

        public ICommand SelectMatchCommand { get => new RelayCommand<FootballMatchInfo>((e) => SelectMatch(e)); }

        private async void SelectMatch(FootballMatchInfo matchInfo)
        {
            if (IsBusy || IsEditMode)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);

            SetIsBusy(false);
        }

        public ICommand EditButtonClickCommand { get => new RelayCommand(EditButtonClick); }

        private void EditButtonClick()
        {
            if (IsBusy)
                return;

            Debug.Assert(_DeleteMatchList.Count == 0);
            IsEditMode = true;
        }

        public ICommand CancelButtonClickCommand { get => new RelayCommand(CancelButton); }

        private void CancelButton()
        {
            if (IsBusy || !IsEditMode)
                return;

            IsEditMode = false;

            _DeleteMatchList.Clear();
            BookmarkedMatches = new ObservableCollection<FootballMatchInfo>(_matchList);
        }

        public ICommand SaveButtonClickCommand { get => new RelayCommand(SaveButtonClick); }

        private void SaveButtonClick()
        {
            if (IsBusy || !IsEditMode)
                return;

            IsEditMode = false;
            BookmarkedMatchesTaskLoaderNotifier.Load(UpdateBookmarkedMatchesAsync);
        }

        public ICommand DeleteMatchCommand { get => new RelayCommand<FootballMatchInfo>((e) => DeleteMatch(e)); }

        private void DeleteMatch(FootballMatchInfo matchInfo)
        {
            if (IsBusy || matchInfo == null)
                return;

            _DeleteMatchList.Add(matchInfo);
            _bookmarkedMatches.Remove(matchInfo);
        }

        #endregion Commands

        #region Constructors

        public FootballBookmarkMatchesViewModel(
            FootballBookmarkMatchesPage coupledPage,
            IBookmarkService bookmarkService,
            IWebApiService webApiService) : base(coupledPage)
        {
            _bookmarkService = bookmarkService;
            _webApiService = webApiService;

            if (OnInitializeView())
            {
                this.CoupledPage.Disappearing += (s, e) => this.OnDisAppearing();
            }
        }

        #endregion Constructors

        #region Methods

        private async Task<IReadOnlyCollection<FootballMatchInfo>> InitBookmarkedMatchesAsync()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            var oldBookmarkedMatches = await _bookmarkService.GetAllBookmark<FootballMatchInfo>();

            // Need Refrash MatchInfo
            List<int> needRefrashMatchIndexes = null;
            if (_matchList?.Count > 0)
            {
                needRefrashMatchIndexes = oldBookmarkedMatches.Where(elem => elem.MatchTime < DateTime.Now
                   && elem.MatchStatus != FootballMatchStatusType.FT
                   && elem.MatchStatus != FootballMatchStatusType.AET
                   && elem.MatchStatus != FootballMatchStatusType.PEN)
                   .Select(elem => elem.Id).ToList();
            }
            else
            {
                needRefrashMatchIndexes = oldBookmarkedMatches.Where(elem =>
                  elem.MatchStatus != FootballMatchStatusType.FT
                  && elem.MatchStatus != FootballMatchStatusType.AET
                  && elem.MatchStatus != FootballMatchStatusType.PEN)
                  .Select(elem => elem.Id).ToList();
            }

            if (needRefrashMatchIndexes.Count > 0)
            {
                // call server
                var server_result = await _webApiService.RequestAsyncWithToken<O_GET_FIXTURES_BY_INDEX>(new WebRequestContext
                {
                    SerializeType = SerializeType.MessagePack,
                    MethodType = WebMethodType.POST,
                    BaseUrl = AppConfig.PoseWebBaseUrl,
                    ServiceUrl = FootballProxy.ServiceUrl,
                    SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_INDEX,
                    PostData = new I_GET_FIXTURES_BY_INDEX
                    {
                        FixtureIds = needRefrashMatchIndexes.ToArray(),
                    }
                });

                if (server_result == null)
                    throw new Exception(LocalizeString.Occur_Error);

                // Update Matches
                foreach (var fixtureDetail in server_result.Fixtures)
                {
                    var convertedMatchInfo = ShinyHost.Resolve<FixtureDetailToMatchInfoConverter>().Convert(
                                   fixtureDetail,
                                   typeof(FootballMatchInfo),
                                   null,
                                   null) as FootballMatchInfo;

                    var foundMatchInfo = oldBookmarkedMatches.Find(elem => elem.Id == fixtureDetail.FixtureId);

                    foundMatchInfo.MatchStatus = convertedMatchInfo.MatchStatus;
                    foundMatchInfo.MatchTime = convertedMatchInfo.MatchTime;
                    foundMatchInfo.HomeScore = convertedMatchInfo.HomeScore;
                    foundMatchInfo.AwayScore = convertedMatchInfo.AwayScore;

                    await _bookmarkService.UpdateBookmark<FootballMatchInfo>(foundMatchInfo);
                    needRefrashMatchIndexes.Remove(foundMatchInfo.Id);
                }

                // Delete Invalid Matches
                foreach (var deletedIndex in needRefrashMatchIndexes)
                {
                    var foundMatchInfo = oldBookmarkedMatches.Find(elem => elem.Id == deletedIndex);

                    await _bookmarkService.RemoveBookmark<FootballMatchInfo>(foundMatchInfo, SportsType.Football, BookMarkType.Match);
                }
            }

            _matchList = (await _bookmarkService.GetAllBookmark<FootballMatchInfo>())
                .OrderBy(elem => elem.MatchTime).ToList();
            BookmarkedMatches = new ObservableCollection<FootballMatchInfo>(_matchList);

            _lastUpdateTime = DateTime.UtcNow;
            IsEditMode = false;

            SetIsBusy(false);

            return _matchList;
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> UpdateBookmarkedMatchesAsync()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            foreach (var deleteMatchInfo in _DeleteMatchList)
            {
                deleteMatchInfo.IsBookmarked = false;
                await _bookmarkService.RemoveBookmark<FootballMatchInfo>(deleteMatchInfo, SportsType.Football, BookMarkType.Match);
            }

            _DeleteMatchList.Clear();

            _matchList = (await _bookmarkService.GetAllBookmark<FootballMatchInfo>())
                .OrderBy(elem => elem.MatchTime).ToList();
            BookmarkedMatches = new ObservableCollection<FootballMatchInfo>(_matchList);

            IsEditMode = false;

            SetIsBusy(false);

            return _matchList;
        }

        private void BookmarkMessageHandler(FootballMatchInfo item)
        {
            SetIsBusy(true);

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
                    var foundItem = _matchList.FirstOrDefault(elem => elem.PrimaryKey == item.PrimaryKey);
                    Debug.Assert(foundItem != null);

                    _matchList.Remove(foundItem);
                    BookmarkedMatches.Remove(foundItem);
                }
            }

            SetIsBusy(false);
        }

        #endregion Methods
    }
}