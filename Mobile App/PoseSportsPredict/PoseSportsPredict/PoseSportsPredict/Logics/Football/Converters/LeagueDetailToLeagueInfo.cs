using PosePacket.Service.Football.Models;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class LeagueDetailToLeagueInfo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FootballLeagueInfo returnValue = null;
            if (value is FootballLeagueDetail leagueDetail)
            {
                // 기본 이미지 설정
                if (string.IsNullOrEmpty(leagueDetail.Country.Logo))
                    leagueDetail.Country.Logo = "img_world.png";
                if (string.IsNullOrEmpty(leagueDetail.Logo))
                    leagueDetail.Logo = leagueDetail.Country.Logo;

                returnValue = new FootballLeagueInfo
                {
                    CountryName = leagueDetail.Country.Name,
                    CountryLogo = leagueDetail.Country.Logo,
                    LeagueName = leagueDetail.Name,
                    LeagueLogo = leagueDetail.Logo,
                    LeagueType = leagueDetail.LeagueType,
                    SeasonStartDate = leagueDetail.SeasonStartDate,
                    SeasonEndDate = leagueDetail.SeasonEndDate,
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