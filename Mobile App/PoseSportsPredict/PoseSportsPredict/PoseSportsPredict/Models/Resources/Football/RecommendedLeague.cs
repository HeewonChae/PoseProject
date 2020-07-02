using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Football;
using Shiny;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PoseSportsPredict.Models.Resources.Football
{
    public class RecommendedLeague
    {
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FootballLeagueType LeagueType { get; set; }

        public string CountryName { get; set; }
        public string CountryLogo { get; set; }

        public static Dictionary<string, FootballLeagueInfo> RecommendedLeagues { get; } = new Dictionary<string, FootballLeagueInfo>();

        public static void Load(params RecommendedLeague[] recommendedLeagues)
        {
            foreach (var recommendedLeague in recommendedLeagues)
            {
                string key = MakeCoverageLeagueKey(recommendedLeague.CountryName, recommendedLeague.LeagueName);

                Debug.Assert(!RecommendedLeagues.ContainsKey(key), $"Alread exist key in Dic_RecommendedLeagues key: {key}");

                // 기본 이미지 설정
                if (string.IsNullOrEmpty(recommendedLeague.CountryLogo))
                    recommendedLeague.CountryLogo = "img_world.png";
                if (string.IsNullOrEmpty(recommendedLeague.LeagueLogo))
                    recommendedLeague.LeagueLogo = recommendedLeague.CountryLogo;

                var leagueInfo = new FootballLeagueInfo
                {
                    CountryName = recommendedLeague.CountryName,
                    CountryLogo = recommendedLeague.CountryLogo,
                    LeagueName = recommendedLeague.LeagueName,
                    LeagueLogo = recommendedLeague.LeagueLogo,
                    LeagueType = recommendedLeague.LeagueType,
                    IsBookmarked = false,
                }; ;

                RecommendedLeagues.Add(key, leagueInfo);
            }
        }

        public static string MakeCoverageLeagueKey(string CountryName, string leaugeName)
        {
            return $"{CountryName}:{leaugeName}";
        }

        public static FootballLeagueInfo FindLeauge(string CountryName, string leaugeName)
        {
            var key = MakeCoverageLeagueKey(CountryName, leaugeName);

            if (RecommendedLeagues.ContainsKey(key))
            {
                return RecommendedLeagues[key];
            }

            return null;
        }

        public static bool HasLeague(string CountryName, string leaugeName)
        {
            var key = MakeCoverageLeagueKey(CountryName, leaugeName);

            return RecommendedLeagues.ContainsKey(key);
        }
    }
}