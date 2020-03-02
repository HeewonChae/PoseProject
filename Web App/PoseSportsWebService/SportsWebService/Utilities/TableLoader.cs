using GameKernel;
using LogicCore.File;
using SportsWebService.App_Config;
using System.IO;

namespace SportsWebService.Utilities
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
                            .JsonDeserialize<ErrorCodeDescription[]>();

            foreach (var error in errors)
            {
                ErrorCodeDescription.Errors.Add(error.ErrorCode, error.Description);
            }
        }
    }
}