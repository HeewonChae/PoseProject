using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_SELECT_FIXTURES_DETAIL : MysqlQuery<P_SELECT_FIXTURES_DETAIL.Input, IEnumerable<P_SELECT_FIXTURES_DETAIL.Output>>
    {
        public struct Input
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        public class Output
        {
            public string CountryName { get; set; }
            public string CountryLogo { get; set; }
            public string LeagueName { get; set; }
            public string LeagueLogo { get; set; }
            public string HomeTeamName { get; set; }
            public string HomeTeamLogo { get; set; }
            public string AwayTeamName { get; set; }
            public string AwayTeamLogo { get; set; }
            public string MatchStatus { get; set; }
            public DateTime MatchTime { get; set; }
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
            // if you need Binding Parameters, write here
        }

        public override IEnumerable<Output> OnQuery()
        {
            var sb = new StringBuilder();
            sb.Append("SELECT c.name as CountryName, c.flag as CountryLogo, l.name as LeagueName, l.logo as LeagueLogo, ");
            sb.Append("ht.name as HomeTeamName, ht.logo as HomeTeamLogo, at.name as AwayTeamName, at.logo as AwayTeamLogo, ");
            sb.Append("f.status as MatchStatus, f.event_date as MatchTime FROM fixture as f ");
            sb.Append("INNER JOIN league as l on f.league_id = l.id ");
            sb.Append("INNER JOIN country as c on l.country_name = c.name ");
            sb.Append("INNER JOIN team as ht on f.home_team_id = ht.id ");
            sb.Append("INNER JOIN team as at on f.away_team_id = at.id ");
            sb.Append("WHERE f.event_date BETWEEN @StartTime AND @EndTime;");

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output = footballDB.QuerySQL<Output>(sb.ToString(), _input);
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