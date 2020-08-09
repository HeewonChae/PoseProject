using PosePacket.Service.Enum;
using PosePacket.Service.Football.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    internal class StandingsDetailToStandingsInfo
    {
        public FootballStandingsInfo Convert(FootballStandingsDetail standingsDetail)
        {
            // 기본 이미지 설정
            if (string.IsNullOrEmpty(standingsDetail.League.Country.Logo))
                standingsDetail.League.Country.Logo = "img_world.png";
            if (string.IsNullOrEmpty(standingsDetail.League.Logo))
                standingsDetail.League.Logo = standingsDetail.League.Country.Logo;

            if (string.IsNullOrEmpty(standingsDetail.Team.Logo))
                standingsDetail.Team.Logo = "img_football.png";

            var returnValue = new FootballStandingsInfo()
            {
                LeagueInfo = new FootballLeagueInfo
                {
                    CountryLogo = standingsDetail.League.Country.Logo,
                    CountryName = standingsDetail.League.Country.Name,
                    LeagueLogo = standingsDetail.League.Logo,
                    LeagueName = standingsDetail.League.Name,
                    LeagueType = standingsDetail.League.LeagueType,
                },

                TeamInfo = new FootballTeamInfo
                {
                    TeamId = standingsDetail.Team.Id,
                    TeamLogo = standingsDetail.Team.Logo,
                    TeamName = standingsDetail.Team.Name,
                    CountryName = standingsDetail.Team.CountryName,
                },

                Rank = standingsDetail.Rank,
                RankColor = Color.Transparent,
                Points = standingsDetail.Points,
                Group = standingsDetail.Group,
                Description = standingsDetail.Description,
                Played = standingsDetail.Played,
                Win = standingsDetail.Win,
                Draw = standingsDetail.Draw,
                Lose = standingsDetail.Lose,
                GoalFor = standingsDetail.GoalFor,
                GoalAgainst = standingsDetail.GoalAgainst,
                GoalDifference = standingsDetail.GoalDifference,
            };

            returnValue.Form = new List<FootballFormInfo>();
            standingsDetail.Form.Reverse();
            foreach (var form in standingsDetail.Form)
            {
                FootballFormInfo footballForm = new FootballFormInfo();

                if (form.Equals('W'))
                {
                    footballForm.Result = MatchResultType.Win;
                }
                else if (form.Equals('L'))
                {
                    footballForm.Result = MatchResultType.Lose;
                }
                else
                {
                    footballForm.Result = MatchResultType.Draw;
                }

                returnValue.Form.Add(footballForm);
            }

            var lastForm = returnValue.Form.LastOrDefault();
            if (lastForm != null)
                lastForm.IsLastMatch = true;

            return returnValue;
        }
    }
}