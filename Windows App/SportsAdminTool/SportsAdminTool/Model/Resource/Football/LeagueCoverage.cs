using GameKernel;
using LogicCore.Debug;
using LogicCore.File;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SportsAdminTool.Model.Resource.Football.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Model.Resource.Football
{
	public class LeagueCoverage : IRecord, TableParser.IPostLoading
	{
		public readonly int Index = 0; // 리그 인덱스랑 상관없는 그냥 테이블 인덱스
		public readonly string LeagueName = string.Empty;
		[JsonConverter(typeof(StringEnumConverter))]
		public readonly LeagueType LeagueType = LeagueType._NONE_;
		public readonly string Country = string.Empty;
		public readonly bool IsCoverage = false;

		public LeagueCoverage() { }

		public LeagueCoverage(int index, string leaugeName, LeagueType leagueType, string country, bool isCoverage)
		{
			Index = index;
			LeagueName = leaugeName;
			LeagueType = leagueType;
			Country = country;
			IsCoverage = isCoverage;
		}

		public static  Dictionary<string, LeagueCoverage> Dic_leagueCoverage { get; } = new Dictionary<string, LeagueCoverage>();

		void TableParser.IPostLoading.Process()
		{
			string key = MakeLeagueCoverageKey(this.LeagueName, this.Country);

			if (Dic_leagueCoverage.ContainsKey(key))
				throw new Exception($"Alread exist key in Dic_leagueCoverage key: {key}");

			Dic_leagueCoverage.Add(key, this);
		}

		public static string MakeLeagueCoverageKey(string leaugeName, string country)
		{
			return $"{leaugeName}:{country}";
		}
	}
}
