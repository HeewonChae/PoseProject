using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using Plugin.LocalNotification;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services.MessagingCenterMessageType;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using Shiny;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using Xamarin.Forms;
using PacketModels = PosePacket.Service.Football.Models;

namespace PoseSportsPredict.ViewModels.Football.Match
{
    public class FootballMatchListViewModel : BaseViewModel
    {
        #region Services

        public ISQLiteService _sqliteService;

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

            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);

            SetIsBusy(false);
        }

        public ICommand TouchAlarmButtonCommand { get => new RelayCommand<FootballMatchInfo>((e) => TouchAlarmButton(e)); }

        private void TouchAlarmButton(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

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

            matchInfo.IsAlarmed = !matchInfo.IsAlarmed;

            var message = matchInfo.IsAlarmed ? LocalizeString.Set_Alarm : LocalizeString.Cancle_Alarm;
            UserDialogs.Instance.Toast(message);

            matchInfo.OnPropertyChanged("IsAlarmed");

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
                await _sqliteService.InsertOrUpdateAsync<FootballMatchInfo>(matchInfo);
            else
                await _sqliteService.DeleteAsync<FootballMatchInfo>(matchInfo.PrimaryKey);

            MessagingCenter.Send(this, FootballMessageType.Update_Bookmark_Match.ToString(), matchInfo);

            var message = matchInfo.IsBookmarked ? LocalizeString.Set_Bookmark : LocalizeString.Delete_Bookmark;
            UserDialogs.Instance.Toast(message);

            matchInfo.OnPropertyChanged("IsBookmarked");

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchListViewModel(ISQLiteService sqliteService)
        {
            _sqliteService = sqliteService;
        }

        #endregion Constructors
    }
}