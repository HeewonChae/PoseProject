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

    [WebModelType(OutputType = typeof(O_GET_MATCH_VIP))]
    public static class P_GET_MATCH_VIP
    {
        public static class RowCode
        {
            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_MATCH_VIP + 1;
        }

        public static O_GET_MATCH_VIP Execute()
        {
            IEnumerable<FootballDB.OutputModels.DB_FootballVIPFixtureDetail> db_output;
            using (var P_SELECT_VP_FIXTURES = new FootballDB.Procedures.P_SELECT_VP_FIXTURES())
            {
                P_SELECT_VP_FIXTURES.SetInput(new FootballDB.Procedures.P_SELECT_VP_FIXTURES.Input
                {
                    IsSelectHistory = false
                });

                db_output = P_SELECT_VP_FIXTURES.OnQuery();

                if (P_SELECT_VP_FIXTURES.EntityStatus != null || db_output == null)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error);
            }

            var vipFixtureDetails = new List<PacketModels.FootballVIPFixtureDetail>();
            foreach (var db_vipFixture in db_output)
            {
                var vipFixtureDetail = Singleton.Get<FootballVIPFixtureDetailConverter>().Convert(db_vipFixture);
                vipFixtureDetails.Add(vipFixtureDetail);
            }

            return new O_GET_MATCH_VIP
            {
                VIPFixtureDetails = vipFixtureDetails.ToArray(),
            };
        }
    }
}