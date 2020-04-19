using PosePacket.Service.Football.Models;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    internal class StandingsDetailToStandingsInfo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FootballStandingsInfo returnValue = null;

            if (value is FootballStandingsDetail standingsDetail)
            {
                // 기본 이미지 설정
                if (string.IsNullOrEmpty(standingsDetail.Country.Logo))
                    standingsDetail.Country.Logo = "img_world.png";
                if (string.IsNullOrEmpty(standingsDetail.League.Logo))
                    standingsDetail.League.Logo = standingsDetail.Country.Logo;
                if (string.IsNullOrEmpty(standingsDetail.Team.Logo))
                    standingsDetail.Team.Logo = "img_football.png";

                returnValue = new FootballStandingsInfo()
                {
                    CountryLogo = standingsDetail.Country.Logo,
                    CountryName = standingsDetail.Country.Name,
                    LeagueLogo = standingsDetail.League.Logo,
                    LeagueName = standingsDetail.League.Name,
                    TeamId = standingsDetail.Team.Id,
                    TeamLogo = standingsDetail.Team.Logo,
                    TeamName = standingsDetail.Team.Name,
                    Rank = standingsDetail.Rank,
                    Points = standingsDetail.Points,
                    Group = standingsDetail.Group,
                    Description = standingsDetail.Description,
                    Win = standingsDetail.Win,
                    Draw = standingsDetail.Draw,
                    Lose = standingsDetail.Lose,
                    GoalFor = standingsDetail.GoalFor,
                    GoalAgainst = standingsDetail.GoalAgainst,
                    GoalDifference = standingsDetail.GoalDifference,
                    Form = standingsDetail.Form,
                };
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}