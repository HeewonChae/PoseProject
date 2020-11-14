using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_DELETE_VIP_PICK : MysqlQuery<P_DELETE_VIP_PICK.Input, int>
    {
        public struct Input
        {
            public int FixtureId { get; set; }
            public byte MainLabel { get; set; }
            public byte SubLabel { get; set; }
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

        public override int OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        int ret = footballDB.Execute("DELETE FROM prediction WHERE fixture_id = @FixtureId AND main_label = @MainLabel AND sub_label = @SubLabel; ",
                            new { _input.FixtureId, _input.MainLabel, _input.SubLabel });

                        if (ret == 1)
                            _output = 0;
                        else
                            _output = 1;
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