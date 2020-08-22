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
        public static db_table.Fixture[] Execute(DateTime startDate)
        {
            return FootballDBFacade.SelectFixtures(where: $"{nameof(db_table.Fixture.is_predicted)} = 1 " +
                $"AND {nameof(db_table.Fixture.match_time)} >= \'{startDate.ToString("yyyyMMdd")}\' " +
                $"AND {nameof(db_table.Fixture.is_completed)} = 1 ").ToArray();
        }
    }
}