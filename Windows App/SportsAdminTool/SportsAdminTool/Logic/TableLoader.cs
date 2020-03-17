using GameKernel;
using LogicCore.Debug;
using LogicCore.File;
using SportsAdminTool.Model.Resource.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Logic
{
    public static class TableLoader
    {
        private readonly static TableParser _parser = new TableParser();

        public static void Init(string rootPath)
        {
            LoadTable(rootPath);

            _parser.PostLoadingTables();

            _parser.PrintErrorMessage();
        }

        private static void LoadTable(string rootPath)
        {
            // Football
            Load_Football_LeagueCoverage(rootPath);
            Load_Football_UndefinedTeam(rootPath);
        }

        private static void Load_Football_LeagueCoverage(string rootPath)
        {
            //_parser.TryLoadJsonTable(
            //    "Football_LeagueCoverage",
            //    "Leagues",
            //    typeof(LeagueCoverage),
            //    FileFacade.FileToByte($"{rootPath}\\Football\\LeagueCoverage.json"));

            //var table = GameDatabase.FindTable("Football_LeagueCoverage").GetRecords<LeagueCoverage>();
            //Dev.Assert(table.Count() != 0);
        }

        private static void Load_Football_UndefinedTeam(string rootPath)
        {
            _parser.TryLoadJsonTable(
                "Football_UndefinedTeam",
                "Teams",
                typeof(UndefinedTeam),
                FileFacade.FileToByte($"{rootPath}\\Football\\UndefinedTeam.json"));

            var table = GameDatabase.FindTable("Football_UndefinedTeam").GetRecords<UndefinedTeam>();
            Dev.Assert(table.Count() != 0);
        }
    }
}