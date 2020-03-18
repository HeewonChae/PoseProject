using LogicCore.Converter;
using LogicCore.Debug;
using Newtonsoft.Json;
using SportsAdminTool.Model.Football.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootballDB = Repository.Mysql.FootballDB;
using footballRes = SportsAdminTool.Model.Resource.Football;

namespace SportsAdminTool.Commands.Football
{
    public static class ExportCoverageLeagues
    {
        private readonly static string FilePath = ".\\ExportFiles\\";
        private readonly static string FileName = "CoverageLeagues.json";

        public static void Execute()
        {
            IEnumerable<FootballDB.Procedures.P_SELECT_COVERAGE_LEAGUES.Output> db_coverageLeagues;
            using (var P_SELECT_COVERAGE_LEAGUES = new FootballDB.Procedures.P_SELECT_COVERAGE_LEAGUES())
            {
                db_coverageLeagues = P_SELECT_COVERAGE_LEAGUES.OnQuery();

                Dev.Assert(P_SELECT_COVERAGE_LEAGUES.EntityStatus == null);
                Dev.Assert(db_coverageLeagues.Count() != 0);

                List<footballRes.CoverageLeague> coverageLeagueList = new List<footballRes.CoverageLeague>();

                int increaseIndex = 1;
                foreach (var coverageLeague in db_coverageLeagues)
                {
                    Dev.Assert(coverageLeague.LeagueType.TryParseEnum(out FootballLeagueType leagueType));

                    coverageLeagueList.Add(new footballRes.CoverageLeague(
                        increaseIndex,
                        coverageLeague.LeagueName,
                        coverageLeague.LeagueLogo,
                        leagueType,
                        coverageLeague.CountryName,
                        coverageLeague.CountryLogo
                        ));
                }

                coverageLeagueList = coverageLeagueList.OrderBy(elem => elem.CountryName).ToList();

                // Create Json File
                Directory.CreateDirectory(FilePath);

                using (FileStream stream = new FileStream($"{FilePath}{FileName}", FileMode.Create))
                {
                    using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(stream)))
                    {
                        JsonSerializer serial = new JsonSerializer();
                        serial.Serialize(writer, coverageLeagueList);
                    }
                }
            }
        }
    }
}