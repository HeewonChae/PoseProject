using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using Plugin.LocalNotification;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using Shiny;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Match
{
    public class FootballMatchListViewModel : BaseViewModel
    {
        #region Services

        public IBookmarkService _bookmarkService;

        #endregion Services

        #region Fields

        private bool _alarmEditMode;

        #endregion Fields

        #region Properties

        public int MatchCount => Matches?.Count ?? 0;
        public bool AlarmEditMode { get => _alarmEditMode; set => SetValue(ref _alarmEditMode, value); }
        public ObservableCollection<FootballMatchInfo> Matches { get; set; }

        #endregion Properties

        #region Commands

        public ICommand SelectMatchCommand { get => new RelayCommand<FootballMatchInfo>((e) => SelectMatch(e)); }

        private async void SelectMatch(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            //await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);

            SetIsBusy(false);
        }

        public ICommand TouchAlarmButtonCommand { get => new RelayCommand<FootballMatchInfo>((e) => TouchAlarmButton(e)); }

        private void TouchAlarmButton(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            matchInfo.IsAlarmed = !matchInfo.IsAlarmed;

            if (matchInfo.IsAlarmed)
            {
                DateTime notifyTime = matchInfo.MatchTime.AddMinutes(-5);
                if (notifyTime < DateTime.Now)
                    notifyTime = DateTime.Now.AddSeconds(5);

                var notification = new NotificationRequest
                {
                    NotificationId = matchInfo.Id,
                    Title = LocalizeString.Match_Begin_Soon,
                    Description = $"{matchInfo.HomeName} vs {matchInfo.AwayName}",
                    ReturningData = matchInfo.JsonSerialize(),
                    NotifyTime = DateTime.Now.AddSeconds(5), // notifyTime
                    Android = new AndroidOptions
                    {
                        IconName = "ic_soccer_alarm",
                    },
                };

                NotificationCenter.Current.Show(notification);
            }

            matchInfo.OnPropertyChanged("IsAlarmed");

            var message = matchInfo.IsAlarmed ? LocalizeString.Set_Alarm : LocalizeString.Cancle_Alarm;
            UserDialogs.Instance.Toast(message);

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand<FootballMatchInfo>((e) => TouchBookmarkButton(e)); }

        private async void TouchBookmarkButton(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            matchInfo.Order = 0;
            matchInfo.StoredTime = DateTime.Now;
            matchInfo.IsBookmarked = !matchInfo.IsBookmarked;

            // Add Bookmark
            if (matchInfo.IsBookmarked)
                await _bookmarkService.AddBookmark<FootballMatchInfo>(matchInfo, SportsType.Football, BookMarkType.Bookmark_Match);
            else
                await _bookmarkService.RemoveBookmark<FootballMatchInfo>(matchInfo, SportsType.Football, BookMarkType.Bookmark_Match);

            var message = matchInfo.IsBookmarked ? LocalizeString.Set_Bookmark : LocalizeString.Delete_Bookmark;
            UserDialogs.Instance.Toast(message);

            matchInfo.OnPropertyChanged("IsBookmarked");

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchListViewModel(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        #endregion Constructors
    }
}