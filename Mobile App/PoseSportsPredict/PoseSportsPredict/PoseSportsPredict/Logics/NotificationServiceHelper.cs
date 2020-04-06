using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Logics
{
    public static class NotificationServiceHelper
    {
        public static string BuildNotificationMessage(this INotificationService bookmarkService, SportsType sportsType, NotificationType notificationType)
        {
            return $"{sportsType}-{notificationType}";
        }
    }
}