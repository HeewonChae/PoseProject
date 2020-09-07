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
        private string _end_point;
        private RequestBuilder _requestBuilder;

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
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        public Fixture FootballFixturesById(int fixtureId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "fixtures/id/{fixtureId}",
                Method.GET,
                new { fixtureId },
                new { timeZone = "Europe/London" });

            var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

            return response.Api.Fixtures?.FirstOrDefault();
        }

        /// <summary>
        /// Get all available from one {team_id}
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public IList<Fixture> TeamFixturesByTeamId(int teamId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "fixtures/team/{teamId}",
                Method.GET,
                new { teamId },
                new { timeZone = "Europe/London" });

            var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

            return response.Api.Fixtures;
        }

        /// <summary>
        /// Get all head to head between two {team_id}
        /// In this request team comparison and teams statistics are also returned in the response
        /// </summary>
        /// <param name="teamId1"></param>
        /// <param name="teamId2"></param>
        /// <returns></returns>
        public IList<Fixture> FootballH2HFixtureByTeamId(int teamId1, int teamId2)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "fixtures/h2h/{teamId1}/{teamId2}",
                Method.GET,
                new { teamId1, teamId2 },
                new { timeZone = "Europe/London" });

            var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

            return response.Api.Fixtures;
        }

        /// <summary>
        /// Get all available from one {league_id}
        /// </summary>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IList<Fixture> FootballFixturesByLeagueId(int leagueId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "fixtures/league/{leagueId}",
                Method.GET,
                new { leagueId },
                new { timeZone = "Europe/London" });

            var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

            return response.Api.Fixtures;
        }

        /// <summary>
        /// Get x next available fixtures from one {team_id}
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public IList<Fixture> FootballLastFixturesByTeamId(int teamId, int count)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "fixtures/team/{teamId}/last/{count}",
                Method.GET,
                new { teamId, count },
                new { timeZone = "Europe/London" });

            var response = apiClient.ExecuteEx<FootballApiResponse<FixturesResponse>>(request);

            return response.Api.Fixtures;
        }

        #endregion Fixture

        #region Fixture Statistics

        /// <summary>
        /// Get all available statistics from one {fixture_id}
        /// </summary>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public FixtureStatistic FootballFixtureStatisticByFixtureId(int fixtureId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "statistics/fixture/{fixtureId}",
                Method.GET,
                new { fixtureId });

            var response = apiClient.ExecuteEx<FootballApiResponse<FixtureStatisticResponse>>(request);

            return response.Api.Results == 0 ? null : response.Api.FixtureStatistic;
        }

        #endregion Fixture Statistics

        #region Country

        /// <summary>
        /// Get all available {country} in the API across all {season} and competitions
        /// </summary>
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

        #endregion Country

        #region Season

        /// <summary>
        /// Allows to retrieve all the seasons available for a {league_id}
        /// </summary>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IList<LeagueDetatil> FootballSeasonsByLeagueId(int leagueId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "leagues/seasonsAvailable/{leagueId}",
                Method.GET,
                new { leagueId });

            var response = apiClient.ExecuteEx<FootballApiResponse<LeaguesResponse>>(request);

            return response.Api.Leagues;
        }

        #endregion Season

        #region League

        /// <summary>
        /// Get all available leagues
        /// </summary>
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
        /// <param name="leaugeId"></param>
        /// <returns></returns>
        public IList<LeagueDetatil> FootballLeagueByLeagueId(int leaugeId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "leagues/league/{leaugeId}",
                Method.GET,
                new { leaugeId });

            var response = apiClient.ExecuteEx<FootballApiResponse<LeaguesResponse>>(request);

            return response.Api.Leagues;
        }

        #endregion League

        #region Standings

        /// <summary>
        /// Get all Standings from one {league_id}
        /// </summary>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IList<Standings> StandingsByLeagueId(int leagueId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "leagueTable/{leagueId}",
                Method.GET,
                new { leagueId });

            var response = apiClient.ExecuteEx<FootballApiResponse<StandingsResponse>>(request);

            return response.Api.Standingsies;
        }

        #endregion Standings

        #region Team

        /// <summary>
        /// Get all statistics for a {team_id} in a {league_id}
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public TeamStatistics TeamStatisticsByLeagueIdAndTeamId(int leagueId, int teamId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "statistics/{leagueId}/{teamId}",
                Method.GET,
                new { leagueId, teamId });

            var response = apiClient.ExecuteEx<FootballApiResponse<TeamStatisticsResponse>>(request);

            return response.Api.TeamStatistics;
        }

        /// <summary>
        /// Allows you to search for a team in relation to a team {name} or {country}
        /// Spaces must be replaced by underscore for better search performance.
        /// EX : Real madrid => real_madrid
        /// </summary>
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
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IList<Team> FootballTeamsByLeagueId(int leagueId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "teams/league/{leagueId}",
                Method.GET,
                new { leagueId });

            var response = apiClient.ExecuteEx<FootballApiResponse<TeamResponse>>(request);

            return response.Api.Teams;
        }

        /// <summary>
        /// Get one team from one {team_id}
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public IList<Team> FootballTeamsByTeamId(int teamId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "teams/team/{teamId}",
                Method.GET,
                new { teamId });

            var response = apiClient.ExecuteEx<FootballApiResponse<TeamResponse>>(request);

            return response.Api.Teams;
        }

        #endregion Team

        #region Player

        /// <summary>
        /// Get all players for one {team_id} and {season}
        /// You can find the available {season} by using the endpoint players seasons
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="season"></param>
        /// <returns></returns>
        public IList<Player> FootballPlayersByTeamIdAndSeason(int teamId, string season)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "players/team/{teamId}/{season}",
                Method.GET,
                new { teamId, season });

            var response = apiClient.ExecuteEx<FootballApiResponse<PlayersResponse>>(request);

            return response.Api.Players;
        }

        /// <summary>
        /// Get the 20 best players from a {league_id}
        /// </summary>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public IList<Player> FootballTopScorerByLeagueId(int leagueId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "topscorers/{leagueId}",
                Method.GET,
                new { leagueId });

            var response = apiClient.ExecuteEx<FootballApiResponse<PlayersResponse>>(request);

            return response.Api.Players;
        }

        #endregion Player

        #region Odds

        /// <summary>
        /// Get all available odds from one {league_id} & {label_id}
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="labelId"></param>
        /// <returns></returns>
        public IList<Odds> OddsByLeagueIdAndLableId(int leagueId, int labelId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "odds/league/{leagueId}/label/{labelId}",
                Method.GET,
                new { leagueId, labelId });

            var response = apiClient.ExecuteEx<FootballApiResponse<OddsResponse>>(request);

            return response.Api.OddsList;
        }

        /// <summary>
        /// Get all available odds from one {league_id} & {bookmaker_id}
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="bookmakerId"></param>
        /// <returns></returns>
        public IList<Odds> OddsByLeagueIdAndBookmakerId(int leagueId, int bookmakerId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "odds/league/{leagueId}/bookmaker/{bookmakerId}",
                Method.GET,
                new { leagueId, bookmakerId });

            var response = apiClient.ExecuteEx<FootballApiResponse<OddsResponse>>(request);

            return response.Api.OddsList;
        }

        /// <summary>
        /// odds from one {fixture_id}
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        public Odds OddsByFixtureId(int fixtureId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "odds/fixture/{fixtureId}",
                Method.GET,
                new { fixtureId });

            var response = apiClient.ExecuteEx<FootballApiResponse<OddsResponse>>(request);

            return response.Api.OddsList.FirstOrDefault();
        }

        /// <summary>
        /// Get all available odds from one {league_id}
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        public IList<Odds> OddsByLeagueId(int leagueId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "odds/league/{leagueId}",
                Method.GET,
                new { leagueId });

            var response = apiClient.ExecuteEx<FootballApiResponse<OddsResponse>>(request);

            return response.Api.OddsList;
        }

        #endregion Odds

        #region Prediction

        /// <summary>
        /// Get all available predictions from one {fixture_id}
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        public IList<Prediction> FootballPredictionByFixtureId(int fixtureId)
        {
            var apiClient = new RestClient(_end_point);

            var request = _requestBuilder.Build(
                "predictions/{fixtureId}",
                Method.GET,
                new { fixtureId });

            var response = apiClient.ExecuteEx<FootballApiResponse<PredictionResponse>>(request);

            return response.Api.Predictions;
        }

        #endregion Prediction
    }
}