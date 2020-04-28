using PosePacket.Service.Football.Models;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class TeamDetailToTeamInfo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FootballTeamInfo returnValue = null;
            if (value is FootballTeamDetail teamDetail)
            {
                // 기본 이미지 설정
                if (string.IsNullOrEmpty(teamDetail.Logo))
                    teamDetail.Logo = "img_football.png";

                returnValue = new FootballTeamInfo
                {
                    TeamId = teamDetail.Id,
                    TeamName = teamDetail.Name,
                    TeamLogo = teamDetail.Logo,
                    CountryName = teamDetail.CountryName,
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