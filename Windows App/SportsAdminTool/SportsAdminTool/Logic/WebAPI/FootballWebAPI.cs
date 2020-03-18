using LogicCore.Debug;
using LogicCore.Utility;
using RapidAPI;
using RapidAPI.Models.Football.Enums;
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
    public class FootballWebAPI : Singleton.INode
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

        #endregion Country

        #region League

        public IList<AppModel.Football.League> GetAllAvailableLeauges()
        {
            Dev.DebugString("Call API - FootballWebAPI.GetAllAvailableLeauges");

            _api.RequestEx(_api.FootballAllAvailableLeauges, out IList<AppModel.Football.League> leagues);

            Dev.DebugString($"League Count: {leagues.Count}");

            return leagues;
        }

        public AppModel.Football.League GetLeagueByLeagueId(short LeaugeId)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetLeagueByLeagueId");

            _api.RequestEx(_api.FootballLeagueByLeagueId, (int)LeaugeId, out IList<AppModel.Football.League> leagues);

            Dev.DebugString($"League Count: {leagues.Count}");

            return leagues.FirstOrDefault();
        }

        #endregion League

        #region Team

        public IList<AppModel.Football.Team> GetAllTeamsByCountryName(string countryName)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetAllTeamsByCountryName");

            _api.RequestEx(_api.FootballTeamsByCountryName, countryName, out IList<AppModel.Football.Team> teams);

            Dev.DebugString($"Teams Count: {teams.Count}");

            return teams;
        }

        public IList<AppModel.Football.Team> GetAllTeamsByLeagueId(short leagueId)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetAllTeamsByLeagueId");

            _api.RequestEx(_api.FootballTeamsByLeagueId, (int)leagueId, out IList<AppModel.Football.Team> teams);

            Dev.DebugString($"Teams Count: {teams.Count}");

            return teams;
        }

        public AppModel.Football.Team GetTeamByTeamId(short teamId)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetTeamByTeamId");

            _api.RequestEx(_api.FootballTeamsByTeamId, (int)teamId, out IList<AppModel.Football.Team> teams);

            Dev.DebugString($"Teams Count: {teams.Count}");

            return teams.FirstOrDefault();
        }

        #endregion Team

        #region Standings

        public IList<AppModel.Football.Standing> GetStandingsByLeagueId(short leagueId)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetStandingsByLeagueId");

            _api.RequestEx(_api.StandingsByLeagueId, (int)leagueId, out IList<AppModel.Football.Standing> standings);

            Dev.DebugString($"Standing Teams Count: {standings.Count}");

            return standings;
        }

        #endregion Standings

        #region Fixture

        public IList<AppModel.Football.Fixture> GetFixturesByDate(params DateTime[] dates)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetFixturesByDate");

            var fixtureList = new List<AppModel.Football.Fixture>();
            foreach (var date in dates)
            {
                _api.RequestEx(_api.FootballFixturesByDate, date, out IList<AppModel.Football.Fixture> fixtures);
                fixtureList.AddRange(fixtures);
            }

            Dev.DebugString($"Fixtures Count: {fixtureList.Count}");

            return fixtureList;
        }

        public IList<AppModel.Football.Fixture> GetLastFixturesByTeamId(short teamId, int count)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetLastFixturesByTeamId");

            _api.RequestEx(_api.FootballLastFixturesByTeamId, (int)teamId, count, out IList<AppModel.Football.Fixture> fixtures);

            Dev.DebugString($"Fixtures Count: {fixtures.Count}");

            return fixtures;
        }

        public IList<AppModel.Football.Fixture> GetH2HFixtures(short teamId1, short teamId2)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetH2HFixtures");

            _api.RequestEx(_api.FootballH2HFixtureByTeamId, (int)teamId1, (int)teamId2, out IList<AppModel.Football.Fixture> fixtures);

            Dev.DebugString($"Fixtures Count: {fixtures.Count}");

            return fixtures;
        }

        public AppModel.Football.Fixture GetFixturesByFixtureId(int fixtureId)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetFixturesByFixtureId");

            _api.RequestEx(_api.FootballFixturesById, fixtureId, out AppModel.Football.Fixture fixture);

            Dev.DebugString($"Fixtures Exist: {fixture != null}");

            return fixture;
        }

        public IList<AppModel.Football.Fixture> GetFixturesByLeagueId(short leagueId)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetFixturesByLeagueId");

            _api.RequestEx(_api.FootballFixturesByLeagueId, (int)leagueId, out IList<AppModel.Football.Fixture> fixtures);

            Dev.DebugString($"Fixtures Count: {fixtures.Count}");

            return fixtures;
        }

        #endregion Fixture

        #region Fixture Statistics

        public AppModel.Football.FixtureStatistic GetFixtureStatisticByFixtureId(int fixtureId)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetFixtureStatisticByFixtureId");

            _api.RequestEx(_api.FootballFixtureStatisticByFixtureId, fixtureId, out AppModel.Football.FixtureStatistic fixtureStatistic);

            bool isExistData = fixtureStatistic != null;
            Dev.DebugString($"Is exist fixture statistic : {isExistData}");

            return fixtureStatistic;
        }

        #endregion Fixture Statistics

        #region Odds

        public AppModel.Football.Odds GetOddsByFixtureId(int fixtureId)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetOddsByFixtureId");

            _api.RequestEx(_api.OddsByFixtureId, fixtureId, out AppModel.Football.Odds Odds);

            Dev.DebugString($"Is exist odds : {Odds != null}");

            return Odds;
        }

        public IList<AppModel.Football.Odds> GetOddsByLeagueId(short leagueId)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetOddsByLeagueId");

            _api.RequestEx(_api.OddsByLeagueId, (int)leagueId, out IList<AppModel.Football.Odds> OddsList);

            Dev.DebugString($"Odds Count : {OddsList.Count}");

            return OddsList;
        }

        public IList<AppModel.Football.Odds> GetOddsByLeagueIdAndLabelId(short leagueId, int labelId)
        {
            Dev.DebugString("Call API - FootballWebAPI.GetOddsByLeagueId");

            _api.RequestEx(_api.OddsByLeagueIdAndLableId, (int)leagueId, (int)labelId, out IList<AppModel.Football.Odds> OddsList);

            Dev.DebugString($"Odds Count : {OddsList.Count}");

            return OddsList;
        }

        #endregion Odds
    }
}