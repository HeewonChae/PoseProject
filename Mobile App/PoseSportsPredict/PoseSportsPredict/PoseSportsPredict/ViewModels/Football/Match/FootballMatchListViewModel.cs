using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Enum;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xam.Plugin;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.ViewModels.Football.Match
{
    public class FootballMatchListViewModel : BaseViewModel, IExpandable, ITempletable
    {
        #region Services

        private IBookmarkService _bookmarkService;
        private INotificationService _notificationService;

        #endregion Services

        #region IExpandable

        private bool _expanded;
        private string _title;
        private string _titleLogo;

        public MatchGroupType GroupType { get; set; }
        public NativeAdsSizeType NativeAdsType { get; set; }
        public string Title { get => _title; set => SetValue(ref _title, value); }
        public string StateIcon => Expanded ? "ic_expanded.png" : "ic_collapsed.png";
        public string TitleLogo { get => _titleLogo; set => SetValue(ref _titleLogo, value); }

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged(nameof(Expanded));
                    OnPropertyChanged(nameof(StateIcon));
                }
            }
        }

        #endregion IExpandable

        #region Fields

        private bool _alarmEditMode;
        private bool _isPredicted;
        private int _selectedIndex; // 추천 경기용

        #endregion Fields

        #region Properties

        public int MatchCount => Matches?.Count ?? 0;
        public bool AlarmEditMode { get => _alarmEditMode; set => SetValue(ref _alarmEditMode, value); }
        public bool IsPredicted { get => _isPredicted; set => SetValue(ref _isPredicted, value); }
        public ObservableCollection<FootballMatchInfo> Matches { get; set; }
        public int CurrentIndex { get => _selectedIndex; set => SetValue(ref _selectedIndex, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectMatchCommand { get => new RelayCommand<FootballMatchInfo>((e) => SelectMatch(e)); }

        private async void SelectMatch(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);

            SetIsBusy(false);
        }

        public ICommand SelectMatch_LongTapCommand { get => new RelayCommand<FootballMatchInfo>((e) => SelectMatch_LongTap(e)); }

        private void SelectMatch_LongTap(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            MatchInfoLongTapPopup.Execute(matchInfo);

            SetIsBusy(false);
        }

        public ICommand TouchAlarmButtonCommand { get => new RelayCommand<FootballMatchInfo>((e) => TouchAlarmButton(e)); }

        private async void TouchAlarmButton(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            if (!matchInfo.IsAlarmed)
            {
                DateTime notifyTime = matchInfo.MatchTime;
                await _notificationService.AddNotification(new NotificationInfo
                {
                    Id = matchInfo.Id,
                    Title = LocalizeString.Match_Begin_Soon,
                    Description = $"{matchInfo.LeagueName}.  {matchInfo.HomeName}  vs  {matchInfo.AwayName}",
                    IntentData = matchInfo.JsonSerialize(),
                    IconName = "ic_soccer_alarm",
                    SportsType = SportsType.Football,
                    NotificationType = NotificationType.MatchStart,
                    NotifyTime = notifyTime,
                    StoredTime = DateTime.UtcNow,
                });
            }
            else
            {
                await _notificationService.DeleteNotification(matchInfo.Id, SportsType.Football, NotificationType.MatchStart);
            }

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand<FootballMatchInfo>((e) => TouchBookmarkButton(e)); }

        private async void TouchBookmarkButton(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            // Add Bookmark
            if (matchInfo.IsBookmarked)
                await _bookmarkService.RemoveBookmark<FootballMatchInfo>(matchInfo, SportsType.Football, BookMarkType.Match);
            else
                await _bookmarkService.AddBookmark<FootballMatchInfo>(matchInfo, SportsType.Football, BookMarkType.Match);

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchListViewModel(
            IBookmarkService bookmarkService,
            INotificationService notificationService)
        {
            _bookmarkService = bookmarkService;
            _notificationService = notificationService;
        }

        #endregion Constructors
    }
}