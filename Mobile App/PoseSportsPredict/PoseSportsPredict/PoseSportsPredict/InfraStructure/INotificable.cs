using PosePacket.Service.Enum;
using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure
{
    public interface INotificable
    {
        int Id { get; set; }
        bool IsAlarmed { get; }
        SportsType SportsType { get; set; }
        NotificationType NotificationType { get; set; }
    }
}