using PosePacket.Service.Football.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class MatchInfoToFormInfo
    {
        public FootballFormInfo Convert(FootballMatchInfo matchInfo, short teamId)
        {
            if (matchInfo == null)
                throw new ArgumentException("matchInfo");

            var returnValue = new FootballFormInfo
            {
                FixtureId = matchInfo.Id,
                LeagueName = matchInfo.LeagueName,
                OpposingTeamLogo = teamId == matchInfo.HomeTeamId ? matchInfo.AwayLogo : matchInfo.HomeLogo,
                OpposingTeamName = teamId == matchInfo.HomeTeamId ? matchInfo.AwayName : matchInfo.HomeName,
                IsHomeMatch = teamId == matchInfo.HomeTeamId,
                HomeScore = matchInfo.HomeScore,
                AwayScore = matchInfo.AwayScore,
            };

            if (returnValue.HomeScore == returnValue.AwayScore)
            {
                returnValue.Result = MatchResultType.Draw;
            }
            else if (returnValue.HomeScore > returnValue.AwayScore)
            {
                returnValue.Result = returnValue.IsHomeMatch ? MatchResultType.Win : MatchResultType.Lose;
            }
            else
            {
                returnValue.Result = returnValue.IsHomeMatch ? MatchResultType.Lose : MatchResultType.Win;
            }

            return returnValue;
        }
    }
}