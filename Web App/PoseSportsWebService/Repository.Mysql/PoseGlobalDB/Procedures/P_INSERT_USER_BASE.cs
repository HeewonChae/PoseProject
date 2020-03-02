﻿using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Procedures
{
    public class P_INSERT_USER_BASE : MysqlQuery<P_INSERT_USER_BASE.Input, bool>
    {
        public struct Input
        {
            public string PlatformId;
            public short PlatformType;
            public DateTime InsertTime;
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

        public override bool OnQuery()
        {
            _output = false;

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.PoseGlobalDB pose_globalDB) =>
                    {
                        var isExist = pose_globalDB.ExecuteScalar<bool>("SELECT IF (EXISTS (SELECT * FROM user_base WHERE platform_id = @platform_id), 1, 0)",
                                                                                new { platform_id = _input.PlatformId });

                        if (isExist)
                        {
                            _output = true;
                        }
                        else
                        {
                            var affectedRows = pose_globalDB.ExecuteSQL("INSERT INTO user_base(platform_id, platform_type, last_login_date, ipt_date)" +
                                                                            "VALUE(@platform_id, @platform_type, @last_login_date, @ipt_date);",
                                                                            new { platform_id = _input.PlatformId, platform_type = _input.PlatformType, last_login_date = _input.InsertTime, ipt_date = _input.InsertTime });

                            if (affectedRows == 1)
                                _output = true;
                        }
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