using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_GET_MATCH_PREDICTIONS : MysqlQuery<P_GET_MATCH_PREDICTIONS.Input, IEnumerable<Tables.Prediction>>
    {
        public struct Input
        {
            public int FixtureId { get; set; }
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
        }

        public override IEnumerable<Tables.Prediction> OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output = footballDB.Query<Tables.Prediction>($"SELECT * FROM prediction WHERE fixture_id={_input.FixtureId} ");
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