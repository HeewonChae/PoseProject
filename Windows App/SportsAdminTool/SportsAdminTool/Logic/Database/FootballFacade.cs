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
	public static class FootballFacade
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
			sb.Append($" (`{nameof(FootballDB.Table.Country.code)}`, " +
				$"`{nameof(FootballDB.Table.Country.name)}`, " +
				$"`{nameof(FootballDB.Table.Country.flag)}`, " +
				$"`{nameof(FootballDB.Table.Country.upt_time)}`)");
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

			sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Table.Country.flag)} = VALUES({nameof(FootballDB.Table.Country.flag)}), " +
				$"{nameof(FootballDB.Table.Country.upt_time)} = VALUES({nameof(FootballDB.Table.Country.upt_time)});");

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
			sb.Append($" (`{nameof(FootballDB.Table.League.id)}`, " +
				$"`{nameof(FootballDB.Table.League.name)}`, " +
				$"`{nameof(FootballDB.Table.League.type)}`, " +
				$"`{nameof(FootballDB.Table.League.country_name)}`, " +
				$"`{nameof(FootballDB.Table.League.is_current)}`, " +
				$"`{nameof(FootballDB.Table.League.season_start)}`, " +
				$"`{nameof(FootballDB.Table.League.season_end)}`, " +
				$"`{nameof(FootballDB.Table.League.logo)}`, " +
				$"`{nameof(FootballDB.Table.League.upt_time)}`)");
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

			sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Table.League.logo)} = VALUES({nameof(FootballDB.Table.League.logo)}), " +
				$"{nameof(FootballDB.Table.League.is_current)} = VALUES({nameof(FootballDB.Table.League.is_current)}), " +
				$"{nameof(FootballDB.Table.League.upt_time)} = VALUES({nameof(FootballDB.Table.League.upt_time)});");

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
			sb.Append($" (`{nameof(FootballDB.Table.Coverage.league_id)}`, " +
				$"`{nameof(FootballDB.Table.Coverage.standings)}`, " +
				$"`{nameof(FootballDB.Table.Coverage.odds)}`, " +
				$"`{nameof(FootballDB.Table.Coverage.fixture_statistics)}`, " +
				$"`{nameof(FootballDB.Table.Coverage.players)}`, " +
				$"`{nameof(FootballDB.Table.Coverage.lineups)}`, " +
				$"`{nameof(FootballDB.Table.Coverage.predictions)}`, " +
				$"`{nameof(FootballDB.Table.Coverage.upt_time)}`)");
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

			sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Table.Coverage.standings)} = VALUES({nameof(FootballDB.Table.Coverage.standings)}), " +
				$"{nameof(FootballDB.Table.Coverage.odds)} = VALUES({nameof(FootballDB.Table.Coverage.odds)}), " +
				$"{nameof(FootballDB.Table.Coverage.fixture_statistics)} = VALUES({nameof(FootballDB.Table.Coverage.fixture_statistics)}), " +
				$"{nameof(FootballDB.Table.Coverage.players)} = VALUES({nameof(FootballDB.Table.Coverage.players)}), " +
				$"{nameof(FootballDB.Table.Coverage.lineups)} = VALUES({nameof(FootballDB.Table.Coverage.lineups)}), " +
				$"{nameof(FootballDB.Table.Coverage.predictions)} = VALUES({nameof(FootballDB.Table.Coverage.predictions)}), " +
				$"{nameof(FootballDB.Table.Coverage.upt_time)} = VALUES({nameof(FootballDB.Table.Coverage.upt_time)});");

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
			sb.Append($" (`{nameof(FootballDB.Table.Team.id)}`, " +
				$"`{nameof(FootballDB.Table.Team.name)}`, " +
				$"`{nameof(FootballDB.Table.Team.country_name)}`, " +
				$"`{nameof(FootballDB.Table.Team.logo)}`, " +
				$"`{nameof(FootballDB.Table.Team.upt_time)}`)");
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

			sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Table.Team.logo)} = VALUES({nameof(FootballDB.Table.Team.logo)}), " +
				$"{nameof(FootballDB.Table.Team.upt_time)} = VALUES({nameof(FootballDB.Table.Team.upt_time)});");

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
			sb.Append($" (`{nameof(FootballDB.Table.Standing.league_id)}`, " +
				$"`{nameof(FootballDB.Table.Standing.team_id)}`, " +
				$"`{nameof(FootballDB.Table.Standing.rank)}`, " +
				$"`{nameof(FootballDB.Table.Standing.group)}`, " +
				$"`{nameof(FootballDB.Table.Standing.forme)}`, " +
				$"`{nameof(FootballDB.Table.Standing.points)}`, " +
				$"`{nameof(FootballDB.Table.Standing.description)}`, " +
				$"`{nameof(FootballDB.Table.Standing.played)}`, " +
				$"`{nameof(FootballDB.Table.Standing.win)}`, " +
				$"`{nameof(FootballDB.Table.Standing.draw)}`, " +
				$"`{nameof(FootballDB.Table.Standing.lose)}`, " +
				$"`{nameof(FootballDB.Table.Standing.goals_for)}`, " +
				$"`{nameof(FootballDB.Table.Standing.goals_against)}`, " +
				$"`{nameof(FootballDB.Table.Standing.upt_time)}`)");
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

			sb.Append($"ON DUPLICATE KEY UPDATE `{nameof(FootballDB.Table.Standing.rank)}` = VALUES(`{nameof(FootballDB.Table.Standing.rank)}`), " +
				$"`{nameof(FootballDB.Table.Standing.group)}` = VALUES(`{nameof(FootballDB.Table.Standing.group)}`), " +
				$"{nameof(FootballDB.Table.Standing.forme)} = VALUES({nameof(FootballDB.Table.Standing.forme)}), " +
				$"{nameof(FootballDB.Table.Standing.points)} = VALUES({nameof(FootballDB.Table.Standing.points)}), " +
				$"{nameof(FootballDB.Table.Standing.description)} = VALUES({nameof(FootballDB.Table.Standing.description)}), " +
				$"{nameof(FootballDB.Table.Standing.played)} = VALUES({nameof(FootballDB.Table.Standing.played)}), " +
				$"{nameof(FootballDB.Table.Standing.win)} = VALUES({nameof(FootballDB.Table.Standing.win)}), " +
				$"{nameof(FootballDB.Table.Standing.draw)} = VALUES({nameof(FootballDB.Table.Standing.draw)}), " +
				$"{nameof(FootballDB.Table.Standing.lose)} = VALUES({nameof(FootballDB.Table.Standing.lose)}), " +
				$"{nameof(FootballDB.Table.Standing.goals_for)} = VALUES({nameof(FootballDB.Table.Standing.goals_for)}), " +
				$"{nameof(FootballDB.Table.Standing.goals_against)} = VALUES({nameof(FootballDB.Table.Standing.goals_against)}), " +
				$"{nameof(FootballDB.Table.Standing.upt_time)} = VALUES({nameof(FootballDB.Table.Standing.upt_time)});");

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
			sb.Append($" (`{nameof(FootballDB.Table.Fixture.id)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.league_id)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.home_team_id)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.away_team_id)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.event_date)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.round)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.status)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.home_score)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.away_score)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.is_completed)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.is_predicted)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.upt_time)}`)");
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

			sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Table.Fixture.status)} = VALUES({nameof(FootballDB.Table.Fixture.status)}), " +
				$"{nameof(FootballDB.Table.Fixture.home_score)} = VALUES({nameof(FootballDB.Table.Fixture.home_score)}), " +
				$"{nameof(FootballDB.Table.Fixture.away_score)} = VALUES({nameof(FootballDB.Table.Fixture.away_score)}), " +
				$"{nameof(FootballDB.Table.Fixture.upt_time)} = VALUES({nameof(FootballDB.Table.Fixture.upt_time)});");

			return ExecuteSQL(sb.ToString());
		}

		public static int UpdateFixture(params FootballDB.Table.Fixture[] fixtures)
		{
			if (fixtures.Length == 0)
				return 0;
			Dev.DebugString("Call DB - FootballFacade.UpdateFixture");

			DateTime upt_time = DateTime.Now.ToUniversalTime();

			StringBuilder sb = new StringBuilder();
			sb.Append(" INSERT INTO fixture");
			sb.Append($" (`{nameof(FootballDB.Table.Fixture.id)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.league_id)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.home_team_id)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.away_team_id)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.event_date)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.round)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.status)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.home_score)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.away_score)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.is_completed)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.is_predicted)}`, " +
				$"`{nameof(FootballDB.Table.Fixture.upt_time)}`)");
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

			sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Table.Fixture.status)} = VALUES({nameof(FootballDB.Table.Fixture.status)}), " +
				$"{nameof(FootballDB.Table.Fixture.home_score)} = VALUES({nameof(FootballDB.Table.Fixture.home_score)}), " +
				$"{nameof(FootballDB.Table.Fixture.away_score)} = VALUES({nameof(FootballDB.Table.Fixture.away_score)}), " +
				$"{nameof(FootballDB.Table.Fixture.is_completed)} = VALUES({nameof(FootballDB.Table.Fixture.is_completed)}), " +
				$"{nameof(FootballDB.Table.Fixture.is_predicted)} = VALUES({nameof(FootballDB.Table.Fixture.is_predicted)}), " +
				$"{nameof(FootballDB.Table.Fixture.upt_time)} = VALUES({nameof(FootballDB.Table.Fixture.upt_time)});");

			return ExecuteSQL(sb.ToString());
		}

		public static int UpdateFixtureStatistics(params FootballDB.Table.FixtureStatistic[] fixtureStatistics)
		{
			if (fixtureStatistics.Length == 0)
				return 0;

			Dev.DebugString("Call DB - FootballFacade.UpdateFixtureStatistics");

			DateTime upt_time = DateTime.Now.ToUniversalTime();

			StringBuilder sb = new StringBuilder();
			sb.Append(" INSERT INTO fixture_statistic");
			sb.Append($" (`{nameof(FootballDB.Table.FixtureStatistic.fixture_id)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.team_id)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.shots_total)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.shots_on_goal)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.shots_off_goal)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.shots_blocked)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.shots_inside_box)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.shots_outside_box)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.goalkeeper_saves)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.fouls)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.corner_kicks)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.offsides)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.yellow_cards)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.red_cards)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.ball_possessions)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.passes_total)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.passes_accurate)}`, " +
				$"`{nameof(FootballDB.Table.FixtureStatistic.upt_time)}`)");
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

			sb.Append($"ON DUPLICATE KEY UPDATE {nameof(FootballDB.Table.FixtureStatistic.upt_time)} = VALUES({nameof(FootballDB.Table.FixtureStatistic.upt_time)});");

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
			sb.Append($" (`{nameof(FootballDB.Table.Odds.fixture_id)}`, " +
				$"`{nameof(FootballDB.Table.Odds.bookmaker_id)}`, " +
				$"`{nameof(FootballDB.Table.Odds.label_id)}`, " +
				$"`{nameof(FootballDB.Table.Odds.subtitle_1)}`, " +
				$"`{nameof(FootballDB.Table.Odds.odds_1)}`, " +
				$"`{nameof(FootballDB.Table.Odds.subtitle_2)}`, " +
				$"`{nameof(FootballDB.Table.Odds.odds_2)}`, " +
				$"`{nameof(FootballDB.Table.Odds.subtitle_3)}`, " +
				$"`{nameof(FootballDB.Table.Odds.odds_3)}`, " +
				$"`{nameof(FootballDB.Table.Odds.upt_time)}`) ");
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

		public static IEnumerable<FootballDB.Table.League> SelectOnGogingCoverageLeagues()
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

		public static IEnumerable<FootballDB.Table.League> SelectLeagues(FootballDB.Procedures.P_SELECT_LEAGUES.Input option)
		{
			Dev.DebugString("Call DB - FootballFacade.SelectLeagues");

			using (var P_SELECT_LEAGUES = new FootballDB.Procedures.P_SELECT_LEAGUES())
			{
				P_SELECT_LEAGUES.SetInput(option);
				return P_SELECT_LEAGUES.OnQuery();
			}
		}

		public static IEnumerable<FootballDB.Table.Team> SelectTeams(FootballDB.Procedures.P_SELECT_TEAMS.Input option)
		{
			Dev.DebugString("Call DB - FootballFacade.SelectTeams");

			using (var P_SELECT_TEAMS = new FootballDB.Procedures.P_SELECT_TEAMS())
			{
				P_SELECT_TEAMS.SetInput(option);
				return P_SELECT_TEAMS.OnQuery();
			}
		}

		public static IEnumerable<FootballDB.Table.Fixture> SelectFixtures(FootballDB.Procedures.P_SELECT_FIXTURES.Input option)
		{
			Dev.DebugString("Call DB - FootballFacade.SelectFixtures");

			using (var P_SELECT_FIXTURES = new FootballDB.Procedures.P_SELECT_FIXTURES())
			{
				P_SELECT_FIXTURES.SetInput(option);
				return P_SELECT_FIXTURES.OnQuery();
			}
		}

		public static FootballDB.Table.Coverage SelectCoverage(short leagueID)
		{
			Dev.DebugString("Call DB - FootballFacade.SelectCoverage");

			using (var P_SELECT_COVERAGE = new FootballDB.Procedures.P_SELECT_COVERAGE())
			{
				P_SELECT_COVERAGE.SetInput(leagueID);
				return P_SELECT_COVERAGE.OnQuery();
			}
		}

		public static IEnumerable<FootballDB.Table.Standing> SelectStandings(short leagueID)
		{
			Dev.DebugString("Call DB - FootballFacade.SelectStandings");

			using (var P_SELECT_STANDINGS = new FootballDB.Procedures.P_SELECT_STANDINGS())
			{
				P_SELECT_STANDINGS.SetInput(leagueID);
				return P_SELECT_STANDINGS.OnQuery();
			}
		}

		public static IEnumerable<FootballDB.Table.Odds> SelectOdds(int fixtureID)
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

				Dev.DebugString($"Affected row cout: {result}");
			}

			return result;
		}
	}
}