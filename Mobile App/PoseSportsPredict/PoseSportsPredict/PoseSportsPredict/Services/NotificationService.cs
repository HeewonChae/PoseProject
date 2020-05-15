using Acr.UserDialogs;
using Plugin.LocalNotification;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.Services
{
    public class NotificationService : PoseSportsPredict.InfraStructure.INotificationService
    {
        private ISQLiteService _sqliteService;

        public NotificationService(ISQLiteService sqliteService)
        {
            _sqliteService = sqliteService;
        }

        public async Task Initialize()
        {
            var allitems = await _sqliteService.SelectAllAsync<NotificationInfo>();
            var deleteItems = allitems.Where(elem => elem.NotifyTime < DateTime.Now);
            var registerItems = allitems.Except(deleteItems);

            // 지난 알람 삭제
            foreach (var deleteitem in deleteItems)
            {
                await _sqliteService.DeleteAsync<NotificationInfo>(deleteitem.PrimaryKey);
                NotificationCenter.Current.Cancel(deleteitem.Id);
            }

            // 노티 재등록
            foreach (var registerItem in registerItems)
            {
                var req = new NotificationRequest
                {
                    NotificationId = registerItem.Id,
                    Title = registerItem.Title,
                    Description = registerItem.Description,
                    ReturningData = registerItem.IntentData,
                    NotifyTime = registerItem.NotifyTime,
                    Android = new AndroidOptions
                    {
                        IconName = registerItem.IconName,
                        ChannelId = AppConfig.Psoe_Noti_Channel_01,
                    },
                };

                NotificationCenter.Current.Show(req);
            }
        }

        public async Task<NotificationInfo> GetNotification(int id, SportsType sportsType, NotificationType notificationType)
        {
            return await _sqliteService.SelectAsync<NotificationInfo>(
                NotificationInfo.BuildPrimaryKey(id, sportsType, notificationType));
        }

        public async Task<List<NotificationInfo>> GetAllNotification(SportsType sportsType, NotificationType notificationType)
        {
            var allitems = await _sqliteService.SelectAllAsync<NotificationInfo>();
            return allitems.Where(elem => elem.SportsType == sportsType && elem.NotificationType == notificationType).ToList();
        }

        public async Task<bool> AddNotification(NotificationInfo item)
        {
            // For Debugging
            // item.NotifyTime = DateTime.Now.AddMinutes(2);

            var ret = await _sqliteService.InsertOrUpdateAsync<NotificationInfo>(item);
            Debug.Assert(ret != 0);

            item.SetIsAlarmed(true);

            var req = new NotificationRequest
            {
                NotificationId = item.Id,
                Title = item.Title,
                Description = item.Description,
                ReturningData = item.IntentData,
                NotifyTime = item.NotifyTime,
                Android = new AndroidOptions
                {
                    IconName = item.IconName,
                    ChannelId = AppConfig.Psoe_Noti_Channel_01,
                },
            };
            NotificationCenter.Current.Show(req);

            var message = this.BuildNotificationMessage(item.SportsType, item.NotificationType);
            MessagingCenter.Send(this, message, item);

            UserDialogs.Instance.Toast(LocalizeString.Set_Alarm);

            return true;
        }

        public async Task<bool> DeleteNotification(int id, SportsType sportsType, NotificationType notificationType)
        {
            var found = await _sqliteService.SelectAsync<NotificationInfo>(
                 NotificationInfo.BuildPrimaryKey(id, sportsType, notificationType));

            var ret = await _sqliteService.DeleteAsync<NotificationInfo>(found.PrimaryKey);
            Debug.Assert(ret != 0);

            NotificationCenter.Current.Cancel(found.Id);

            found.SetIsAlarmed(false);

            string message = this.BuildNotificationMessage(found.SportsType, found.NotificationType);
            MessagingCenter.Send(this, message, found);

            UserDialogs.Instance.Toast(LocalizeString.Cancle_Alarm);
            return true;
        }
    }
}