using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football.Models.Enums
{
    public enum FootballLeagueType
    {
        _NONE_,
        League,
        Cup,
        _MAX_,
    }

    public enum FootballMatchStatusType
    {
        _NONE_,
        TBD, // Time To Be Defined
        NS, // Not Started
        FH, // First Half, Kick Off
        HT, // Halftime
        SH, // Second Half, 2nd Half Started
        ET, // Extra Time
        P, // Penalty In Progress
        FT, // Match Finished
        AET, // Match Finished After Extra Time
        PEN, // Match Finished After Penalty
        BT, // Break Time (in Extra Time)
        SUSP, // Match Suspended
        INT, // Match Interrupted
        PST, // Match Postponed
        CANC, // Match Cancelled
        ABD, // Match Abandoned
        AWD, // Technical Loss
        WO, // WalkOver
        _MAX_
    }

    public enum FootballBookmakerType
    {
        _NONE_,
        TenBet = 1,
        Marathonbet = 2,
        Betfair = 3,
        Bwin = 6,
        WilliamHill = 7,
        Bet365 = 8,
        Dafabet = 9,
        Ladbrokes = 10,
        OnexBet = 11,
        Interwetten = 15,
        _MAX_
    }

    public enum FootballOddsLabelType
    {
        _NONE_,
        Match_Winner = 1,
        _MAX_
    }

    // Both_Teams_Score = 8,
    // Goals_Over_Under = 5,
    // Double_Chance = 12,

    public enum FootballPredictionType
    {
        _NONE_,
        Final_Score = 1,
        Match_Winner = 2,
        Both_Teams_to_Score = 3,
        Under_Over = 4,
        _MAX_
    }

    public enum FootballUnderOverType
    {
        _NONE_,
        UNDER_1_5,
        OVER_1_5,
        UNDER_2_5,
        OVER_2_5,
        UNDER_3_5,
        OVER_3_5,
        UNDER_4_5,
        OVER_4_5,
        _MAX_
    }

    public enum FootballMatchWinnerType
    {
        _NONE_,
        Win,
        Lose,
        WinOrDraw,
        WinOrLose,
        DrawOrLose,
        _MAX_,
    }
}