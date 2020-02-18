using LogicCore.Debug;
using LogicCore.Utility;
using RapidAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AppModel = SportsAdminTool.Model;

namespace SportsAdminTool.Logic.WebAPI
{
	public class FootballWebAPI :Singleton.INode
	{
		private readonly FootballAPI _api = null;
		public FootballWebAPI()
		{
			string football_url = ConfigurationManager.AppSettings["football_api_url"];
			string football_host = ConfigurationManager.AppSettings["football_api_host"];
			string football_key = ConfigurationManager.AppSettings["football_api_key"];

			_api = new FootballAPI();
			_api.Init(football_url, football_host, football_key);
		}

		#region Country
		public IList<AppModel.Football.Country> GetAllCountries()
		{
			Dev.DebugString("Call API - FootballWebAPI.GetAllCountries");

			_api.RequestEx(_api.FootballAllCountries, out IList<AppModel.Football.Country> countries);

			Dev.DebugString($"Country Count: {countries.Count}");

			return countries;
		}
		#endregion

		#region League
		public IList<AppModel.Football.League> GetAllAvailableLeauges()
		{
			Dev.DebugString("Call API - FootballWebAPI.GetAllAvailableLeauges");

			_api.RequestEx(_api.FootballAllAvailableLeauges, out IList<AppModel.Football.League> leagues);

			Dev.DebugString($"League Count: {leagues.Count}");

			return leagues;
		}

		public AppModel.Football.League GetLeagueByLeagueID(short LeaugeID)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetLeagueByLeagueID");

			_api.RequestEx(_api.FootballLeagueByLeagueID, (int)LeaugeID,out IList<AppModel.Football.League> leagues);

			Dev.DebugString($"League Count: {leagues.Count}");

			return leagues.FirstOrDefault();
		}
		#endregion

		#region Team
		public IList<AppModel.Football.Team> GetAllTeamsByCountryName(string countryName)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetAllTeamsByCountryName");

			_api.RequestEx(_api.FootballTeamsByCountryName, countryName, out IList<AppModel.Football.Team> teams);

			Dev.DebugString($"Teams Count: {teams.Count}");

			return teams;
		}

		public IList<AppModel.Football.Team> GetAllTeamsByLeagueID(short leagueID)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetAllTeamsByLeagueID");

			_api.RequestEx(_api.FootballTeamsByLeagueID, (int)leagueID, out IList<AppModel.Football.Team> teams);

			Dev.DebugString($"Teams Count: {teams.Count}");

			return teams;
		}

		public AppModel.Football.Team GetTeamByTeamID(short teamID)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetTeamByTeamID");

			_api.RequestEx(_api.FootballTeamsByTeamID, (int)teamID, out IList<AppModel.Football.Team> teams);

			Dev.DebugString($"Teams Count: {teams.Count}");

			return teams.FirstOrDefault();
		}
		#endregion

		#region Standings
		public IList<AppModel.Football.Standing> GetStandingsByLeagueID(short leagueID)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetStandingsByLeagueID");

			_api.RequestEx(_api.StandingsByLeagueID, (int)leagueID, out IList<AppModel.Football.Standing> standings);

			Dev.DebugString($"Standing Teams Count: {standings.Count}");

			return standings;
		}
		#endregion

		#region Fixture
		public IList<AppModel.Football.Fixture> GetFixturesByDate(params DateTime[] dates)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetFixturesByDate");

			var fixtureList = new List<AppModel.Football.Fixture>();
			foreach(var date in dates)
			{
				_api.RequestEx(_api.FootballFixturesByDate, date, out IList<AppModel.Football.Fixture> fixtures);
				fixtureList.AddRange(fixtures);
			}

			Dev.DebugString($"Fixtures Count: {fixtureList.Count}");

			return fixtureList;
		}

		public IList<AppModel.Football.Fixture> GetLastFixturesByTeamID(short teamID, int count)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetLastFixturesByTeamID");

			_api.RequestEx(_api.FootballLastFixturesByTeamID, (int)teamID, count, out IList <AppModel.Football.Fixture> fixtures);

			Dev.DebugString($"Fixtures Count: {fixtures.Count}");

			return fixtures;
		}

		public IList<AppModel.Football.Fixture> GetH2HFixtures(short teamID1, short teamID2)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetH2HFixtures");

			_api.RequestEx(_api.FootballH2HFixtureByTeamID, (int)teamID1, (int)teamID2, out IList<AppModel.Football.Fixture> fixtures);

			Dev.DebugString($"Fixtures Count: {fixtures.Count}");

			return fixtures;
		}

		public AppModel.Football.Fixture GetFixturesByFixtureID(int fixtureID)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetFixturesByFixtureID");

			_api.RequestEx(_api.FootballFixturesByID, fixtureID, out AppModel.Football.Fixture fixtures);

			bool isExistData = fixtures != null;
			Dev.DebugString($"Fixtures Exist: {isExistData}");

			return fixtures;
		}
		#endregion

		#region Fixture Statistics
		public AppModel.Football.FixtureStatistic GetFixtureStatisticByFixtureID(int fixtureID)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetFixtureStatisticByFixtureID");

			_api.RequestEx(_api.FootballFixtureStatisticByFixtureID, fixtureID, out AppModel.Football.FixtureStatistic fixtureStatistic);

			bool isExistData = fixtureStatistic != null;
			Dev.DebugString($"Is exist fixture statistic : {isExistData}");

			return fixtureStatistic;
		}
		#endregion

		#region Odds
		public AppModel.Football.Odds GetOddsByFixtureID(int fixtureID)
		{
			Dev.DebugString("Call API - FootballWebAPI.GetFixtureStatisticByFixtureID");

			_api.RequestEx(_api.OddsByFixtureID, fixtureID, out AppModel.Football.Odds Odds);

			bool isExistData = Odds != null;
			Dev.DebugString($"Is exist odds : {isExistData}");

			return Odds;
		}
		#endregion
	}
}
