using GameKernel;
using LogicCore.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Model.Resource.Football
{
	public class UndefinedTeam : IRecord, TableParser.IPostLoading
	{
		public readonly int Index = 0; // 팀 인덱스랑 상관없는 그냥 테이블 인덱스
		public readonly short LeagueID = 0;
		public readonly string TeamName = string.Empty;
		public readonly string CountryName = string.Empty;
		public readonly short ConvertTeamID = 0;

		private static Dictionary<string, UndefinedTeam> Dic_undefinedTeam { get; } = new Dictionary<string, UndefinedTeam>();

		void TableParser.IPostLoading.Process()
		{
			Dic_undefinedTeam.Add(MakeTeamConvertKey(this.CountryName, this.LeagueID, this.TeamName), this);
		}

		private static string MakeTeamConvertKey(string CountryName, short LeagueID, string TeamName)
		{
			return $"{CountryName}:{LeagueID}:{TeamName}";
		}

		public static bool TryConvertTeamID(string CountryName, short LeagueID, string TeamName, out short convertedTeamID)
		{
			convertedTeamID = 0;

			var key = MakeTeamConvertKey(CountryName, LeagueID, TeamName);
			if (!Dic_undefinedTeam.ContainsKey(key))
				return false;

			convertedTeamID = Dic_undefinedTeam[key].ConvertTeamID;

			return true;
		}
	}
}