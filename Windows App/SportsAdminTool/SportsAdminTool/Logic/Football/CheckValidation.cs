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
using LogicCore.Utility.ThirdPartyLog;
using SportsAdminTool.Model.Resource.Football;

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
        public short TeamId;
        public string TeamName;
        public short LeagueId;
        public string CountryName;
        public InvalidType ReasonType;

        public string TeamKey => $"{TeamId}:{CountryName}:{TeamName}";
    }

    public struct InvalidLeague
    {
        public short LeagueId;
        public string LeagueName;
        public string CountryName;
        public InvalidType ReasonType;
        public string LeagueKey => $"{LeagueId}:{CountryName}:{LeagueName}";
    }

    public class CheckValidation : Singleton.INode
    {
        private readonly string rootDir = ".\\Errors\\";
        private readonly Dictionary<string, InvalidLeague> _invalidLeauges = new Dictionary<string, InvalidLeague>();
        private readonly Dictionary<string, InvalidTeam> _invalidTeams = new Dictionary<string, InvalidTeam>();

        #region Check validation

        public bool IsValidLeague(short leagueId, string leagueName, string countryName, out FootballDB.Tables.League league, out FootballDB.Tables.LeagueCoverage leagueCoverage)
        {
            league = DatabaseLogic.FootballDBFacade.SelectLeagues(where: $"id = {leagueId}").FirstOrDefault();
            leagueCoverage = DatabaseLogic.FootballDBFacade.SelectCoverages(where: $"league_id = {leagueId}").FirstOrDefault();

            bool db_result = league != null;
            if (!db_result)
            {
                AddInvalidLeague(new InvalidLeague()
                {
                    LeagueId = (short)leagueId,
                    LeagueName = leagueName,
                    CountryName = countryName,
                    ReasonType = InvalidType.NotExistInDB,
                });
            }

            return db_result;
        }

        public bool IsValidTeam(short teamId, string teamName, short leagueId, string countryName, bool isDB_check)
        {
            bool isValidTeamId = teamId != 0;
            if (!isValidTeamId)
            {
                AddInvalidTeam(new InvalidTeam()
                {
                    TeamId = teamId,
                    TeamName = teamName,
                    LeagueId = leagueId,
                    CountryName = countryName,
                    ReasonType = InvalidType.Zero,
                });
            }

            bool db_result = true;
            if (isValidTeamId && isDB_check)
            {
                db_result = DatabaseLogic.FootballDBFacade.SelectTeams(where: $"id = {teamId}").FirstOrDefault() != null;
                if (!db_result)
                {
                    AddInvalidTeam(new InvalidTeam()
                    {
                        TeamId = teamId,
                        TeamName = teamName,
                        LeagueId = leagueId,
                        CountryName = countryName,
                        ReasonType = InvalidType.NotExistInDB,
                    });
                }
            }

            return isValidTeamId && db_result;
        }

        public bool IsValidFixtureStatus(ApiModel.Football.Enums.FixtureStatusType status, DateTime matchTime)
        {
            if (DateTime.UtcNow > matchTime.AddHours(6)
                && status != ApiModel.Football.Enums.FixtureStatusType.FT // 경기전
                && status != ApiModel.Football.Enums.FixtureStatusType.AET // 연장 후 종료
                && status != ApiModel.Football.Enums.FixtureStatusType.PEN) // 승부차기 후 종료
                return false;

            if (status == ApiModel.Football.Enums.FixtureStatusType.NS // 경기전
                || status == ApiModel.Football.Enums.FixtureStatusType.FH // 전반
                || status == ApiModel.Football.Enums.FixtureStatusType.HT // 하프타임
                || status == ApiModel.Football.Enums.FixtureStatusType.SH // 후반
                || status == ApiModel.Football.Enums.FixtureStatusType.ET // 연장
                || status == ApiModel.Football.Enums.FixtureStatusType.P // 승부차기
                || status == ApiModel.Football.Enums.FixtureStatusType.BT // 연장 브레이크 타임
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
                if (!_invalidLeauges.ContainsKey(invalidLeague.LeagueKey))
                    _invalidLeauges.Add(invalidLeague.LeagueKey, invalidLeague);

                Log4Net.WriteLog($"Invalid League {nameof(InvalidLeague.LeagueId)}: {invalidLeague.LeagueId}" +
                                $", {nameof(InvalidLeague.LeagueName)}: {invalidLeague.LeagueName}" +
                                $", {nameof(InvalidLeague.CountryName)}: {invalidLeague.CountryName}" +
                                $", {nameof(InvalidLeague.ReasonType)}: {invalidLeague.ReasonType}"
                                , Log4Net.Level.ERROR);
            }
        }

        private void AddInvalidTeam(InvalidTeam invalidTeam)
        {
            lock (_lockObject)
            {
                if (!_invalidTeams.ContainsKey(invalidTeam.TeamKey))
                    _invalidTeams.Add(invalidTeam.TeamKey, invalidTeam);

                Log4Net.WriteLog($"Invalid Team {nameof(InvalidTeam.TeamId)}: {invalidTeam.TeamId}" +
                                $", {nameof(InvalidTeam.TeamName)}: {invalidTeam.TeamName}" +
                                $", {nameof(InvalidTeam.CountryName)}: {invalidTeam.CountryName}" +
                                $", {nameof(InvalidTeam.ReasonType)}: {invalidTeam.ReasonType}"
                                , Log4Net.Level.ERROR);
            }
        }

        #endregion Add data

        #region GetError

        public InvalidLeague[] GetErrorLeauges(InvalidType invalidType, bool isRemove)
        {
            lock (_lockObject)
            {
                var result = _invalidLeauges.Values.Where(elem => elem.ReasonType == invalidType).ToArray();

                if (isRemove)
                {
                    foreach (var data in result)
                    {
                        _invalidLeauges.Remove(data.LeagueKey);
                    }
                }

                return result;
            }
        }

        public void DeleteErrorLeague(InvalidLeague league)
        {
            lock (_lockObject)
            {
                _invalidLeauges.Remove(league.LeagueKey);
            }
        }

        public InvalidTeam[] GetErrorTeams(InvalidType invalidType, bool isRemove)
        {
            lock (_lockObject)
            {
                var result = _invalidTeams.Values.Where(elem => elem.ReasonType == invalidType).ToArray();

                if (isRemove)
                {
                    foreach (var data in result)
                    {
                        _invalidTeams.Remove(data.TeamKey);
                    }
                }

                return result;
            }
        }

        public void DeleteErrorTeam(InvalidTeam team)
        {
            lock (_lockObject)
            {
                _invalidTeams.Remove(team.TeamKey);
            }
        }

        public bool IsExistError(InvalidType invalidType)
        {
            return GetErrorLeauges(invalidType, false).Length > 0
                || GetErrorTeams(invalidType, false).Length > 0;
        }

        #endregion GetError

        public void OutputErrorToJsonFile(string fileName)
        {
            lock (_lockObject)
            {
                if (_invalidLeauges.Count > 0)
                {
                    var invalidLeauges = _invalidLeauges.Values.OrderBy(elem => elem.CountryName).ToList();

                    var serializeString = JsonConvert.SerializeObject(invalidLeauges, Formatting.Indented);
                    FileFacade.MakeSimpleTextFile(rootDir, fileName, serializeString);
                }

                _invalidLeauges.Clear();

                if (_invalidTeams.Count > 0)
                {
                    var invalidTeams = _invalidTeams.Values.OrderBy(elem => elem.CountryName).ToList();

                    var serializeString = JsonConvert.SerializeObject(invalidTeams, Formatting.Indented);
                    FileFacade.MakeSimpleTextFile(rootDir, fileName, serializeString);
                }

                _invalidTeams.Clear();
            }
        }
    }
}