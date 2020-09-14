using SportsAdminTool.Logic.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictionBackTesting.Commands
{
    using db_table = Repository.Mysql.FootballDB.Tables;

    public static class SelecltPredictedFixtures
    {
        public static db_table.Fixture[] Execute(DateTime startDate, DateTime endData)
        {
            return FootballDBFacade.SelectCoverageFixtures(where: $"f.{nameof(db_table.Fixture.is_completed)} = 1 " +
                $"AND f.{nameof(db_table.Fixture.match_time)} BETWEEN \'{startDate:yyyyMMdd}\' AND \'{endData.AddDays(1):yyyyMMdd}\' " +
                $"AND lc.{nameof(db_table.LeagueCoverage.predictions)} = 1 ").ToArray();
        }
    }
}