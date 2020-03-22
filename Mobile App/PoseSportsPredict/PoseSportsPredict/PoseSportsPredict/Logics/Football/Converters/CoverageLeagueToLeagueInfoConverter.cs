using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class CoverageLeagueToLeagueInfoConverter : IValueConverter
    {
        public CoverageLeagueToLeagueInfoConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FootballLeagueInfo returnValue = null;

            var coverageLeague = value as CoverageLeague;
            if (coverageLeague != null)
            {
                returnValue = new FootballLeagueInfo
                {
                    CountryName = coverageLeague.CountryName,
                    CountryLogo = coverageLeague.CountryLogo,
                    LeagueName = coverageLeague.LeagueName,
                    LeagueLogo = coverageLeague.LeagueLogo,
                    LeagueType = coverageLeague.LeagueType,
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