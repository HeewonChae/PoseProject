using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class MatchInfoToLeagueInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FootballLeagueInfo returnValue = null;

            var matchInfo = value as FootballMatchInfo;
            if (matchInfo != null)
            {
                returnValue = new FootballLeagueInfo
                {
                    CountryName = matchInfo.CountryName,
                    CountryLogo = matchInfo.CountryLogo,
                    LeagueName = matchInfo.LeagueName,
                    LeagueLogo = matchInfo.LeagueLogo,
                    LeagueType = matchInfo.LeagueType,
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