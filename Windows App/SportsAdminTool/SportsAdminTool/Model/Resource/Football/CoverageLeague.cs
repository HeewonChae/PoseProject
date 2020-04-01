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
    public class CoverageLeague
    {
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FootballLeagueType LeagueType { get; set; }

        public string CountryName { get; set; }
        public string CountryLogo { get; set; }

        public CoverageLeague()
        {
        }

        public CoverageLeague(string leaugeName, string leagueLogo,
            FootballLeagueType leagueType, string countryName, string countryLogo)
        {
            LeagueName = leaugeName;
            LeagueLogo = leagueLogo;
            LeagueType = leagueType;
            CountryName = countryName;
            CountryLogo = countryLogo;
        }

        public static Dictionary<string, CoverageLeague> CoverageLeagues { get; } = new Dictionary<string, CoverageLeague>();

        public static void Load(params CoverageLeague[] coverageLeagues)
        {
            foreach (var covaerageLeague in coverageLeagues)
            {
                string key = MakeCoverageLeagueKey(covaerageLeague.CountryName, covaerageLeague.LeagueName, covaerageLeague.LeagueType.ToString());

                Dev.Assert(!CoverageLeagues.ContainsKey(key), $"Alread exist key in Dic_leagueCoverage key: {key}");

                CoverageLeagues.Add(key, covaerageLeague);
            }
        }

        public static string MakeCoverageLeagueKey(string CountryName, string leaugeName, string leagueType)
        {
            return $"{CountryName}:{leaugeName}:{leagueType}";
        }

        public static CoverageLeague FindLeauge(string CountryName, string leaugeName, string leagueType)
        {
            var key = MakeCoverageLeagueKey(CountryName, leaugeName, leagueType);

            if (CoverageLeagues.ContainsKey(key))
            {
                return CoverageLeagues[key];
            }

            return null;
        }

        public static bool HasLeague(string CountryName, string leaugeName, string leagueType)
        {
            var key = MakeCoverageLeagueKey(CountryName, leaugeName, leagueType);

            return CoverageLeagues.ContainsKey(key);
        }
    }
}