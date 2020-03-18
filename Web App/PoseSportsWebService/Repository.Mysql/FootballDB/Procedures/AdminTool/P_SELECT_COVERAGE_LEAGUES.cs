using Repository.Mysql.Dapper;
using Repository.Mysql.FootballDB.Tables;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_SELECT_COVERAGE_LEAGUES : MysqlQuery<string, IEnumerable<P_SELECT_COVERAGE_LEAGUES.Output>>
    {
        public class Output
        {
            public string LeagueName { get; set; }
            public string LeagueLogo { get; set; }
            public string LeagueType { get; set; }
            public string CountryName { get; set; }
            public string CountryLogo { get; set; }
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
            sb.Append($"SELECT l.{nameof(League.name)} as {nameof(Output.LeagueName)}, ");
            sb.Append($"l.{nameof(League.logo)} as {nameof(Output.LeagueLogo)}, l.{nameof(League.type)} as {nameof(Output.LeagueType)}, ");
            sb.Append($"c.{nameof(Country.name)} as {nameof(Output.CountryName)}, c.{nameof(Country.logo)} as {nameof(Output.CountryLogo)} ");
            sb.Append("FROM league as l ");
            sb.Append($"INNER JOIN country as c on l.{nameof(League.country_name)} = c.{nameof(Country.name)} ");
            sb.Append($"WHERE l.{nameof(League.is_predict_coverage)} = 1 GROUP BY l.{nameof(League.name)}, l.{nameof(League.type)}, l.{nameof(League.country_name)};");

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output = footballDB.Query<Output>(sb.ToString());
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