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
    public class MatchInfoToTeamInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                throw new ArgumentException("parameter");

            var teamType = (TeamType)parameter;

            FootballTeamInfo returnValue = null;

            if (value is FootballMatchInfo matchInfo)
            {
                returnValue = new FootballTeamInfo
                {
                    TeamId = teamType == TeamType.Home ? matchInfo.HomeTeamId : matchInfo.AwayTeamId,
                    TeamName = teamType == TeamType.Home ? matchInfo.HomeName : matchInfo.AwayName,
                    TeamLogo = teamType == TeamType.Home ? matchInfo.HomeLogo : matchInfo.AwayLogo,
                    CountryName = matchInfo.CountryName,
                    CountryLogo = matchInfo.CountryLogo,
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