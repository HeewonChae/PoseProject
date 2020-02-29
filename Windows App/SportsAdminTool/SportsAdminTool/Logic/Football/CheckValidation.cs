using LogicCore.Utility;
using LogicCore.Debug;
using LogicCore.File;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootballDB = Repository.Mysql.FootballDB;
using ResourceModel = SportsAdminTool.Model.Resource;
using AppModel = SportsAdminTool.Model;
using DatabaseLogic = SportsAdminTool.Logic.Database;
using ApiModel = RapidAPI.Models;
using System.Collections.Concurrent;

namespace SportsAdminTool.Logic.Football
{
	public enum InvalidType
	{
		_NONE_,
		Undefined,
		Zero,
		NotExistInDB,
		NULL,
		_MAX_,
	}

	public struct InvalidTeam
	{
		public short TeamsID;
		public string TeamName;
		public short LeagueID;
		public string CountryName;
		public int ReasonType;
	}

	public struct InvalidLeague
	{
		public short LeagueID;
		public string LeagueName;
		public string CountryName;
		public int ReasonType;
	}

	public class CheckValidation : Singleton.INode
	{
		private readonly string rootDir = "C:\\Users\\korma\\Desktop\\FootballError";
		private readonly List<InvalidLeague> _invalidLeauges = new List<InvalidLeague>();
		private readonly List<InvalidTeam> _invalidTeams = new List<InvalidTeam>();

		#region Check validation

		public bool IsValidLeague(short leagueID, string leagueName, string countryName, bool isDB_check)
		{
			if (leagueID == 0)
			{
				AddInvalidLeague(new InvalidLeague()
				{
					LeagueID = leagueID,
					LeagueName = leagueName,
					CountryName = countryName,
					ReasonType = (int)InvalidType.Zero,
				});

				return false;
			}

			var key = ResourceModel.Football.LeagueCoverage.MakeLeagueCoverageKey(leagueName, countryName);

			bool result;
			if (!ResourceModel.Football.LeagueCoverage.Dic_leagueCoverage.ContainsKey(key))
			{
				AddInvalidLeague(new InvalidLeague()
				{
					LeagueID = leagueID,
					LeagueName = leagueName,
					CountryName = countryName,
					ReasonType = (int)InvalidType.Undefined,
				});

				result = false;
			}
			else
			{
				result = ResourceModel.Football.LeagueCoverage.Dic_leagueCoverage[key].IsCoverage;
			}

			bool db_result = true;
			if (isDB_check)
			{
				db_result = DatabaseLogic.FootballFacade.SelectLeagues(
					new FootballDB.Procedures.P_SELECT_LEAGUES.Input()
					{
						Where = $"id = {leagueID}"
					})
					.FirstOrDefault() != null;

				if (!db_result)
				{
					AddInvalidLeague(new InvalidLeague()
					{
						LeagueID = (short)leagueID,
						LeagueName = leagueName,
						CountryName = countryName,
						ReasonType = (int)InvalidType.NotExistInDB,
					});
				}
			}

			return result && db_result;
		}

		public bool IsValidTeam(short teamID, string teamName, short leagueID, string countryName, bool isDB_check)
		{
			// TeamID 컨버트 가능한지..
			ResourceModel.Football.UndefinedTeam.TryConvertTeamID(countryName, leagueID, teamName, out short convertedteamID);
			if (convertedteamID != 0)
				teamID = convertedteamID;

			bool result = teamID != 0;

			if (!result)
			{
				AddInvalidTeam(new InvalidTeam()
				{
					TeamsID = teamID,
					TeamName = teamName,
					LeagueID = leagueID,
					CountryName = countryName,
					ReasonType = (int)InvalidType.Zero,
				});
			}

			if (result && isDB_check)
			{
				result = DatabaseLogic.FootballFacade.SelectTeams(new FootballDB.Procedures.P_SELECT_TEAMS.Input()
				{
					Where = $"id = {teamID}",
				})
					.FirstOrDefault() != null;

				if (!result)
				{
					AddInvalidTeam(new InvalidTeam()
					{
						TeamsID = teamID,
						TeamName = teamName,
						LeagueID = leagueID,
						CountryName = countryName,
						ReasonType = (int)InvalidType.NotExistInDB,
					});
				}
			}

			return result;
		}

