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
    public class FixtureDetailToLastFormConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                throw new ArgumentException("parameter");

            var myTeamId = (short)parameter;

            FootballLastForm returnValue = null;

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

                returnValue = new FootballLastForm
                {
                    FixtureId = fixtureDetail.FixtureId,
                    LeagueName = fixtureDetail.League.Name,
                    OpposingTeamLogo = myTeamId == fixtureDetail.HomeTeam.Id ? fixtureDetail.AwayTeam.Logo : fixtureDetail.HomeTeam.Logo,
                    OpposingTeamName = myTeamId == fixtureDetail.HomeTeam.Id ? fixtureDetail.AwayTeam.Name : fixtureDetail.HomeTeam.Name,
                    IsHomeMatch = myTeamId == fixtureDetail.HomeTeam.Id,
                    HomeScore = fixtureDetail.HomeTeamScore,
                    AwayScore = fixtureDetail.AwayTeamScore,
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
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value != 0 ? true : false;
        }
    }
}