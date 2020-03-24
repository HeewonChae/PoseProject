using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Football;
using Shiny;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.Models.Resources.Football
{
    public class CoverageLeague
    {
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public FootballLeagueType LeagueType { get; set; }
        public string CountryName { get; set; }
        public string CountryLogo { get; set; }

        public static Dictionary<string, FootballLeagueInfo> CoverageLeagues { get; } = new Dictionary<string, FootballLeagueInfo>();

        public static async Task Load(params CoverageLeague[] coverageLeagues)
        {
            var bookmarkedLeagues = await ShinyHost.Resolve<ISQLiteService>().SelectAllAsync<FootballLeagueInfo>();

            foreach (var covaerageLeague in coverageLeagues)
            {
                string key = MakeCoverageLeagueKey(covaerageLeague.CountryName, covaerageLeague.LeagueName, covaerageLeague.LeagueType.ToString());

                Debug.Assert(!CoverageLeagues.ContainsKey(key), $"Alread exist key in Dic_leagueCoverage key: {key}");

                var leagueInfo = ShinyHost.Resolve<CoverageLeagueToLeagueInfoConverter>().Convert(
                    covaerageLeague, typeof(FootballLeagueInfo), null, CultureInfo.CurrentUICulture) as FootballLeagueInfo;

                var bookmarkedLeauge = bookmarkedLeagues.Find(elem => elem.PrimaryKey == leagueInfo.PrimaryKey);
                if (bookmarkedLeauge != null)
                {
                    leagueInfo = bookmarkedLeauge;
                }

                CoverageLeagues.Add(key, leagueInfo);
            }
        }

        public static string MakeCoverageLeagueKey(string CountryName, string leaugeName, string leagueType)
        {
            return $"{CountryName}:{leaugeName}:{leagueType}";
        }

        public static FootballLeagueInfo FindLeauge(string CountryName, string leaugeName, string leagueType)
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