		public bool IsValidStandings(IList<AppModel.Football.Standing> standings,
			short leagueID, string leagueName, string countryName, bool checkExistLeagueInDB)
		{
			if (standings.Count == 0)
			{
				AddInvalidLeague(new InvalidLeague()
				{
					LeagueID = leagueID,
					LeagueName = leagueName,
					CountryName = countryName,
					ReasonType = (int)InvalidType.NULL,
				});
			}

			if (checkExistLeagueInDB)
			{
				bool result = DatabaseLogic.FootballFacade.SelectLeagues(
						new FootballDB.Procedures.P_SELECT_LEAGUES.Input()
						{
							Where = $"id = {leagueID}"
						})
						.FirstOrDefault() != null;

				if (!result)
				{
					AddInvalidLeague(new InvalidLeague()
					{
						LeagueID = leagueID,
						LeagueName = leagueName,
						CountryName = countryName,
						ReasonType = (int)InvalidType.NotExistInDB,
					});
				}

				return result;
			}

			//if (standings.Count > 1 && standings[0][0].Group.Equals(standings[1][0].Group))
			//{
			//	AddInvalidLeague(new InvalidLeague()
			//	{
			//		LeagueID = (short)leagueID,
			//		LeagueName = leagueName,
			//		CountryName = countryName,
			//		Reason = "Standings data is invalidation",
			//	});

			//	return false;
			//}

			return true;
		}

		public bool IsValidFixtureStatus(ApiModel.Football.Enums.FixtureStatusType status)
		{
			if (status == ApiModel.Football.Enums.FixtureStatusType.NS // 경기전
				|| status == ApiModel.Football.Enums.FixtureStatusType.FH // 전반
				|| status == ApiModel.Football.Enums.FixtureStatusType.HT // 하프타임
				|| status == ApiModel.Football.Enums.FixtureStatusType.SH // 후반
				|| status == ApiModel.Football.Enums.FixtureStatusType.ET // 연장
				|| status == ApiModel.Football.Enums.FixtureStatusType.P // 승부차기
				|| status == ApiModel.Football.Enums.FixtureStatusType.FT // 종료
				|| status == ApiModel.Football.Enums.FixtureStatusType.AET // 연장 후 종료
				|| status == ApiModel.Football.Enums.FixtureStatusType.PEN) // 승부차기 후 종료
				return true;

			return false;
		}

		#endregion Check validation

		#region Add data

		private readonly object _lockObject = new object();

		private void AddInvalidLeague(InvalidLeague invalidLeague)
		{
			lock (_lockObject)
			{
				// TODO: Log
				_invalidLeauges.Add(invalidLeague);
				Dev.DebugString($"Invalid League {nameof(InvalidLeague.LeagueID)}: {invalidLeague.LeagueID}" +
								$", {nameof(InvalidLeague.LeagueName)}: {invalidLeague.LeagueName}" +
								$", {nameof(InvalidLeague.CountryName)}: {invalidLeague.CountryName}" +
								$", {nameof(InvalidLeague.ReasonType)}: {invalidLeague.ReasonType}"
								, ConsoleColor.Red);
			}
		}

		private void AddInvalidTeam(InvalidTeam invalidTeam)
		{
			lock (_lockObject)
			{
				// TODO: Log
				_invalidTeams.Add(invalidTeam);

				Dev.DebugString($"Invalid League {nameof(InvalidTeam.TeamsID)}: {invalidTeam.TeamsID}" +
								$", {nameof(InvalidTeam.TeamName)}: {invalidTeam.TeamName}" +
								$", {nameof(InvalidTeam.CountryName)}: {invalidTeam.CountryName}" +
								$", {nameof(InvalidTeam.ReasonType)}: {invalidTeam.ReasonType}"
								, ConsoleColor.Red);
			}
		}

		#endregion Add data

		#region GetError

		public InvalidLeague[] GetErrorLeauges(InvalidType invalidType, bool isRemove)
		{
			lock (_lockObject)
			{
				var result = _invalidLeauges.Where(elem => elem.ReasonType == (int)invalidType).ToArray();

				if (isRemove)
				{
					foreach (var data in result)
					{
						_invalidLeauges.Remove(data);
					}
				}

				return result;
			}
		}

		public InvalidTeam[] GetErrorTeams(InvalidType invalidType, bool isRemove)
		{
			lock (_lockObject)
			{
				var result = _invalidTeams.Where(elem => elem.ReasonType == (int)invalidType).ToArray();

				if (isRemove)
				{
					foreach (var data in result)
					{
						_invalidTeams.Remove(data);
					}
				}

				return result;
			}
		}

		public bool IsExistError(InvalidType invalidType)
		{
			return GetErrorLeauges(invalidType, false).Length > 0 || GetErrorTeams(invalidType, false).Length > 0;
		}

		#endregion GetError

		public void OutputErrorToJsonFile()
		{
			lock (_lockObject)
			{
				if (_invalidLeauges.Count > 0)
				{
					var serializeString = JsonConvert.SerializeObject(_invalidLeauges, Formatting.Indented);
					FileFacade.MakeSimpleTextFile(rootDir, "invalidLeauges.json", serializeString);
				}

				_invalidLeauges.Clear();

				if (_invalidTeams.Count > 0)
				{
					var serializeString = JsonConvert.SerializeObject(_invalidTeams, Formatting.Indented);
					FileFacade.MakeSimpleTextFile(rootDir, "invalidTeams.json", serializeString);
				}

				_invalidTeams.Clear();
			}
		}
	}
}