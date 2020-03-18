using GameKernel;
using LogicCore.Debug;
using LogicCore.File;
using SportsAdminTool.Model.Resource.Football;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
            Load_Football_CoverageLeagues(rootPath);
            Load_Football_UndefinedTeams(rootPath);
        }

        private static void Load_Football_CoverageLeagues(string rootPath)
        {
            var CoverageLeagues = FileFacade.ReadAllText(Path.Combine($"{rootPath}\\Football\\", "CoverageLeagues.json"))
                            .JsonDeserialize<CoverageLeague[]>();

            Dev.Assert(CoverageLeagues.Count() != 0);

            CoverageLeague.Load(CoverageLeagues);
        }

        private static void Load_Football_UndefinedTeams(string rootPath)
        {
            _parser.TryLoadJsonTable(
                "Football_UndefinedTeam",
                "Teams",
                typeof(UndefinedTeam),
                FileFacade.FileToByte($"{rootPath}\\Football\\UndefinedTeams.json"));

            var table = GameDatabase.FindTable("Football_UndefinedTeam").GetRecords<UndefinedTeam>();
            Dev.Assert(table.Count() != 0);
        }
    }
}