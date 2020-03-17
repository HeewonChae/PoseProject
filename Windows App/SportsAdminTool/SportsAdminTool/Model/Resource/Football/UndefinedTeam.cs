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
        public readonly short LeagueId = 0;
        public readonly string TeamName = string.Empty;
        public readonly string CountryName = string.Empty;
        public readonly short ConvertTeamId = 0;
        public readonly string ConvertTeamName = string.Empty;

        private static Dictionary<string, UndefinedTeam> Dic_undefinedTeam { get; } = new Dictionary<string, UndefinedTeam>();

        void TableParser.IPostLoading.Process()
        {
            Dic_undefinedTeam.Add(MakeTeamConvertKey(this.CountryName, this.LeagueId, this.TeamName), this);
        }

        private static string MakeTeamConvertKey(string CountryName, short LeagueId, string TeamName)
        {
            return $"{CountryName}:{LeagueId}:{TeamName}";
        }

        public static bool TryConvertTeamId(string CountryName, short LeagueId, string TeamName, out short convertedTeamId, out string convertedTeamName)
        {
            convertedTeamId = 0;
            convertedTeamName = string.Empty;

            var key = MakeTeamConvertKey(CountryName, LeagueId, TeamName);
            if (!Dic_undefinedTeam.ContainsKey(key))
                return false;

            convertedTeamId = Dic_undefinedTeam[key].ConvertTeamId;
            convertedTeamName = Dic_undefinedTeam[key].ConvertTeamName;
            return true;
        }
    }
}