using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Models.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models
{
    public class NotificationInfo : ISQLiteStorable, INotificable
    {
        #region ISQLiteStorable

        private string _primaryKey;
        private bool _isAlarmed;

        [PrimaryKey]
        public string PrimaryKey { get => NotificationInfo.BuildPrimaryKey(this.Id, this.SportsType, this.NotificationType); set => _primaryKey = value; }

        public int Order { get; set; }
        public DateTime StoredTime { get; set; }

        #endregion ISQLiteStorable

        #region INotificable

        public int Id { get; set; }
        public bool IsAlarmed => _isAlarmed;
        public SportsType SportsType { get; set; }
        public NotificationType NotificationType { get; set; }

        #endregion INotificable

        public DateTime NotifyTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IntentData { get; set; }
        public string IconName { get; set; }

        public static string BuildPrimaryKey(int id, SportsType sportsType, NotificationType notificationType)
        {
            return $"{id}:{sportsType}:{notificationType}";
        }

        public void SetIsAlarmed(bool value)
        {
            _isAlarmed = value;
        }
    }
}