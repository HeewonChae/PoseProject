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
        public FixtureDetailToMatchInfoConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FootballMatchInfo returnValue = null;

            var fixtureDetail = value as FootballFixtureDetail;
            if (fixtureDetail != null)
            {
                returnValue = new FootballMatchInfo
                {
                    Id = fixtureDetail.FixtureId,
                    CountryName = fixtureDetail.Country.Name,
                    CountryLogo = fixtureDetail.Country.Logo,
                    LeagueName = fixtureDetail.League.Name,
                    LeagueLogo = fixtureDetail.League.Logo,

                    HomeTeamId = fixtureDetail.HomeTeam.Id,
                    HomeName = fixtureDetail.HomeTeam.Name,
                    HomeLogo = fixtureDetail.HomeTeam.Logo,
                    HomeScore = fixtureDetail.HomeTeam.Score,

                    AwayTeamId = fixtureDetail.AwayTeam.Id,
                    AwayName = fixtureDetail.AwayTeam.Name,
                    AwayLogo = fixtureDetail.AwayTeam.Logo,
                    AwayScore = fixtureDetail.AwayTeam.Score,

                    MatchStatus = fixtureDetail.MatchStatus,
                    MatchTime = fixtureDetail.MatchTime,
                    LeagueType = fixtureDetail.LeagueType,

                    IsAlarmed = false,
                    IsBookmarked = false,
                };
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value != 0 ? true : false;
        }
    }
}