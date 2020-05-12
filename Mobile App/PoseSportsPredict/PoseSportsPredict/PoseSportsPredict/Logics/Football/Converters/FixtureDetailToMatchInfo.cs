using PosePacket.Service.Football.Models;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class FixtureDetailToMatchInfo
    {
        public FootballMatchInfo Convert(FootballFixtureDetail fixtureDetail)
        {
            // 기본 이미지 설정
            if (string.IsNullOrEmpty(fixtureDetail.League.Country.Logo))
                fixtureDetail.League.Country.Logo = "img_world.png";
            if (string.IsNullOrEmpty(fixtureDetail.League.Logo))
                fixtureDetail.League.Logo = fixtureDetail.League.Country.Logo;

            if (string.IsNullOrEmpty(fixtureDetail.HomeTeam.Logo))
                fixtureDetail.HomeTeam.Logo = "img_football.png";
            if (string.IsNullOrEmpty(fixtureDetail.AwayTeam.Logo))
                fixtureDetail.AwayTeam.Logo = "img_football.png";

            var returnValue = new FootballMatchInfo
            {
                Id = fixtureDetail.FixtureId,
                League_CountryName = fixtureDetail.League.Country.Name,
                League_CountryLogo = fixtureDetail.League.Country.Logo,
                LeagueName = fixtureDetail.League.Name,
                LeagueLogo = fixtureDetail.League.Logo,
                LeagueType = fixtureDetail.League.LeagueType,

                HomeTeamId = fixtureDetail.HomeTeam.Id,
                HomeName = fixtureDetail.HomeTeam.Name,
                HomeLogo = fixtureDetail.HomeTeam.Logo,
                Home_CountryName = fixtureDetail.HomeTeam.CountryName,
                HomeScore = fixtureDetail.HomeTeamScore,

                AwayTeamId = fixtureDetail.AwayTeam.Id,
                AwayName = fixtureDetail.AwayTeam.Name,
                AwayLogo = fixtureDetail.AwayTeam.Logo,
                Away_CountryName = fixtureDetail.AwayTeam.CountryName,
                AwayScore = fixtureDetail.AwayTeamScore,

                MatchStatus = fixtureDetail.MatchStatus,
                MatchTime = fixtureDetail.MatchTime.ToLocalTime(),
                Round = fixtureDetail.Round,

                IsAlarmed = false,
                IsBookmarked = false,
            };

            return returnValue;
        }
    }
}