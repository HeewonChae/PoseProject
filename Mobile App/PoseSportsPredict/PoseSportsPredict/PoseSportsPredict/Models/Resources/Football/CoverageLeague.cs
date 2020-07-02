using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Football;
using Shiny;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.Models.Resources.Football
{
    public class CoverageLeague
    {
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FootballLeagueType LeagueType { get; set; }

        public string CountryName { get; set; }
        public string CountryLogo { get; set; }

        public static Dictionary<string, FootballLeagueInfo> CoverageLeagues { get; } = new Dictionary<string, FootballLeagueInfo>();

        public static void Load(params CoverageLeague[] coverageLeagues)
        {
            foreach (var coverageLeague in coverageLeagues)
            {
                string key = MakeCoverageLeagueKey(coverageLeague.CountryName, coverageLeague.LeagueName);

                Debug.Assert(!CoverageLeagues.ContainsKey(key), $"Alread exist key in Dic_CoverageLeagues key: {key}");

                // 기본 이미지 설정
                if (string.IsNullOrEmpty(coverageLeague.CountryLogo))
                    coverageLeague.CountryLogo = "img_world.png";
                if (string.IsNullOrEmpty(coverageLeague.LeagueLogo))
                    coverageLeague.LeagueLogo = coverageLeague.CountryLogo;

                var leagueInfo = new FootballLeagueInfo
                {
                    CountryName = coverageLeague.CountryName,
                    CountryLogo = coverageLeague.CountryLogo,
                    LeagueName = coverageLeague.LeagueName,
                    LeagueLogo = coverageLeague.LeagueLogo,
                    LeagueType = coverageLeague.LeagueType,
                    IsBookmarked = false,
                };

                CoverageLeagues.Add(key, leagueInfo);
            }
        }

        public static string MakeCoverageLeagueKey(string CountryName, string leaugeName)
        {
            return $"{CountryName}:{leaugeName}";
        }

        public static FootballLeagueInfo FindLeauge(string CountryName, string leaugeName)
        {
            var key = MakeCoverageLeagueKey(CountryName, leaugeName);

            if (CoverageLeagues.ContainsKey(key))
            {
                return CoverageLeagues[key];
            }

            return null;
        }

        public static bool HasLeague(string CountryName, string leaugeName)
        {
            var key = MakeCoverageLeagueKey(CountryName, leaugeName);

            return CoverageLeagues.ContainsKey(key);
        }
    }
}