using PosePacket.Service.Football.Models;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class FixtureDetailToMatchInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FootballMatchInfo returnValue = null;

            if (value is FootballFixtureDetail fixtureDetail)
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

                returnValue = new FootballMatchInfo
                {
                    Id = fixtureDetail.FixtureId,
                    League_CountryName = fixtureDetail.League.Country.Name,
                    League_CountryLogo = fixtureDetail.League.Country.Logo,
                    LeagueName = fixtureDetail.League.Name,
                    LeagueLogo = fixtureDetail.League.Logo,
                    Round = fixtureDetail.Round,

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
                    LeagueType = fixtureDetail.League.LeagueType,

                    IsAlarmed = false,
                    IsBookmarked = false,
                };
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}