using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 공용 열거형 타입들
/// </summary>
namespace PosePacket.Service.Enum
{
    public enum SportsType
    {
        Football,
    }

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

    public enum TeamCampType
    {
        Any,
        Home,
        Away,
    }

    public enum YesNoType
    {
        Yes,
        No,
    }

    public enum MemberRoleType
    {
        Guest,
        Regular,
        Promotion,
        Diamond,
        VIP,
        VVIP,
        Admin,
    }

    public enum StoreType
    {
        GooglePlay,
        AppStore,
    }
}