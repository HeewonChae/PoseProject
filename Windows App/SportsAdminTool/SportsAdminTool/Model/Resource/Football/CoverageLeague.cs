using GameKernel;
using LogicCore.Debug;
using LogicCore.File;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SportsAdminTool.Model.Football.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Model.Resource.Football
{
    public class CoverageLeague : IRecord, TableParser.IPostLoading
    {
        public readonly int Index = 0;
        public readonly short LeagueId = 0;
        public readonly string LeagueName = string.Empty;
        public readonly string LeagueLogo = string.Empty;

        [JsonConverter(typeof(StringEnumConverter))]
        public readonly FootballLeagueType LeagueType = FootballLeagueType._NONE_;

        public readonly string CountryName = string.Empty;
        public readonly string CountryLogo = string.Empty;

        public CoverageLeague()
        {
        }

        public CoverageLeague(int index, short leagueId, string leaugeName, string leagueLogo,
            FootballLeagueType leagueType, string countryName, string countryLogo)
        {
            Index = index;
            LeagueId = leagueId;
            LeagueName = leaugeName;
            LeagueLogo = leagueLogo;
            LeagueType = leagueType;
            CountryName = countryName;
            CountryLogo = countryLogo;
        }

        public static Dictionary<string, CoverageLeague> CoverageLeagues { get; } = new Dictionary<string, CoverageLeague>();

        void TableParser.IPostLoading.Process()
        {
            string key = MakeCoverageLeagueKey(this.CountryName, this.LeagueName);

            Dev.Assert(!CoverageLeagues.ContainsKey(key), $"Alread exist key in Dic_leagueCoverage key: {key}");

            CoverageLeagues.Add(key, this);
        }

        public static string MakeCoverageLeagueKey(string CountryName, string leaugeName)
        {
            return $"{CountryName}:{leaugeName}";
        }

        public static CoverageLeague FindLeauge(string CountryName, string leaugeName)
        {
            var key = MakeCoverageLeagueKey(CountryName, leaugeName);

            if (CoverageLeagues.ContainsKey(key))
            {
                return CoverageLeagues[key];
            }

            return null;
        }
    }
}