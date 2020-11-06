using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class VIPMatchInfoToMatchInfo
    {
        public FootballMatchInfo Convert(FootballVIPMatchInfo vipMatchInfo)
        {
            var return_value = new FootballMatchInfo
            {
                Id = vipMatchInfo.Id,
                League_CountryName = vipMatchInfo.League_CountryName,
                League_CountryLogo = vipMatchInfo.League_CountryLogo,
                LeagueName = vipMatchInfo.LeagueName,
                LeagueLogo = vipMatchInfo.LeagueLogo,
                LeagueType = vipMatchInfo.LeagueType,

                HomeTeamId = vipMatchInfo.HomeTeamId,
                HomeName = vipMatchInfo.HomeName,
                HomeLogo = vipMatchInfo.HomeLogo,
                Home_CountryName = vipMatchInfo.Home_CountryName,
                HomeScore = vipMatchInfo.HomeScore,

                AwayTeamId = vipMatchInfo.AwayTeamId,
                AwayName = vipMatchInfo.AwayName,
                AwayLogo = vipMatchInfo.AwayLogo,
                Away_CountryName = vipMatchInfo.Away_CountryName,
                AwayScore = vipMatchInfo.AwayScore,

                MatchStatus = vipMatchInfo.MatchStatus,
                MatchTime = vipMatchInfo.MatchTime,
                Round = vipMatchInfo.Round,

                IsAlarmed = vipMatchInfo.IsAlarmed,
                IsBookmarked = vipMatchInfo.IsBookmarked,

                IsPredicted = vipMatchInfo.IsPredicted,
                IsRecommended = vipMatchInfo.IsRecommended,
                MaxRating = vipMatchInfo.MaxRating,
                HasOdds = vipMatchInfo.HasOdds,
            };

            return return_value;
        }
    }
}