using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 공용 열거형 타입들
/// </summary>
namespace PosePacket.Service.Enum
{
    public enum MatchResultType
    {
        Win,
        Draw,
        Lose
    }

    public enum SearchFixtureStatusType
    {
        Finished,
        Scheduled,
    }
}