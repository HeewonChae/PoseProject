using LogicCore.Utility;
using LogicCore.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppModel = SportsAdminTool.Model;
using FootballDB = Repository.Mysql.FootballDB;
using ResourceModel = SportsAdminTool.Model.Resource;
using FootballLogic = SportsAdminTool.Logic.Football;

namespace SportsAdminTool.Logic.Database
{
    /// <summary>
    /// 간단한 FootballDB관련 CRUD 로직 처리
    /// </summary>
    public static class FootballDBFacade
    {
        #region Update

        public static bool UpdateCountry(params AppModel.Football.Country[] countries)
        {
            if (countries.Length == 0)
                return false;

            Dev.DebugString("Call DB - FootballFacade.UpdateCountry");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO country");
            sb.Append($" (`{nameof(FootballDB.Tables.Country.code)}`, " +
                $"`{nameof(FootballDB.Tables.Country.name)}`, " +
                $"`{nameof(FootballDB.Tables.Country.logo)}`, " +
                $"`{nameof(FootballDB.Tables.Country.upt_time)}`)");
            sb.Append("VALUES");

            for (int i = 0; i < countries.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var country = countries[i];
                sb.Append($"(\"{country.Code}\", " +
                    $"\"{country.Name}\", " +
                    $"\"{country.Logo}\", " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.Country.logo)} = VALUES({nameof(FootballDB.Tables.Country.logo)}), " +
                $"{nameof(FootballDB.Tables.Country.upt_time)} = VALUES({nameof(FootballDB.Tables.Country.upt_time)});");

            return ExecuteQuery(sb.ToString());
        }

        public static bool UpdateLeague(params AppModel.Football.League[] leagues)
        {
            if (leagues.Length == 0)
                return false;

            Dev.DebugString("Call DB - FootballFacade.UpdateLeague");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO league");
            sb.Append($" (`{nameof(FootballDB.Tables.League.id)}`, " +
                $"`{nameof(FootballDB.Tables.League.name)}`, " +
                $"`{nameof(FootballDB.Tables.League.type)}`, " +
                $"`{nameof(FootballDB.Tables.League.country_name)}`, " +
                $"`{nameof(FootballDB.Tables.League.is_current)}`, " +
                $"`{nameof(FootballDB.Tables.League.season_start)}`, " +
                $"`{nameof(FootballDB.Tables.League.season_end)}`, " +
                $"`{nameof(FootballDB.Tables.League.logo)}`, " +
                $"`{nameof(FootballDB.Tables.League.is_predict_coverage)}`, " +
                $"`{nameof(FootballDB.Tables.League.upt_time)}`)");
            sb.Append("VALUES");

            for (int i = 0; i < leagues.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var league = leagues[i];

                bool isCoverageLeage = league.Coverage != null && league.Coverage.FixtureCoverage.Statistics;

                sb.Append($"({league.LeagueId}, " +
                    $"\"{league.Name}\", " +
                    $"\"{league.Type}\", " +
                    $"\"{league.Country}\", " +
                    $"\"{league.IsCurrent}\", " +
                    $"\"{league.SeasonStart.ToString("yyyyMMddTHHmmss")}\", " +
                    $"\"{league.SeasonEnd.ToString("yyyyMMddTHHmmss")}\", " +
                    $"\"{league.Logo}\", " +
                    $"{isCoverageLeage}, " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.League.logo)} = VALUES({nameof(FootballDB.Tables.League.logo)}), " +
                $"{nameof(FootballDB.Tables.League.is_current)} = VALUES({nameof(FootballDB.Tables.League.is_current)}), " +
                $"{nameof(FootballDB.Tables.League.upt_time)} = VALUES({nameof(FootballDB.Tables.League.upt_time)});");

