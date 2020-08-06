using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class MatchInfoToTeamInfo
    {
        public FootballTeamInfo Convert(FootballMatchInfo matchInfo, TeamCampType teamType)
        {
            if (matchInfo == null)
                throw new ArgumentException("matchInfo");

            if (teamType == TeamCampType.Any)
                throw new ArgumentException("teamType");

            var returnValue = new FootballTeamInfo
            {
                TeamId = teamType == TeamCampType.Home ? matchInfo.HomeTeamId : matchInfo.AwayTeamId,
                TeamName = teamType == TeamCampType.Home ? matchInfo.HomeName : matchInfo.AwayName,
                TeamLogo = teamType == TeamCampType.Home ? matchInfo.HomeLogo : matchInfo.AwayLogo,
                CountryName = teamType == TeamCampType.Home ? matchInfo.Home_CountryName : matchInfo.Away_CountryName,
                IsBookmarked = false,
            };

            return returnValue;
        }
    }
}