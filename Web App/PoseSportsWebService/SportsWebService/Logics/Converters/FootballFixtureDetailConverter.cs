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
    public class FootballFixtureDetailConverter : IValueConverter<FootballFixtureDetail>, Singleton.INode
    {
        public FootballFixtureDetail Convert(object value, params object[] parameters)
        {
            var result = new FootballFixtureDetail();

            if (value is DB_FootballFixtureDetail db_fixtureDetail)
            {
                db_fixtureDetail.MatchStatus.TryParseEnum(out FootballMatchStatusType statusType);
                db_fixtureDetail.LeagueType.TryParseEnum(out FootballLeagueType leagueType);

                result.Country = new FootballFixtureDetail.DataInfo
                {
                    Name = db_fixtureDetail.CountryName,
                    Logo = db_fixtureDetail.CountryLogo,
                };
                result.League = new FootballFixtureDetail.DataInfo
                {
                    Name = db_fixtureDetail.LeagueName,
                    Logo = db_fixtureDetail.LeagueLogo,
                };
                result.HomeTeam = new FootballFixtureDetail.TeamInfo
                {
                    Id = db_fixtureDetail.HomeTeamId,
                    Name = db_fixtureDetail.HomeTeamName,
                    Logo = db_fixtureDetail.HomeTeamLogo,
                    Score = db_fixtureDetail.HomeTeamScore,
                };
                result.AwayTeam = new FootballFixtureDetail.TeamInfo
                {
                    Id = db_fixtureDetail.AwayTeamId,
                    Name = db_fixtureDetail.AwayTeamName,
                    Logo = db_fixtureDetail.AwayTeamLogo,
                    Score = db_fixtureDetail.AwayTeamScore
                };
                result.Round = db_fixtureDetail.Round;
                result.FixtureId = db_fixtureDetail.FixtureId;
                result.MatchStatus = statusType;
                result.MatchTime = db_fixtureDetail.MatchTime;
                result.LeagueType = leagueType;
            }
            return result;
        }
    }
}