            return ExecuteQuery(sb.ToString());
        }

        public static bool UpdateCoverage(params AppModel.Football.League[] leagues)
        {
            if (leagues.Length == 0)
                return false;

            Dev.DebugString("Call DB - FootballFacade.UpdateCoverage");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO league_coverage");
            sb.Append($" (`{nameof(FootballDB.Tables.LeagueCoverage.league_id)}`, " +
                $"`{nameof(FootballDB.Tables.LeagueCoverage.standings)}`, " +
                $"`{nameof(FootballDB.Tables.LeagueCoverage.odds)}`, " +
                $"`{nameof(FootballDB.Tables.LeagueCoverage.fixture_statistics)}`, " +
                $"`{nameof(FootballDB.Tables.LeagueCoverage.players)}`, " +
                $"`{nameof(FootballDB.Tables.LeagueCoverage.lineups)}`, " +
                $"`{nameof(FootballDB.Tables.LeagueCoverage.predictions)}`, " +
                $"`{nameof(FootballDB.Tables.LeagueCoverage.upt_time)}`)");
            sb.Append("VALUES");

            for (int i = 0; i < leagues.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var league = leagues[i];

                sb.Append($"({league.LeagueId}, " +
                    $"{league.Coverage.Standings}, " +
                    $"{league.Coverage.Odds}, " +
                    $"{league.Coverage.FixtureCoverage.Statistics}, " +
                    $"{league.Coverage.Players}, " +
                    $"{league.Coverage.FixtureCoverage.Lineups}, " +
                    $"{league.Coverage.Predictions}, " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.LeagueCoverage.standings)} = VALUES({nameof(FootballDB.Tables.LeagueCoverage.standings)}), " +
                $"{nameof(FootballDB.Tables.LeagueCoverage.odds)} = VALUES({nameof(FootballDB.Tables.LeagueCoverage.odds)}), " +
                $"{nameof(FootballDB.Tables.LeagueCoverage.fixture_statistics)} = VALUES({nameof(FootballDB.Tables.LeagueCoverage.fixture_statistics)}), " +
                $"{nameof(FootballDB.Tables.LeagueCoverage.players)} = VALUES({nameof(FootballDB.Tables.LeagueCoverage.players)}), " +
                $"{nameof(FootballDB.Tables.LeagueCoverage.lineups)} = VALUES({nameof(FootballDB.Tables.LeagueCoverage.lineups)}), " +
                $"{nameof(FootballDB.Tables.LeagueCoverage.predictions)} = VALUES({nameof(FootballDB.Tables.LeagueCoverage.predictions)}), " +
                $"{nameof(FootballDB.Tables.LeagueCoverage.upt_time)} = VALUES({nameof(FootballDB.Tables.LeagueCoverage.upt_time)});");

            return ExecuteQuery(sb.ToString());
        }

        public static bool UpdateTeam(short leagueId, params AppModel.Football.Team[] teams)
        {
            if (teams.Length == 0)
                return false;

            Dev.DebugString("Call DB - FootballFacade.UpdateTeam");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO team");
            sb.Append($" (`{nameof(FootballDB.Tables.Team.id)}`, " +
                $"`{nameof(FootballDB.Tables.Team.name)}`, " +
                $"`{nameof(FootballDB.Tables.Team.country_name)}`, " +
                $"`{nameof(FootballDB.Tables.Team.logo)}`, " +
                $"`{nameof(FootballDB.Tables.Team.upt_time)}`)");
            sb.Append("VALUES");

            for (int i = 0; i < teams.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var team = teams[i];

                Dev.Assert(team.Id != 0);

                sb.Append($"({team.Id}, " +
                    $"\"{team.Name}\", " +
                    $"\"{team.CountryName}\", " +
                    $"\"{team.Logo}\", " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.Team.logo)} = VALUES({nameof(FootballDB.Tables.Team.logo)}), " +
                $"{nameof(FootballDB.Tables.Team.upt_time)} = VALUES({nameof(FootballDB.Tables.Team.upt_time)});");

            return ExecuteQuery(sb.ToString());
        }

        public static bool UpdateStanding(short leagueId, string countryName, params AppModel.Football.Standing[] standings)
        {
            if (standings.Length == 0)
                return false;

            Dev.DebugString("Call DB - FootballFacade.UpdateStanding");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO standing");
            sb.Append($" (`{nameof(FootballDB.Tables.Standing.league_id)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.team_id)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.rank)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.group)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.forme)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.points)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.description)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.played)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.win)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.draw)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.lose)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.goals_for)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.goals_against)}`, " +
                $"`{nameof(FootballDB.Tables.Standing.upt_time)}`)");
            sb.Append("VALUES");

            int errorTeamCnt = 0;
            for (int i = 0; i < standings.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var standing = standings[i];

                // TeamId 컨버트 가능한지..
                if (standing.TeamId == 0)
                {
                    ResourceModel.Football.UndefinedTeam.TryConvertTeamId(countryName, leagueId, standing.TeamName, out short convertedteamId, out string convertedTeamName);
                    standing.TeamId = convertedteamId;
                    standing.TeamName = convertedTeamName;
                }

                if (!Singleton.Get<FootballLogic.CheckValidation>().IsValidTeam((short)standing.TeamId, standing.TeamName, leagueId, countryName, true))
                {
                    errorTeamCnt++;
                }

                sb.Append($"({leagueId}, " +
                    $"{standing.TeamId}, " +
                    $"{standing.Rank}, " +
                    $"\"{standing.Group}\", " +
                    $"\"{standing.Forme}\", " +
                    $"{standing.Points}, " +
                    $"\"{standing.Description}\", " +
                    $"{standing.AllPlayedInfo.Played}, " +
                    $"{standing.AllPlayedInfo.Win}, " +
                    $"{standing.AllPlayedInfo.Draw}, " +
                    $"{standing.AllPlayedInfo.Lose}, " +
                    $"{standing.AllPlayedInfo.GoalsFor}, " +
                    $"{standing.AllPlayedInfo.GoalsAgainst}, " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE `{nameof(FootballDB.Tables.Standing.rank)}` = VALUES(`{nameof(FootballDB.Tables.Standing.rank)}`), " +
                $"`{nameof(FootballDB.Tables.Standing.group)}` = VALUES(`{nameof(FootballDB.Tables.Standing.group)}`), " +
                $"{nameof(FootballDB.Tables.Standing.forme)} = VALUES({nameof(FootballDB.Tables.Standing.forme)}), " +
                $"{nameof(FootballDB.Tables.Standing.points)} = VALUES({nameof(FootballDB.Tables.Standing.points)}), " +
                $"{nameof(FootballDB.Tables.Standing.description)} = VALUES({nameof(FootballDB.Tables.Standing.description)}), " +
                $"{nameof(FootballDB.Tables.Standing.played)} = VALUES({nameof(FootballDB.Tables.Standing.played)}), " +
                $"{nameof(FootballDB.Tables.Standing.win)} = VALUES({nameof(FootballDB.Tables.Standing.win)}), " +
                $"{nameof(FootballDB.Tables.Standing.draw)} = VALUES({nameof(FootballDB.Tables.Standing.draw)}), " +
                $"{nameof(FootballDB.Tables.Standing.lose)} = VALUES({nameof(FootballDB.Tables.Standing.lose)}), " +
                $"{nameof(FootballDB.Tables.Standing.goals_for)} = VALUES({nameof(FootballDB.Tables.Standing.goals_for)}), " +
                $"{nameof(FootballDB.Tables.Standing.goals_against)} = VALUES({nameof(FootballDB.Tables.Standing.goals_against)}), " +
                $"{nameof(FootballDB.Tables.Standing.upt_time)} = VALUES({nameof(FootballDB.Tables.Standing.upt_time)});");

            if (errorTeamCnt > 0)
                return false;

            return ExecuteQuery(sb.ToString());
        }

        public static bool UpdateFixture(bool is_completed, bool is_predicted, params AppModel.Football.Fixture[] fixtures)
        {
            if (fixtures.Length == 0)
                return false;

            Dev.DebugString("Call DB - FootballFacade.UpdateFixture");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO fixture");
            sb.Append($" (`{nameof(FootballDB.Tables.Fixture.id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.league_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.home_team_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.away_team_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.match_time)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.round)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.status)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.home_score)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.away_score)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.is_completed)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.is_predicted)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.upt_time)}`)");
            sb.Append("VALUES");

            for (int i = 0; i < fixtures.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var fixture = fixtures[i];

                Dev.Assert(fixture.HomeTeam.TeamId != 0);

                sb.Append($"({fixture.FixtureId}, " +
                    $"{fixture.LeagueId}, " +
                    $"{fixture.HomeTeam.TeamId}, " +
                    $"{fixture.AwayTeam.TeamId}, " +
                    $"\"{fixture.MatchTime.ToString("yyyyMMddTHHmmss")}\", " +
                    $"\"{fixture.Round}\", " +
                    $"\"{fixture.Status}\", " +
                    $"{fixture.GoalsHomeTeam}, " +
                    $"{fixture.GoalsAwayTeam}, " +
                    $"{is_completed}, " +
                    $"{is_predicted}, " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.Fixture.status)} = VALUES({nameof(FootballDB.Tables.Fixture.status)}), " +
                $"{nameof(FootballDB.Tables.Fixture.home_score)} = VALUES({nameof(FootballDB.Tables.Fixture.home_score)}), " +
                $"{nameof(FootballDB.Tables.Fixture.away_score)} = VALUES({nameof(FootballDB.Tables.Fixture.away_score)}), " +
                $"{nameof(FootballDB.Tables.Fixture.upt_time)} = VALUES({nameof(FootballDB.Tables.Fixture.upt_time)});");

            return ExecuteQuery(sb.ToString());
        }

        public static bool UpdateFixture(params FootballDB.Tables.Fixture[] fixtures)
        {
            if (fixtures.Length == 0)
                return false;

            Dev.DebugString("Call DB - FootballFacade.UpdateFixture");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO fixture");
            sb.Append($" (`{nameof(FootballDB.Tables.Fixture.id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.league_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.home_team_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.away_team_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.match_time)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.round)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.status)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.home_score)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.away_score)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.is_completed)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.is_predicted)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.upt_time)}`)");
            sb.Append("VALUES");

            for (int i = 0; i < fixtures.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var fixture = fixtures[i];
                sb.Append($"({fixture.id}, " +
                    $"{fixture.league_id}, " +
                    $"{fixture.home_team_id}, " +
                    $"{fixture.away_team_id}, " +
                    $"\"{fixture.match_time.ToString("yyyyMMddTHHmmss")}\", " +
                    $"\"{fixture.round}\", " +
                    $"\"{fixture.status}\", " +
                    $"{fixture.home_score}, " +
                    $"{fixture.away_score}, " +
                    $"{fixture.is_completed}, " +
                    $"{fixture.is_predicted}, " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.Fixture.status)} = VALUES({nameof(FootballDB.Tables.Fixture.status)}), " +
                $"{nameof(FootballDB.Tables.Fixture.home_score)} = VALUES({nameof(FootballDB.Tables.Fixture.home_score)}), " +
                $"{nameof(FootballDB.Tables.Fixture.away_score)} = VALUES({nameof(FootballDB.Tables.Fixture.away_score)}), " +
                $"{nameof(FootballDB.Tables.Fixture.is_completed)} = VALUES({nameof(FootballDB.Tables.Fixture.is_completed)}), " +
                $"{nameof(FootballDB.Tables.Fixture.is_predicted)} = VALUES({nameof(FootballDB.Tables.Fixture.is_predicted)}), " +
                $"{nameof(FootballDB.Tables.Fixture.upt_time)} = VALUES({nameof(FootballDB.Tables.Fixture.upt_time)});");

            return ExecuteQuery(sb.ToString());
        }

        public static bool UpdateFixtureStatistics(params FootballDB.Tables.FixtureStatistics[] fixtureStatistics)
        {
            if (fixtureStatistics.Length == 0)
                return false;

            Dev.DebugString("Call DB - FootballFacade.UpdateFixtureStatistics");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO fixture_statistics");
            sb.Append($" (`{nameof(FootballDB.Tables.FixtureStatistics.fixture_id)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.team_id)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.shots_total)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.shots_on_goal)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.shots_off_goal)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.shots_blocked)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.shots_inside_box)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.shots_outside_box)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.goalkeeper_saves)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.fouls)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.corner_kicks)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.offsides)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.yellow_cards)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.red_cards)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.ball_possessions)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.passes_total)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.passes_accurate)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistics.upt_time)}`)");
            sb.Append("VALUES");

            for (int i = 0; i < fixtureStatistics.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var fixturestatistic = fixtureStatistics[i];
                sb.Append($"({fixturestatistic.fixture_id}, " +
                    $"{fixturestatistic.team_id}, " +
                    $"{fixturestatistic.shots_total}, " +
                    $"{fixturestatistic.shots_on_goal}, " +
                    $"{fixturestatistic.shots_off_goal}, " +
                    $"{fixturestatistic.shots_blocked}, " +
                    $"{fixturestatistic.shots_inside_box}, " +
                    $"{fixturestatistic.shots_outside_box}, " +
                    $"{fixturestatistic.goalkeeper_saves}, " +
                    $"{fixturestatistic.fouls}, " +
                    $"{fixturestatistic.corner_kicks}, " +
                    $"{fixturestatistic.offsides}, " +
                    $"{fixturestatistic.yellow_cards}, " +
                    $"{fixturestatistic.red_cards}, " +
                    $"{fixturestatistic.ball_possessions}, " +
                    $"{fixturestatistic.passes_total}, " +
                    $"{fixturestatistic.passes_accurate}, " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.FixtureStatistics.upt_time)} = VALUES({nameof(FootballDB.Tables.FixtureStatistics.upt_time)});");

            return ExecuteQuery(sb.ToString());
        }

        public static bool UpdateOdds(int fixtureId, params AppModel.Football.Odds.BookmakerInfo[] bookMakerInfos)
        {
            if (bookMakerInfos.Length == 0)
                return false;

            Dev.DebugString("Call DB - FootballFacade.UpdateOdds");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO odds");
            sb.Append($" (`{nameof(FootballDB.Tables.Odds.fixture_id)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.bookmaker_type)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.label_type)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.subtitle_1)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.odds_1)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.subtitle_2)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.odds_2)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.subtitle_3)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.odds_3)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.upt_time)}`) ");
            sb.Append("VALUES");

            for (int k = 0; k < bookMakerInfos.Length; k++)
            {
                if (k != 0)
                    sb.Append(", ");

                var bookMaker = bookMakerInfos[k];
                for (int i = 0; i < bookMaker.BetInfos.Length && bookMaker.BetInfos[i].BetValues.Length > 0; i++)
                {
                    if (i != 0)
                    {
                        sb.Append(", ");
                    }

                    var betInfo = bookMaker.BetInfos[i];
                    //if (betInfo.LabelType == RapidAPI.Models.Football.Enums.OddsLabelType.Goals_Over_Under)
                    //{
                    //    for (int j = 0; j < betInfo.BetValues.Length; j++)
                    //    {
                    //        if (j != 0)
                    //            sb.Append(", ");

                    //        sb.Append($"({fixtureID}, " +
                    //            $"{(int)bookMaker.BookmakerType}, " +
                    //            $"{(int)betInfo.LabelType}, " +
                    //            $"\"{betInfo.BetValues[j].Name}\", " +
                    //            $"{betInfo.BetValues[j].Odds}, " +
                    //            $"\"\", 0, \"\", 0, " +
                    //            $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
                    //    }
                    //}
                    //else
                    //{
                    //    sb.Append($"({fixtureID}, " +
                    //        $"{(int)bookMaker.BookmakerType}, " +
                    //        $"{(int)betInfo.LabelType}, ");

                    //    for (int j = 0; j < 3; j++)
                    //    {
                    //        if (betInfo.BetValues.Length > j)
                    //        {
                    //            sb.Append($"\"{betInfo.BetValues[j].Name}\", " +
                    //            $"{betInfo.BetValues[j].Odds}, ");
                    //        }
                    //        else
                    //        {
                    //            sb.Append($"\"\", 0, ");
                    //        }
                    //    }

                    //    sb.Append($"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
                    //}

                    sb.Append($"({fixtureId}, " +
                           $"\"{bookMaker.BookmakerType}\", " +
                           $"\"{betInfo.LabelType}\", ");

                    for (int j = 0; j < 3; j++)
                    {
                        if (betInfo.BetValues.Length > j)
                        {
                            sb.Append($"\"{betInfo.BetValues[j].Name}\", " +
                            $"{betInfo.BetValues[j].Odds}, ");
                        }
                        else
                        {
                            sb.Append($"\"\", 0, ");
                        }
                    }

                    sb.Append($"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
                }
            }

            return ExecuteQuery(sb.ToString());
        }

        #endregion Update

        #region Select

        public static IEnumerable<FootballDB.Tables.League> SelectLeagues(string where = null, string groupBy = null, string orderBy = null)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectLeagues");

            return SelectQuery(new FootballDB.Procedures.P_SELECT_QUERY<FootballDB.Tables.League>.Input
            {
                Query = "SELECT * FROM league",
                Where = where,
                GroupBy = groupBy,
                OrderBy = orderBy,
            });
        }

        public static IEnumerable<FootballDB.Tables.Team> SelectTeams(string where = null, string groupBy = null, string orderBy = null)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectTeams");

            return SelectQuery(new FootballDB.Procedures.P_SELECT_QUERY<FootballDB.Tables.Team>.Input
            {
                Query = "SELECT * FROM team",
                Where = where,
                GroupBy = groupBy,
                OrderBy = orderBy,
            });
        }

        public static IEnumerable<FootballDB.Tables.Fixture> SelectFixtures(string where = null, string groupBy = null, string orderBy = null)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectFixtures");

            return SelectQuery(new FootballDB.Procedures.P_SELECT_QUERY<FootballDB.Tables.Fixture>.Input
            {
                Query = "SELECT * FROM fixture",
                Where = where,
                GroupBy = groupBy,
                OrderBy = orderBy,
            });
        }

        public static IEnumerable<FootballDB.Tables.LeagueCoverage> SelectCoverages(string where = null, string groupBy = null, string orderBy = null)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectCoverage");

            return SelectQuery(new FootballDB.Procedures.P_SELECT_QUERY<FootballDB.Tables.LeagueCoverage>.Input
            {
                Query = "SELECT * FROM league_coverage",
                Where = where,
                GroupBy = groupBy,
                OrderBy = orderBy,
            });
        }

        public static IEnumerable<FootballDB.Tables.Standing> SelectStandings(string where = null, string groupBy = null, string orderBy = null)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectStandings");

            return SelectQuery(new FootballDB.Procedures.P_SELECT_QUERY<FootballDB.Tables.Standing>.Input
            {
                Query = "SELECT * FROM standing",
                Where = where,
                GroupBy = groupBy,
                OrderBy = orderBy,
            });
        }

        public static IEnumerable<FootballDB.Tables.Odds> SelectOdds(string where = null, string groupBy = null, string orderBy = null)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectOdds");

            return SelectQuery(new FootballDB.Procedures.P_SELECT_QUERY<FootballDB.Tables.Odds>.Input
            {
                Query = "SELECT * FROM odds",
                Where = where,
                GroupBy = groupBy,
                OrderBy = orderBy,
            });
        }

        #endregion Select

        #region Delete

        public static bool DeleteFixtures(string where)
        {
            Dev.DebugString("Call DB - FootballFacade.DeleteFixtures");

            return ExecuteQuery($"DELETE FROM fixture WHERE {where}");
        }

        #endregion Delete

        public static bool ExecuteQuery(string query)
        {
            using (var P_EXECUTE_QUERY = new FootballDB.Procedures.P_EXECUTE_QUERY())
            {
                P_EXECUTE_QUERY.SetInput(query);
                P_EXECUTE_QUERY.OnQuery();

                Dev.Assert(P_EXECUTE_QUERY.EntityStatus == null);
            }

            return true;
        }

        public static IEnumerable<T> SelectQuery<T>(FootballDB.Procedures.P_SELECT_QUERY<T>.Input input)
        {
            IEnumerable<T> result;
            using (var P_SELECT_QUERY = new FootballDB.Procedures.P_SELECT_QUERY<T>())
            {
                P_SELECT_QUERY.SetInput(input);
                result = P_SELECT_QUERY.OnQuery();

                Dev.Assert(P_SELECT_QUERY.EntityStatus == null);
            }

            return result;
        }
    }
}