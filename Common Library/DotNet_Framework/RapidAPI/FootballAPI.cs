using RapidAPI.Models.Football;
using RapidAPI.Models.Football.Response;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI
{
	public class FootballAPI : IRapidAPI
	{
		string _end_point;
		RequestBuilder _requestBuilder;

		public void Init(string endPoint, string host, string key)
		{
			_end_point = endPoint;
			_requestBuilder = new RequestBuilder(host, key);
		}

		#region Fixture
		/// <summary>
		/// Get all available from one {date}
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public IList<Fixture> FootballFixturesByDate(DateTime date)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"fixtures/date/{searchDate}",
				Method.GET,
				new { searchDate = date.Date.ToString("yyyy-MM-dd") },
				new { timeZone = "Europe/London" });

			var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

			return response.Api.Fixtures;
		}

		/// <summary>
		/// Get fixture from one {fixture_id}
		/// In this request events, lineups, statistics and players fixture are returned in the response
		/// </summary>
		/// <param name="fixtureID"></param>
		/// <returns></returns>
		public Fixture FootballFixturesByID(int fixtureID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"fixtures/id/{fixtureID}",
				Method.GET,
				new { fixtureID },
				new { timeZone = "Europe/London" });

			var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

			return response.Api.Fixtures.FirstOrDefault();
		}

		/// <summary>
		/// Get all available from one {team_id}
		/// </summary>
		/// <param name="teamID"></param>
		/// <returns></returns>
		public IList<Fixture> TeamFixturesByTeamID(int teamID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"fixtures/team/{teamID}",
				Method.GET,
				new { teamID },
				new { timeZone = "Europe/London" });

			var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

			return response.Api.Fixtures;
		}

		/// <summary>
		/// Get all head to head between two {team_id} 
		/// In this request team comparison and teams statistics are also returned in the response
		/// </summary>
		/// <param name="teamID1"></param>
		/// <param name="teamID2"></param>
		/// <returns></returns>
		public IList<Fixture> FootballH2HFixtureByTeamID(int teamID1, int teamID2)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"fixtures/h2h/{teamID1}/{teamID2}",
				Method.GET,
				new { teamID1, teamID2 },
				new { timeZone = "Europe/London" });

			var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

			return response.Api.Fixtures;
		}

		/// <summary>
		/// Get all available from one {league_id}
		/// </summary>
		/// <param name="leagueID"></param>
		/// <returns></returns>
		public IList<Fixture> FootballFixturesByLeagueID(int leagueID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"fixtures/league/{leagueID}",
				Method.GET,
				new { leagueID },
				new { timeZone = "Europe/London" });

			var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

			return response.Api.Fixtures;
		}

		/// <summary>
		/// Get x next available fixtures from one {team_id}
		/// </summary>
		/// <param name="teamID"></param>
		/// <returns></returns>
		public IList<Fixture> FootballLastFixturesByTeamID(int teamID, int count)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"fixtures/team/{teamID}/last/{count}",
				Method.GET,
				new { teamID, count },
				new { timeZone = "Europe/London" });

			var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

			return response.Api.Fixtures;
		}
		#endregion

		#region Fixture Statistics
		/// <summary>
		/// Get all available statistics from one {fixture_id}
		/// </summary>
		/// <param name="leagueId"></param>
		/// <returns></returns>
		public FixtureStatistic FootballFixtureStatisticByFixtureID(int fixtureID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"statistics/fixture/{fixtureID}",
				Method.GET,
				new { fixtureID });

			var response = apiClient.ExecuteEx<FootballApiResponse<FixtureStatisticResponse>>(request);

			return response.Api.Results == 0 ? null : response.Api.FixtureStatistic;
		}
		#endregion

		#region Country
		/// <summary>
		/// Get all available {country} in the API across all {season} and competitions
		/// </summary>
		/// <param name="leagueID"></param>
		/// <returns></returns>
		public IList<Country> FootballAllCountries()
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"countries",
				Method.GET);

			var response = apiClient.ExecuteEx<FootballApiResponse<CountriesResponse>>(request);

			return response.Api.Countries;
		}
		#endregion

		#region Season
		/// <summary>
		/// Allows to retrieve all the seasons available for a {league_id}
		/// </summary>
		/// <param name="leagueID"></param>
		/// <returns></returns>
		public IList<LeagueDetatil> FootballSeasonsByLeagueID(int leagueID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"leagues/seasonsAvailable/{leagueID}",
				Method.GET,
				new { leagueID });

			var response = apiClient.ExecuteEx<FootballApiResponse<LeaguesResponse>>(request);

			return response.Api.Leagues;
		}
		#endregion

		#region League
		/// <summary>
		/// Get all available leagues
		/// </summary>
		/// <param name="leagueID"></param>
		/// <returns></returns>
		public IList<LeagueDetatil> FootballAllAvailableLeauges()
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"leagues",
				Method.GET);

			var response = apiClient.ExecuteEx<FootballApiResponse<LeaguesResponse>>(request);

			return response.Api.Leagues;
		}

		/// <summary>
		/// Get one league from one {league_id}
		/// </summary>
		/// <param name="leagueID"></param>
		/// <returns></returns>
		public IList<LeagueDetatil> FootballLeagueByLeagueID(int leaugeID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"leagues/league/{leaugeID}",
				Method.GET,
				new { leaugeID });

			var response = apiClient.ExecuteEx<FootballApiResponse<LeaguesResponse>>(request);

			return response.Api.Leagues;
		}
		#endregion

		#region Standings
		/// <summary>
		/// Get all Standings from one {league_id}
		/// </summary>
		/// <param name="leagueId"></param>
		/// <returns></returns>
		public IList<Standing> StandingsByLeagueID(int leagueId)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"leagueTable/{leagueId}",
				Method.GET,
				new { leagueId });

			var response = apiClient.ExecuteEx<FootballApiResponse<StandingsResponse>>(request);

			return response.Api.Standings;
		}
		#endregion

		#region Team
		/// <summary>
		/// Get all statistics for a {team_id} in a {league_id}
		/// </summary>
		/// <param name="leagueID"></param>
		/// <param name="teamID"></param>
		/// <returns></returns>
		public TeamStatistics TeamStatisticsByLeagueIDAndTeamID(int leagueID, int teamID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"statistics/{leagueID}/{teamID}",
				Method.GET,
				new { leagueID, teamID });

			var response = apiClient.ExecuteEx<FootballApiResponse<TeamStatisticsResponse>>(request);

			return response.Api.TeamStatistics;
		}

		/// <summary>
		/// Allows you to search for a team in relation to a team {name} or {country}
		/// Spaces must be replaced by underscore for better search performance.
		/// EX : Real madrid => real_madrid
		/// </summary>
		/// <param name="leagueID"></param>
		/// <returns></returns>
		public IList<Team> FootballTeamsByCountryName(string countryName)
		{
			var apiClient = new RestClient(_end_point);

			countryName = countryName.Replace(" ", "_");

			var request = _requestBuilder.Build(
				"teams/search/{countryName}",
				Method.GET,
				new { countryName });

			var response = apiClient.ExecuteEx<FootballApiResponse<TeamResponse>>(request);

			return response.Api.Teams;
		}

		/// <summary>
		/// Get all teams from one {league_id}
		/// </summary>
		/// <param name="leagueID"></param>
		/// <returns></returns>
		public IList<Team> FootballTeamsByLeagueID(int leagueID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"teams/league/{leagueID}",
				Method.GET,
				new { leagueID });

			var response = apiClient.ExecuteEx<FootballApiResponse<TeamResponse>>(request);

			return response.Api.Teams;
		}

		/// <summary>
		/// Get one team from one {team_id}
		/// </summary>
		/// <param name="leagueID"></param>
		/// <returns></returns>
		public IList<Team> FootballTeamsByTeamID(int teamID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"teams/team/{teamID}",
				Method.GET,
				new { teamID });

			var response = apiClient.ExecuteEx<FootballApiResponse<TeamResponse>>(request);

			return response.Api.Teams;
		}
		#endregion

		#region Player
		/// <summary>
		/// Get all players for one {team_id} and {season}
		/// You can find the available {season} by using the endpoint players seasons
		/// </summary>
		/// <param name="teamID"></param>
		/// <param name="season"></param>
		/// <returns></returns>
		public IList<Player> FootballPlayersByTeamIDAndSeason(int teamID, string season)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"players/team/{teamID}/{season}",
				Method.GET,
				new { teamID, season });

			var response = apiClient.ExecuteEx<FootballApiResponse<PlayersResponse>>(request);

			return response.Api.Players;
		}

		/// <summary>
		/// Get the 20 best players from a {league_id}
		/// </summary>
		/// <param name="leagueID"></param>
		/// <returns></returns>
		public IList<Player> FootballTopScorerByLeagueID(int leagueID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"topscorers/{leagueID}",
				Method.GET,
				new { leagueID });

			var response = apiClient.ExecuteEx<FootballApiResponse<PlayersResponse>>(request);

			return response.Api.Players;
		}
		#endregion

		#region Odds
		/// <summary>
		/// Get all available odds from one {league_id} & {label_id}
		/// </summary>
		/// <param name="leagueID"></param>
		/// <param name="labelID"></param>
		/// <returns></returns>
		public IList<Odds> OddsByLeagueIDAndLableID(int leagueID, int labelID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"odds/league/{leagueID}/label/{labelID}",
				Method.GET,
				new { leagueID, labelID });

			var response = apiClient.ExecuteEx<FootballApiResponse<OddsResponse>>(request);

			return response.Api.OddsList;
		}

		/// <summary>
		/// Get all available odds from one {league_id} & {bookmaker_id}
		/// </summary>
		/// <param name="leagueID"></param>
		/// <param name="bookmakerID"></param>
		/// <returns></returns>
		public IList<Odds> OddsByLeagueIDAndBookmakerID(int leagueID, int bookmakerID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"odds/league/{leagueID}/bookmaker/{bookmakerID}",
				Method.GET,
				new { leagueID, bookmakerID });

			var response = apiClient.ExecuteEx<FootballApiResponse<OddsResponse>>(request);

			return response.Api.OddsList;
		}

		/// <summary>
		/// odds from one {fixture_id}
		/// </summary>
		/// <param name="fixtureID"></param>
		/// <returns></returns>
		public Odds OddsByFixtureID(int fixtureID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"odds/fixture/{fixtureID}",
				Method.GET,
				new { fixtureID });

			var response = apiClient.ExecuteEx<FootballApiResponse<OddsResponse>>(request);

			return response.Api.OddsList.FirstOrDefault();
		}
		#endregion

		#region Prediction
		/// <summary>
		/// Get all available predictions from one {fixture_id}
		/// </summary>
		/// <param name="fixtureID"></param>
		/// <returns></returns>
		public IList<Prediction> FootballPredictionByFixtureID(int fixtureID)
		{
			var apiClient = new RestClient(_end_point);

			var request = _requestBuilder.Build(
				"predictions/{fixtureID}",
				Method.GET,
				new { fixtureID });

			var response = apiClient.ExecuteEx<FootballApiResponse<PredictionResponse>>(request);

			return response.Api.Predictions;
		}
		#endregion
	}
}
