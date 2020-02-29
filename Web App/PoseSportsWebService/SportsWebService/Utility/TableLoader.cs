using GameKernel;
using LogicCore.File;
using SportsWebService.App_Config;

namespace SportsWebService.Utility
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
			Load_ErrorCodeDescription(rootPath);
		}

		private static void Load_ErrorCodeDescription(string rootPath)
		{
			_parser.TryLoadJsonTable(
				"ErrorCodeDescriptions",
				"Items",
				typeof(ErrorCodeDescription),
				FileFacade.FileToByte($"{rootPath}\\ErrorCodeDescription.json"),
				indexField: "ErrorCode");

			var table = GameDatabase.FindTable("ErrorCodeDescriptions").GetRecords<ErrorCodeDescription>();
		}
	}
}