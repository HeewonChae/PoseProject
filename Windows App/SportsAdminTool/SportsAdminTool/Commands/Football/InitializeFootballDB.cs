using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiLogic = SportsAdminTool.Logic.WebAPI;

namespace SportsAdminTool.Commands.Football
{
    public static class InitializeFootballDB
    {
        public static Task<bool> Execute()
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                // Update all country
                mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, "Update countries");
                var api_countries = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllCountries();
                Logic.Database.FootballDBFacade.UpdateCountry(api_countries.ToArray());

                // Update all league
                mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, "Update leagues");
                var api_leagues = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllAvailableLeauges();
                Logic.Database.FootballDBFacade.UpdateLeague(api_leagues.ToArray());
                Logic.Database.FootballDBFacade.UpdateCoverage(api_leagues.ToArray());

                // Update all team
                mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, "Update teams");
                var allLeagues = Logic.Database.FootballDBFacade.SelectLeagues();

                var leagueCnt = allLeagues.Count();
                int loop = 0;
                foreach (var league in allLeagues)
                {
                    loop++;
                    mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, $"Update Teams ({loop}/{leagueCnt})");

                    var api_teams = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllTeamsByLeagueId(league.id);
                    Logic.Database.FootballDBFacade.UpdateTeam(league.id, api_teams.ToArray());
                }

                return true;
            });
        }
    }
}