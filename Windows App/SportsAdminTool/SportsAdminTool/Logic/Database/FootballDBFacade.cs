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

        public static int UpdateCountry(params AppModel.Football.Country[] countries)
        {
            if (countries.Length == 0)
                return 0;

            Dev.DebugString("Call DB - FootballFacade.UpdateCountry");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO country");
            sb.Append($" (`{nameof(FootballDB.Tables.Country.code)}`, " +
                $"`{nameof(FootballDB.Tables.Country.name)}`, " +
                $"`{nameof(FootballDB.Tables.Country.flag)}`, " +
                $"`{nameof(FootballDB.Tables.Country.upt_time)}`)");
            sb.Append("VALUES");

            for (int i = 0; i < countries.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var country = countries[i];
                sb.Append($"(\"{country.Code}\", " +
                    $"\"{country.Name}\", " +
                    $"\"{country.Flag}\", " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.Country.flag)} = VALUES({nameof(FootballDB.Tables.Country.flag)}), " +
                $"{nameof(FootballDB.Tables.Country.upt_time)} = VALUES({nameof(FootballDB.Tables.Country.upt_time)});");

            return ExecuteSQL(sb.ToString());
        }

        public static int UpdateLeague(params AppModel.Football.League[] leagues)
        {
            if (leagues.Length == 0)
                return 0;

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
                $"`{nameof(FootballDB.Tables.League.upt_time)}`)");
            sb.Append("VALUES");

            for (int i = 0; i < leagues.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var league = leagues[i];
                sb.Append($"({league.LeagueID}, " +
                    $"\"{league.Name}\", " +
                    $"\"{league.Type}\", " +
                    $"\"{league.Country}\", " +
                    $"\"{league.IsCurrent}\", " +
                    $"\"{league.SeasonStart.ToString("yyyyMMddTHHmmss")}\", " +
                    $"\"{league.SeasonEnd.ToString("yyyyMMddTHHmmss")}\", " +
                    $"\"{league.Logo}\", " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.League.logo)} = VALUES({nameof(FootballDB.Tables.League.logo)}), " +
                $"{nameof(FootballDB.Tables.League.is_current)} = VALUES({nameof(FootballDB.Tables.League.is_current)}), " +
                $"{nameof(FootballDB.Tables.League.upt_time)} = VALUES({nameof(FootballDB.Tables.League.upt_time)});");

            return ExecuteSQL(sb.ToString());
        }

        public static int UpdateCoverage(params AppModel.Football.League[] leagues)
        {
            if (leagues.Length == 0)
                return 0;

            Dev.DebugString("Call DB - FootballFacade.UpdateCoverage");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO coverage");
            sb.Append($" (`{nameof(FootballDB.Tables.Coverage.league_id)}`, " +
                $"`{nameof(FootballDB.Tables.Coverage.standings)}`, " +
                $"`{nameof(FootballDB.Tables.Coverage.odds)}`, " +
                $"`{nameof(FootballDB.Tables.Coverage.fixture_statistics)}`, " +
                $"`{nameof(FootballDB.Tables.Coverage.players)}`, " +
                $"`{nameof(FootballDB.Tables.Coverage.lineups)}`, " +
                $"`{nameof(FootballDB.Tables.Coverage.predictions)}`, " +
                $"`{nameof(FootballDB.Tables.Coverage.upt_time)}`)");
            sb.Append("VALUES");

            for (int i = 0; i < leagues.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                var league = leagues[i];

                sb.Append($"({league.LeagueID}, " +
                    $"{league.Coverage.Standings}, " +
                    $"{league.Coverage.Odds}, " +
                    $"{league.Coverage.FixtureCoverage.Statistics}, " +
                    $"{league.Coverage.Players}, " +
                    $"{league.Coverage.FixtureCoverage.Lineups}, " +
                    $"{league.Coverage.Predictions}, " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.Coverage.standings)} = VALUES({nameof(FootballDB.Tables.Coverage.standings)}), " +
                $"{nameof(FootballDB.Tables.Coverage.odds)} = VALUES({nameof(FootballDB.Tables.Coverage.odds)}), " +
                $"{nameof(FootballDB.Tables.Coverage.fixture_statistics)} = VALUES({nameof(FootballDB.Tables.Coverage.fixture_statistics)}), " +
                $"{nameof(FootballDB.Tables.Coverage.players)} = VALUES({nameof(FootballDB.Tables.Coverage.players)}), " +
                $"{nameof(FootballDB.Tables.Coverage.lineups)} = VALUES({nameof(FootballDB.Tables.Coverage.lineups)}), " +
                $"{nameof(FootballDB.Tables.Coverage.predictions)} = VALUES({nameof(FootballDB.Tables.Coverage.predictions)}), " +
                $"{nameof(FootballDB.Tables.Coverage.upt_time)} = VALUES({nameof(FootballDB.Tables.Coverage.upt_time)});");

            return ExecuteSQL(sb.ToString());
        }

        public static int UpdateTeam(short leagueId, params AppModel.Football.Team[] teams)
        {
            if (teams.Length == 0)
                return 0;

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

                // TeamID 컨버트 가능한지..
                ResourceModel.Football.UndefinedTeam.TryConvertTeamID(team.CountryName, leagueId, team.Name, out short convertedteamID, out string convertedTeamName);
                if (convertedteamID != 0)
                {
                    team.ID = convertedteamID;
                    team.Name = convertedTeamName;
                }

                if (!Singleton.Get<FootballLogic.CheckValidation>().IsValidTeam((short)team.ID, team.Name, leagueId, team.CountryName, false))
                    continue;

                sb.Append($"({team.ID}, " +
                    $"\"{team.Name}\", " +
                    $"\"{team.CountryName}\", " +
                    $"\"{team.Logo}\", " +
                    $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
            }

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.Team.logo)} = VALUES({nameof(FootballDB.Tables.Team.logo)}), " +
                $"{nameof(FootballDB.Tables.Team.upt_time)} = VALUES({nameof(FootballDB.Tables.Team.upt_time)});");

            return ExecuteSQL(sb.ToString());
        }

        public static int UpdateStanding(short leagueID, string countryName, params AppModel.Football.Standing[] standings)
        {
            if (standings.Length == 0)
                return 0;

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

                // TeamID 컨버트 가능한지..
                ResourceModel.Football.UndefinedTeam.TryConvertTeamID(countryName, leagueID, standing.TeamName, out short convertedteamID, out string convertedTeamName);
                if (convertedteamID != 0)
                {
                    standing.TeamID = convertedteamID;
                    standing.TeamName = convertedTeamName;
                }

                if (!Singleton.Get<FootballLogic.CheckValidation>().IsValidTeam((short)standing.TeamID, standing.TeamName, leagueID, countryName, true))
                {
                    errorTeamCnt++;
                }

                sb.Append($"({leagueID}, " +
                    $"{standing.TeamID}, " +
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
                return 0;

            return ExecuteSQL(sb.ToString());
        }

        public static int UpdateFixture(bool is_completed, bool is_predicted, params AppModel.Football.Fixture[] fixtures)
        {
            if (fixtures.Length == 0)
                return 0;

            Dev.DebugString("Call DB - FootballFacade.UpdateFixture");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO fixture");
            sb.Append($" (`{nameof(FootballDB.Tables.Fixture.id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.league_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.home_team_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.away_team_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.event_date)}`, " +
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

                // TeamID 컨버트 가능한지..
                ResourceModel.Football.UndefinedTeam.TryConvertTeamID(fixture.League.Country, (short)fixture.LeagueID, fixture.HomeTeam.TeamName, out short convertedteamID, out string convertedTeamName);
                if (convertedteamID != 0)
                {
                    fixture.HomeTeam.TeamID = convertedteamID;
                    fixture.HomeTeam.TeamName = convertedTeamName;
                }
                if (!Singleton.Get<FootballLogic.CheckValidation>().IsValidTeam((short)fixture.HomeTeam.TeamID, fixture.HomeTeam.TeamName, (short)fixture.LeagueID, fixture.League.Country, true))
                    continue;

                ResourceModel.Football.UndefinedTeam.TryConvertTeamID(fixture.League.Country, (short)fixture.LeagueID, fixture.AwayTeam.TeamName, out convertedteamID, out convertedTeamName);
                if (convertedteamID != 0)
                {
                    fixture.AwayTeam.TeamID = convertedteamID;
                    fixture.AwayTeam.TeamName = convertedTeamName;
                }
                if (!Singleton.Get<FootballLogic.CheckValidation>().IsValidTeam((short)fixture.AwayTeam.TeamID, fixture.AwayTeam.TeamName, (short)fixture.LeagueID, fixture.League.Country, true))
                    continue;

                sb.Append($"({fixture.FixtureID}, " +
                    $"{fixture.LeagueID}, " +
                    $"{fixture.HomeTeam.TeamID}, " +
                    $"{fixture.AwayTeam.TeamID}, " +
                    $"\"{fixture.EventDate.ToString("yyyyMMddTHHmmss")}\", " +
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

            return ExecuteSQL(sb.ToString());
        }

        public static int UpdateFixture(params FootballDB.Tables.Fixture[] fixtures)
        {
            if (fixtures.Length == 0)
                return 0;
            Dev.DebugString("Call DB - FootballFacade.UpdateFixture");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO fixture");
            sb.Append($" (`{nameof(FootballDB.Tables.Fixture.id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.league_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.home_team_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.away_team_id)}`, " +
                $"`{nameof(FootballDB.Tables.Fixture.event_date)}`, " +
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
                    $"\"{fixture.event_date.ToString("yyyyMMddTHHmmss")}\", " +
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

            return ExecuteSQL(sb.ToString());
        }

        public static int UpdateFixtureStatistics(params FootballDB.Tables.FixtureStatistic[] fixtureStatistics)
        {
            if (fixtureStatistics.Length == 0)
                return 0;

            Dev.DebugString("Call DB - FootballFacade.UpdateFixtureStatistics");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO fixture_statistic");
            sb.Append($" (`{nameof(FootballDB.Tables.FixtureStatistic.fixture_id)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.team_id)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.shots_total)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.shots_on_goal)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.shots_off_goal)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.shots_blocked)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.shots_inside_box)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.shots_outside_box)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.goalkeeper_saves)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.fouls)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.corner_kicks)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.offsides)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.yellow_cards)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.red_cards)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.ball_possessions)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.passes_total)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.passes_accurate)}`, " +
                $"`{nameof(FootballDB.Tables.FixtureStatistic.upt_time)}`)");
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

            sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Tables.FixtureStatistic.upt_time)} = VALUES({nameof(FootballDB.Tables.FixtureStatistic.upt_time)});");

            return ExecuteSQL(sb.ToString());
        }

        public static int UpdateOdds(int fixtureID, params AppModel.Football.Odds.BookmakerInfo[] bookMakerInfos)
        {
            if (bookMakerInfos.Length == 0)
                return 0;

            Dev.DebugString("Call DB - FootballFacade.UpdateOdds");

            DateTime upt_time = DateTime.Now.ToUniversalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO odds");
            sb.Append($" (`{nameof(FootballDB.Tables.Odds.fixture_id)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.bookmaker_id)}`, " +
                $"`{nameof(FootballDB.Tables.Odds.label_id)}`, " +
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
                    if (betInfo.LabelType == RapidAPI.Models.Football.Enums.OddsLabelType.Goals_Over_Under)
                    {
                        for (int j = 0; j < betInfo.BetValues.Length; j++)
                        {
                            if (j != 0)
                                sb.Append(", ");

                            sb.Append($"({fixtureID}, " +
                                $"{(int)bookMaker.BookmakerType}, " +
                                $"{(int)betInfo.LabelType}, " +
                                $"\"{betInfo.BetValues[j].Name}\", " +
                                $"{betInfo.BetValues[j].Odds}, " +
                                $"\"\", 0, \"\", 0, " +
                                $"\"{upt_time.ToString("yyyyMMddTHHmmss")}\")");
                        }
                    }
                    else
                    {
                        sb.Append($"({fixtureID}, " +
                            $"{(int)bookMaker.BookmakerType}, " +
                            $"{(int)betInfo.LabelType}, ");

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
            }

            return ExecuteSQL(sb.ToString());
        }

        #endregion Update

        #region Select

        public static IEnumerable<FootballDB.Tables.League> SelectOnGogingCoverageLeagues()
        {
            Dev.DebugString("Call DB - FootballFacade.SelectOnGogingCoverageLeagues");

            using (var P_SELECT_LEAGUES = new FootballDB.Procedures.P_SELECT_LEAGUES())
            {
                var input = new FootballDB.Procedures.P_SELECT_LEAGUES.Input()
                {
                    Where = "is_current = 1",
                };

                P_SELECT_LEAGUES.SetInput(input);
                var selectedLeauges = P_SELECT_LEAGUES.OnQuery();

                foreach (var league in selectedLeauges)
                {
                    if (Singleton.Get<FootballLogic.CheckValidation>().IsValidLeague(league.id, league.name, league.country_name, false))
                        yield return league;
                }
            }
        }

        public static IEnumerable<FootballDB.Tables.League> SelectLeagues(FootballDB.Procedures.P_SELECT_LEAGUES.Input option)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectLeagues");

            using (var P_SELECT_LEAGUES = new FootballDB.Procedures.P_SELECT_LEAGUES())
            {
                P_SELECT_LEAGUES.SetInput(option);
                return P_SELECT_LEAGUES.OnQuery();
            }
        }

        public static IEnumerable<FootballDB.Tables.Team> SelectTeams(FootballDB.Procedures.P_SELECT_TEAMS.Input option)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectTeams");

            using (var P_SELECT_TEAMS = new FootballDB.Procedures.P_SELECT_TEAMS())
            {
                P_SELECT_TEAMS.SetInput(option);
                return P_SELECT_TEAMS.OnQuery();
            }
        }

        public static IEnumerable<FootballDB.Tables.Fixture> SelectFixtures(FootballDB.Procedures.P_SELECT_FIXTURES.Input option)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectFixtures");

            using (var P_SELECT_FIXTURES = new FootballDB.Procedures.P_SELECT_FIXTURES())
            {
                P_SELECT_FIXTURES.SetInput(option);
                return P_SELECT_FIXTURES.OnQuery();
            }
        }

        public static FootballDB.Tables.Coverage SelectCoverage(short leagueID)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectCoverage");

            using (var P_SELECT_COVERAGE = new FootballDB.Procedures.P_SELECT_COVERAGE())
            {
                P_SELECT_COVERAGE.SetInput(leagueID);
                return P_SELECT_COVERAGE.OnQuery();
            }
        }

        public static IEnumerable<FootballDB.Tables.Standing> SelectStandings(short leagueID)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectStandings");

            using (var P_SELECT_STANDINGS = new FootballDB.Procedures.P_SELECT_STANDINGS())
            {
                P_SELECT_STANDINGS.SetInput(leagueID);
                return P_SELECT_STANDINGS.OnQuery();
            }
        }

        public static IEnumerable<FootballDB.Tables.Odds> SelectOdds(int fixtureID)
        {
            Dev.DebugString("Call DB - FootballFacade.SelectOdds");

            using (var P_SELECT_ODDS = new FootballDB.Procedures.P_SELECT_ODDS())
            {
                P_SELECT_ODDS.SetInput(fixtureID);
                return P_SELECT_ODDS.OnQuery();
            }
        }

        #endregion Select

        #region Delete

        public static int DeleteFixtures(int fixtureID)
        {
            if (fixtureID == 0)
                return 0;

            Dev.DebugString("Call DB - FootballFacade.DeleteFixtures");

            return ExecuteSQL($"DELETE FROM fixture WHERE id = {fixtureID}");
        }

        #endregion Delete

        #region Check Validation

        public static bool IsAlreadyUpdatedStandings(short leagueID)
        {
            var db_standings = SelectStandings(leagueID);

            var firstData = db_standings.FirstOrDefault();

            return firstData != null && firstData.upt_time.Date == DateTime.UtcNow.Date;
        }

        public static bool IsAlreadyUpdatedOdds(int fixtureID)
        {
            var db_odds = SelectOdds(fixtureID);

            var firstData = db_odds.FirstOrDefault();

            return firstData != null;
        }

        public static bool IsExistFixture(int fixtureID)
        {
            var option = new FootballDB.Procedures.P_SELECT_FIXTURES.Input()
            {
                Where = $"id = {fixtureID}",
            };
            var db_standings = SelectFixtures(option);

            var firstData = db_standings.FirstOrDefault();

            return firstData != null;
        }

        #endregion Check Validation

        public static int ExecuteSQL(string sql)
        {
            int result = 0;

            using (var P_EXECUTE_QUERY = new FootballDB.Procedures.P_EXECUTE_QUERY())
            {
                P_EXECUTE_QUERY.SetInput(sql);
                result = P_EXECUTE_QUERY.OnQuery();

                if (P_EXECUTE_QUERY.EntityStatus != null)
                {
                    Dev.DebugString($"Error SQL: {sql}");
                }

                Dev.DebugString($"Affected row cout: {result}");
            }

            return result;
        }
    }
}