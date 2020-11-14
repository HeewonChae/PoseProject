using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Commands.Football
{
    using FootballDB = Repository.Mysql.FootballDB;
    using PacketModels = PosePacket.Service.Football.Models;
    using PosePacket.Service.Football;
    using SportsWebService.Infrastructure;
    using System.ComponentModel;
    using SportsWebService.Services;
    using SportsWebService.Logics;
    using LogicCore.Utility;
    using SportsWebService.Logics.Converters;
    using PosePacket.Service.Football.Models.Enums;

    [WebModelType(InputType = typeof(I_DELETE_VIP_PICK), OutputType = typeof(O_DELETE_VIP_PICK))]
    public static class P_DELETE_VIP_PICK
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_DELETE_VIP_PICK + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_DELETE_VIP_PICK + 1;

            [Description("Not Found Pick")]
            public const int DB_Not_Found_Pick = ServiceErrorCode.StoredProcedure_Football.P_DELETE_VIP_PICK + 2;
        }

        public static O_DELETE_VIP_PICK Execute(I_DELETE_VIP_PICK input)
        {
            if (input == null
                || input.FixtureId == 0
                || input.MainLabel == FootballPredictionType._NONE_
                || input.MainLabel == FootballPredictionType._MAX_)
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            using (var P_DELETE_VIP_PICK = new FootballDB.Procedures.P_DELETE_VIP_PICK())
            {
                P_DELETE_VIP_PICK.SetInput(new FootballDB.Procedures.P_DELETE_VIP_PICK.Input
                {
                    FixtureId = input.FixtureId,
                    MainLabel = (byte)input.MainLabel,
                    SubLabel = input.SubLabel,
                });

                int db_output = P_DELETE_VIP_PICK.OnQuery();

                if (P_DELETE_VIP_PICK.EntityStatus != null || db_output != 0)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error + db_output);
            }

            return new O_DELETE_VIP_PICK
            {
                IsSuccess = true,
            };
        }
    }
}