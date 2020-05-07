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

            var teamType = (TeamCampType)parameter;

            FootballTeamInfo returnValue = null;

            if (value is FootballMatchInfo matchInfo)
            {
                returnValue = new FootballTeamInfo
                {
                    TeamId = teamType == TeamCampType.Home ? matchInfo.HomeTeamId : matchInfo.AwayTeamId,
                    TeamName = teamType == TeamCampType.Home ? matchInfo.HomeName : matchInfo.AwayName,
                    TeamLogo = teamType == TeamCampType.Home ? matchInfo.HomeLogo : matchInfo.AwayLogo,
                    CountryName = teamType == TeamCampType.Home ? matchInfo.Home_CountryName : matchInfo.Away_CountryName,
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