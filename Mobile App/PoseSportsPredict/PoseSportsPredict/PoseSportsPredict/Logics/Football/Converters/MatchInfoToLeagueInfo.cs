using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class MatchInfoToLeagueInfo
    {
        public FootballLeagueInfo Convert(FootballMatchInfo matchInfo)
        {
            var returnValue = new FootballLeagueInfo
            {
                CountryName = matchInfo.League_CountryName,
                CountryLogo = matchInfo.League_CountryLogo,
                LeagueName = matchInfo.LeagueName,
                LeagueLogo = matchInfo.LeagueLogo,
                LeagueType = matchInfo.LeagueType,
                IsBookmarked = false,
            };

            return returnValue;
        }
    }
}