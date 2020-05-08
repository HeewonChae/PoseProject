using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class CoverageLeagueToLeagueInfo
    {
        public FootballLeagueInfo Convert(CoverageLeague coverageLeague)
        {
            var returnValue = new FootballLeagueInfo
            {
                CountryName = coverageLeague.CountryName,
                CountryLogo = coverageLeague.CountryLogo,
                LeagueName = coverageLeague.LeagueName,
                LeagueLogo = coverageLeague.LeagueLogo,
                LeagueType = coverageLeague.LeagueType,
                IsBookmarked = false,
            };

            return returnValue;
        }
    }
}