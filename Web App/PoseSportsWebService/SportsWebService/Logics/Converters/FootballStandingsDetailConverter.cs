using LogicCore.Converter;
using LogicCore.Utility;
using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using Repository.Mysql.FootballDB.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Logics.Converters
{
    public class FootballStandingsDetailConverter : IValueConverter<FootballStandingsDetail>, Singleton.INode
    {
        public FootballStandingsDetail Convert(object value, params object[] parameters)
        {
            var result = new FootballStandingsDetail();

            if (value is DB_FootballStandingsDetail db_standingsDetail)
            {
                result.Country = new FootballStandingsDetail.DataInfo()
                {
                    Logo = db_standingsDetail.CountryLogo,
                    Name = db_standingsDetail.CountryName,
                };

                result.League = new FootballStandingsDetail.DataInfo()
                {
                    Logo = db_standingsDetail.LeagueLogo,
                    Name = db_standingsDetail.LeagueName,
                };

                db_standingsDetail.LeagueType.TryParseEnum<FootballLeagueType>(out FootballLeagueType leagueType);
                result.LeagueType = leagueType;

                result.Team = new FootballStandingsDetail.TeamInfo()
                {
                    Id = db_standingsDetail.TeamId,
                    Logo = db_standingsDetail.TeamLogo,
                    Name = db_standingsDetail.TeamName,
                };

                result.Rank = db_standingsDetail.Rank;
                result.Points = db_standingsDetail.Points;
                result.Group = db_standingsDetail.Group;
                result.Description = db_standingsDetail.Description;
                result.Played = db_standingsDetail.Played;
                result.Win = db_standingsDetail.Win;
                result.Draw = db_standingsDetail.Draw;
                result.Lose = db_standingsDetail.Lose;
                result.GoalFor = db_standingsDetail.GoalFor;
                result.GoalAgainst = db_standingsDetail.GoalAgainst;
                result.GoalDifference = (short)(result.GoalFor - result.GoalAgainst);
                result.Form = db_standingsDetail.Form.Select(elem => elem).ToList();
            }

            return result;
        }
    }
}