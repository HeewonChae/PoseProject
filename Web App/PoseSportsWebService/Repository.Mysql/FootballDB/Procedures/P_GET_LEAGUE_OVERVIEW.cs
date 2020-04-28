using Repository.Mysql.Dapper;
using Repository.Mysql.FootballDB.OutputModels;
using Repository.Mysql.FootballDB.Tables;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_GET_LEAGUE_OVERVIEW : MysqlQuery<P_GET_LEAGUE_OVERVIEW.Input, P_GET_LEAGUE_OVERVIEW.Output>
    {
        public string LeagueQuery;
        public string TeamsQuery;
        public string StandingsQuery;

        public struct Input
        {
            public string CountryName { get; set; }
            public string LeagueName { get; set; }
        }

        public class Output
        {
            public DB_FootballLeagueDetail LeagueDetail { get; set; }
            public IEnumerable<DB_FootballTeamDetail> ParticipatingTeamDetails { get; set; }
            public IEnumerable<DB_FootballStandingsDetail> StandingsDetails { get; set; }

            public int Result { get; set; }
        }

        public override void OnAlloc()
        {
            base.OnAlloc();
        }

        public override void OnFree()
        {
            base.OnFree();
        }

        public override void BindParameters()
        {
            LeagueQuery = $"{DB_FootballLeagueDetail.SelectQuery} WHERE l.{nameof(League.country_name)} = \'{_input.CountryName}\' AND l.{nameof(League.name)} = \'{_input.LeagueName}\' AND l.{nameof(League.is_current)} = 1";
            StandingsQuery = $"{DB_FootballStandingsDetail.SelectQuery} WHERE s.{nameof(Standings.league_id)} = @LeagueId AND s.{nameof(Standings.group)} <> \"\";";
            TeamsQuery = $"{DB_FootballTeamDetail.SelectQuery} WHERE t.{nameof(Team.id)} in @TeamIds";
        }

        public override P_GET_LEAGUE_OVERVIEW.Output OnQuery()
        {
            _output = new Output();

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output.LeagueDetail = footballDB.Query<DB_FootballLeagueDetail>(LeagueQuery).FirstOrDefault();
                        if (_output.LeagueDetail == null)
                        {
                            _output.Result = 1;
                            return;
                        }

                        _output.StandingsDetails = footballDB.Query<DB_FootballStandingsDetail>(StandingsQuery, new { LeagueId = _output.LeagueDetail.Id });
                        if (_output.StandingsDetails.Count() == 0)
                        {
                            var participatingTeams = footballDB.Query<int>("SELECT home_team_id as team_id FROM fixture WHERE league_id = @LeagueId " +
                                "UNION SELECT away_team_id as team_id FROM fixture WHERE league_id = @LeagueId;", new { LeagueId = _output.LeagueDetail.Id }).ToArray();

                            if (participatingTeams.Length > 0)
                                _output.ParticipatingTeamDetails = footballDB.Query<DB_FootballTeamDetail>(TeamsQuery, new { TeamIds = participatingTeams });
                        }

                        _output.Result = 0;
                    },
                    this.OnError);

            return _output;
        }

        public override void OnError(EntityStatus entityStatus)
        {
            base.OnError(entityStatus);

            // Error Control
        }
    }
}