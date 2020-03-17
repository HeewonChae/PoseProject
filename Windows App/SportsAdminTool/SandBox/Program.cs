using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootballDB = Repository.Mysql.FootballDB;
using ResourceModel = SportsAdminTool.Model.Resource;
using AppModel = SportsAdminTool.Model;
using AppLogic = SportsAdminTool.Logic;
using LogicCore.File;

namespace SandBox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Prepare logical initialize
            {
                Repository.RepositoryStatic.Init_Mysql();
                // Football Data Mapping API Model to App Model
                AppModel.Football.Mapper.FootballMapper.Mapping();
                // Load Table
                string tableRootPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                AppLogic.TableLoader.Init(tableRootPath);
            }

            //MakeJsonFile();
        }

        //public static void MakeJsonFile()
        //{
        //    var Leagues = new List<ResourceModel.Football.LeagueCoverage>();

        //    // LeagueInfo to JsonFile
        //    using (var P_SELECT_LEAGUES = new FootballDB.Procedures.P_SELECT_LEAGUES())
        //    {
        //        var input = new FootballDB.Procedures.P_SELECT_LEAGUES.Input()
        //        {
        //            GroupBy = "country_name, type, name",
        //            OrderBy = "country_name asc, type desc, name asc",
        //        };

        //        P_SELECT_LEAGUES.SetInput(input);
        //        var dbLeagues = P_SELECT_LEAGUES.OnQuery();

        //        int index = 1;
        //        foreach (var dbLeague in dbLeagues)
        //        {
        //            Leagues.Add(new ResourceModel.Football.LeagueCoverage
        //                (index
        //                , dbLeague.name
        //                , (ResourceModel.Football.Enums.LeagueType)Enum.Parse(typeof(ResourceModel.Football.Enums.LeagueType), dbLeague.type)
        //                , dbLeague.country_name
        //                , false));

        //            index++;
        //        }
        //    }

        //    var serializeString = JsonConvert.SerializeObject(Leagues, Formatting.Indented);
        //    FileFacade.MakeSimpleTextFile("C:\\Users\\korma\\Desktop", "leauges.json", serializeString);
        //}
    }
}