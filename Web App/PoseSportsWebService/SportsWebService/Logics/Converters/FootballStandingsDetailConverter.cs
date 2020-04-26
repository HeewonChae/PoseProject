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
                db_standingsDetail.LeagueType.TryParseEnum<FootballLeagueType>(out FootballLeagueType leagueType);

                result.League = new FootballLeagueDetail
                {
                    Country = new FootballCountryDetail
                    {
                        Name = db_standingsDetail.League_CountryName,
                        Logo = db_standingsDetail.League_CountryLogo,
                    },
                    Name = db_standingsDetail.LeagueLogo,
                    Logo = db_standingsDetail.LeagueName,
                    LeagueType = leagueType,
                };

                result.Team = new FootballTeamDetail
                {
                    Id = db_standingsDetail.TeamId,
                    Logo = db_standingsDetail.TeamLogo,
                    Name = db_standingsDetail.TeamName,
                    CountryName = db_standingsDetail.Team_CountryName,
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