using PosePacket.Service.Enum;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.InfraStructure
{
    public interface INotificationService
    {
        Task Initialize();

        Task<NotificationInfo> GetNotification(int id, SportsType sportsType, NotificationType notificationType);

        Task<List<NotificationInfo>> GetAllNotification(SportsType sportsType, NotificationType notificationType);

        Task<bool> AddNotification(NotificationInfo item);

        Task<bool> DeleteNotification(int id, SportsType sportsType, NotificationType notificationType);
    }
}