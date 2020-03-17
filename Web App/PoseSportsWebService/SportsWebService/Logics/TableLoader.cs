using GameKernel;
using LogicCore.File;
using SportsWebService.App_Config;
using SportsWebService.Utilities;
using System.Collections.Generic;
using System.IO;

namespace SportsWebService.Logics
{
    public static class TableLoader
    {
        private readonly static TableParser _parser = new TableParser();

        public static void Init(string rootPath)
        {
            LoadTable(rootPath);
        }

        private static void LoadTable(string rootPath)
        {
            Load_ErrorCodeDescription(rootPath);
        }

        private static void Load_ErrorCodeDescription(string rootPath)
        {
            var errors = FileFacade.ReadAllText(Path.Combine(rootPath, "ErrorCodeDescription.json"))
                            .JsonDeserialize<Dictionary<int, string>>();

            ErrorCodeDescription.Errors = errors;
        }
    }
}