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
        public short TeamsId;
        public string TeamName;
        public short LeagueId;
        public string CountryName;
        public int ReasonType;
    }

    public struct InvalidLeague
    {
        public short LeagueId;
        public string LeagueName;
        public string CountryName;
        public int ReasonType;
    }

    public class CheckValidation : Singleton.INode
    {
        private readonly string rootDir = ".\\Errors\\";
        private readonly List<InvalidLeague> _invalidLeauges = new List<InvalidLeague>();
        private readonly List<InvalidTeam> _invalidTeams = new List<InvalidTeam>();

        #region Check validation

        public bool IsValidLeague(short leagueId, string leagueName, string countryName, out FootballDB.Tables.League league)
        {
            league = DatabaseLogic.FootballDBFacade.SelectLeagues(where: $"id = {leagueId}").FirstOrDefault();
            bool db_result = league != null;
            if (!db_result)
            {
                AddInvalidLeague(new InvalidLeague()
                {
                    LeagueId = (short)leagueId,
                    LeagueName = leagueName,
                    CountryName = countryName,
                    ReasonType = (int)InvalidType.NotExistInDB,
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
                    TeamsId = teamId,
                    TeamName = teamName,
                    LeagueId = leagueId,
                    CountryName = countryName,
                    ReasonType = (int)InvalidType.Zero,
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
                        TeamsId = teamId,
                        TeamName = teamName,
                        LeagueId = leagueId,
                        CountryName = countryName,
                        ReasonType = (int)InvalidType.NotExistInDB,
                    });
                }
            }

            return isValidTeamId && db_result;
        }

        public bool IsValidFixtureStatus(ApiModel.Football.Enums.FixtureStatusType status, DateTime matchTime)
        {
            // 12시간이 지났는데 아직 경기가 안끝났으면 삭제
            if (matchTime < DateTime.UtcNow.AddHours(-12)
                && (status != ApiModel.Football.Enums.FixtureStatusType.FT // 종료
                && status != ApiModel.Football.Enums.FixtureStatusType.AET // 연장 후 종료
                && status != ApiModel.Football.Enums.FixtureStatusType.PEN)) // 승부차기 후 종료
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
                _invalidLeauges.Add(invalidLeague);

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
                _invalidTeams.Add(invalidTeam);

                Log4Net.WriteLog($"Invalid Team {nameof(InvalidTeam.TeamsId)}: {invalidTeam.TeamsId}" +
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
            return GetErrorLeauges(invalidType, false).Length > 0
                || GetErrorTeams(invalidType, false).Length > 0;
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