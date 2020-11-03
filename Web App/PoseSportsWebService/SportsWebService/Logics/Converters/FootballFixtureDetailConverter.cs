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

                result.League = new FootballLeagueDetail
                {
                    Country = new FootballCountryDetail
                    {
                        Name = db_fixtureDetail.League_CountryName,
                        Logo = db_fixtureDetail.League_CountryLogo
                    },
                    Name = db_fixtureDetail.LeagueName,
                    Logo = db_fixtureDetail.LeagueLogo,
                    LeagueType = leagueType,
                };
                result.HomeTeam = new FootballTeamDetail
                {
                    Id = db_fixtureDetail.HomeTeamId,
                    Name = db_fixtureDetail.HomeTeamName,
                    Logo = db_fixtureDetail.HomeTeamLogo,
                    CountryName = db_fixtureDetail.HomeTeam_CountryName,
                };
                result.AwayTeam = new FootballTeamDetail
                {
                    Id = db_fixtureDetail.AwayTeamId,
                    Name = db_fixtureDetail.AwayTeamName,
                    Logo = db_fixtureDetail.AwayTeamLogo,
                    CountryName = db_fixtureDetail.AwayTeam_CountryName,
                };
                result.Round = db_fixtureDetail.Round;
                result.FixtureId = db_fixtureDetail.FixtureId;
                result.MatchStatus = statusType;
                result.MatchTime = db_fixtureDetail.MatchTime;
                result.HomeTeamScore = db_fixtureDetail.HomeTeamScore;
                result.AwayTeamScore = db_fixtureDetail.AwayTeamScore;

                result.IsPredicted = db_fixtureDetail.IsPredicted;

                result.IsRecommended = db_fixtureDetail.IsRecommended;
                result.MaxGrade = (byte)db_fixtureDetail.MaxGrade;

                result.HasOdds = db_fixtureDetail.HasOdds;
            }
            return result;
        }
    }
}