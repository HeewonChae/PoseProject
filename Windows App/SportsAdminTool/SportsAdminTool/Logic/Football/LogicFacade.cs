using LogicCore.Utility;
using LogicCore.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ApiLogic = SportsAdminTool.Logic.WebAPI;
using FootballDB = Repository.Mysql.FootballDB;
using AppModel = SportsAdminTool.Model;
using ApiModel = RapidAPI.Models;
using System.Windows.Controls;
using SportsAdminTool.Model.Resource.Football;

namespace SportsAdminTool.Logic.Football
{
    public static class LogicFacade
    {
        /// <summary>
        /// Update country, league, team
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<bool> InitializeFootballDB(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                if (cancellationToken.IsCancellationRequested)
                    return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                // Update country
                {
                    mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, "Update countries");
                    var api_countries = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllCountries();
                    Database.FootballDBFacade.UpdateCountry(api_countries.ToArray());
                }

                if (cancellationToken.IsCancellationRequested)
                    return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                // Update league
                {
                    mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, "Update leagues");
                    var api_leagues = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllAvailableLeauges();
                    Database.FootballDBFacade.UpdateLeague(api_leagues.ToArray());
                    Database.FootballDBFacade.UpdateCoverage(api_leagues.ToArray());
                }

                if (cancellationToken.IsCancellationRequested)
                    return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                // Update team
                {
                    mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, "Update teams");
                    var onGoingLeauges = Database.FootballDBFacade.SelectLeagues(new FootballDB.Procedures.P_SELECT_LEAGUES.Input()
                    {
                        Where = "is_current = 1",
                    });

                    var leagueCnt = onGoingLeauges.Count();
                    int loop = 0;
                    foreach (var league in onGoingLeauges)
                    {
                        loop++;
                        mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, $"Update Teams ({loop}/{leagueCnt})");

                        if (cancellationToken.IsCancellationRequested)
                            return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                        var api_teams = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllTeamsByLeagueID(league.id);
                        Database.FootballDBFacade.UpdateTeam(league.id, api_teams.ToArray());
                    }
                }

                return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);
            });
        }

        /// <summary>
        /// update scheduled fixture, odds, standings
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<bool> UpdateScheduledFixtures(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                if (cancellationToken.IsCancellationRequested)
                    return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                // Update scheduled fixtrues
                {
                    mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, "Update scheduled fixtrues");

                    // Call API
                    var api_fixtures = Singleton.Get<ApiLogic.FootballWebAPI>().GetFixturesByDate(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

                    // 취소, 연기된 경기 필터링
                    var api_filteredFixture = api_fixtures.Where(elem =>
                         Singleton.Get<CheckValidation>().IsValidLeague((short)elem.LeagueID, elem.League.Name, elem.League.Country, true)
                         && Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status));

                    // 예측할때 사용할 데이터들 수집
                    int fixtureCnt = api_filteredFixture.Count();
                    int loop = 0;
                    foreach (var api_fixture in api_filteredFixture)
                    {
                        loop++;
                        mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update scheduled fixtrues ({loop}/{fixtureCnt})");

                        if (cancellationToken.IsCancellationRequested)
                            return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                        // Check exist in db
                        if (Database.FootballDBFacade.IsExistFixture(api_fixture.FixtureID))
                            continue;

                        // Updte home & away last fixtrues, H2H (컵 경기 포함)
                        var api_homeLastFixtures = Singleton.Get<ApiLogic.FootballWebAPI>().GetLastFixturesByTeamID((short)api_fixture.HomeTeam.TeamID, 10);
                        var filtered_homeLastFixture = api_homeLastFixtures.Where(elem => elem.FixtureID != api_fixture.FixtureID
                                                    && Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status));

                        var api_awayLastFixtures = Singleton.Get<ApiLogic.FootballWebAPI>().GetLastFixturesByTeamID((short)api_fixture.AwayTeam.TeamID, 10);
                        var filtered_awayLastFixture = api_awayLastFixtures.Where(elem => elem.FixtureID != api_fixture.FixtureID
                                                    && Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status));

                        var api_h2h = Singleton.Get<ApiLogic.FootballWebAPI>().GetH2HFixtures((short)api_fixture.HomeTeam.TeamID, (short)api_fixture.AwayTeam.TeamID);
                        var filtered_h2h = api_h2h.Where(elem => elem.EventDate < DateTime.Now.Date
                                            && elem.EventDate > new DateTime(DateTime.Now.AddYears(-3).Year, 1, 1)
                                            && elem.FixtureID != api_fixture.FixtureID
                                            && Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status));

                        // Merge api fixtures
                        var api_allFixtures = new HashSet<AppModel.Football.Fixture>(new Comparer.AppFootballFixtureComparer());
                        api_allFixtures.UnionWith(filtered_homeLastFixture);
                        api_allFixtures.UnionWith(filtered_awayLastFixture);
                        api_allFixtures.UnionWith(filtered_h2h);

                        // Update last fixture, fixture statistic
                        int lastFixturesCnt = api_allFixtures.Count();
                        int innerLoop = 0;
                        bool isLastFixturesUpdateSuccess = true;
                        foreach (var api_lastFixture in api_allFixtures)
                        {
                            if (cancellationToken.IsCancellationRequested)
                                return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                            innerLoop++;
                            mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update last fixtrues ({innerLoop}/{lastFixturesCnt})");

                            // Check exist in db
                            if (Database.FootballDBFacade.IsExistFixture(api_lastFixture.FixtureID))
                                continue;

                            var api_fixtureStatistic = Singleton.Get<ApiLogic.FootballWebAPI>().GetFixtureStatisticByFixtureID(api_lastFixture.FixtureID);
                            if (api_fixtureStatistic != null)
                            {
                                // Convert data for db model
                                DataConverter.CovertAppModelToDbModel(api_lastFixture.FixtureID, (short)api_lastFixture.HomeTeam.TeamID, (short)api_lastFixture.AwayTeam.TeamID, api_fixtureStatistic,
                                    out FootballDB.Tables.FixtureStatistic[] coverted_fixtureStatistics);

                                Database.FootballDBFacade.UpdateFixtureStatistics(coverted_fixtureStatistics);
                            }

                            // Call DB
                            isLastFixturesUpdateSuccess = isLastFixturesUpdateSuccess && Database.FootballDBFacade.UpdateFixture(true, true, api_lastFixture) > 0;
                        }

                        // Update standing
                        // grouping by leagueID
                        var api_groupingLastFixturesByLeague = api_allFixtures.GroupBy(elem => elem.LeagueID);
                        int groupintCnt = api_groupingLastFixturesByLeague.Count();
                        innerLoop = 0;
                        bool isStandingsUpdateSuccess = true;
                        foreach (var api_groupingLastFixtures in api_groupingLastFixturesByLeague)
                        {
                            if (cancellationToken.IsCancellationRequested)
                                return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                            innerLoop++;
                            mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update last league standings ({innerLoop}/{groupintCnt})");

                            var tempFirstFixture = api_groupingLastFixtures.First();

                            isStandingsUpdateSuccess = isStandingsUpdateSuccess && UpdateStandings((short)tempFirstFixture.LeagueID, tempFirstFixture.League.Name, tempFirstFixture.League.Country);
                        }

                        if (!isStandingsUpdateSuccess || !isLastFixturesUpdateSuccess)
                            continue;

                        // Call DB
                        Database.FootballDBFacade.UpdateFixture(false, false, api_fixture);
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                    return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                // Update standings
                bool standingsUpdateSuccess = true;
                {
                    mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, "Update standings");

                    // Call DB
                    var db_fixtures = Database.FootballDBFacade.SelectFixtures(new FootballDB.Procedures.P_SELECT_FIXTURES.Input()
                    {
                        Where = $"is_predicted = 0",
                    });

                    // grouping by league_id
                    var db_groupingFixtures = db_fixtures.GroupBy(elem => elem.league_id);

                    int groupCnt = db_groupingFixtures.Count();
                    int loop = 0;
                    foreach (var db_groupingFixture in db_groupingFixtures)
                    {
                        loop++;
                        mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update Standings ({loop}/{groupCnt})");

                        var db_leauge = Database.FootballDBFacade.SelectLeagues(new FootballDB.Procedures.P_SELECT_LEAGUES.Input()
                        {
                            Where = $"id = {db_groupingFixture.Key}",
                        }).FirstOrDefault();

                        standingsUpdateSuccess = UpdateStandings(db_groupingFixture.Key, db_leauge.name, db_leauge.country_name);
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                    return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB) && standingsUpdateSuccess;

                // Update odds
                bool oddsUpdateSuccess = true;
                {
                    mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, "Update odds");

                    // Call DB
                    var db_fixtures = Database.FootballDBFacade.SelectFixtures(new FootballDB.Procedures.P_SELECT_FIXTURES.Input()
                    {
                        Where = $"is_predicted = 0",
                    });

                    int fixtureCnt = db_fixtures.Count();
                    int loop = 0;
                    foreach (var db_fixture in db_fixtures)
                    {
                        loop++;
                        mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update odds ({loop}/{fixtureCnt})");

                        // Check already updated odds
                        if (Database.FootballDBFacade.IsAlreadyUpdatedOdds(db_fixture.id))
                            continue;

                        // Call API
                        var api_odds = Singleton.Get<ApiLogic.FootballWebAPI>().GetOddsByFixtureID(db_fixture.id);

                        // DB Save
                        int dbSaveResult = 0;
                        if (api_odds != null)
                            dbSaveResult = Database.FootballDBFacade.UpdateOdds(db_fixture.id, api_odds.Bookmakers);

                        if (api_odds != null && dbSaveResult == 0)
                            oddsUpdateSuccess = false;
                    }
                }

                return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB) && standingsUpdateSuccess && oddsUpdateSuccess;
            });
        }

        /// <summary>
        /// Predict fixture
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<bool> PredictFixtures(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                if (cancellationToken.IsCancellationRequested)
                    return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, "Predict fixtrues");

                var db_fixtures = Database.FootballDBFacade.SelectFixtures(new FootballDB.Procedures.P_SELECT_FIXTURES.Input()
                {
                    Where = $"is_predicted = 0",
                });

                int fixtureCnt = db_fixtures.Count();
                int loop = 0;
                foreach (var db_fixture in db_fixtures)
                {
                    loop++;
                    mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Predict fixtrues ({loop}/{fixtureCnt})");

                    if (cancellationToken.IsCancellationRequested)
                        return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                    db_fixture.is_predicted = true;

                    // DB Save
                    Database.FootballDBFacade.UpdateFixture(db_fixture);
                }

                return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);
            });
        }

        /// <summary>
        /// Check completed fixtures
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<bool> CheckCompletedFixtures(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                if (cancellationToken.IsCancellationRequested)
                    return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                mainWindow.Set_Lable(mainWindow._lbl_check_completed_fixtures, "Check completed fixtures");

                var db_fixtures = Database.FootballDBFacade.SelectFixtures(new FootballDB.Procedures.P_SELECT_FIXTURES.Input()
                {
                    Where = $"is_completed = 0 AND event_date < '{DateTime.UtcNow.ToString("yyyyMMddTHHmmss")}'",
                });

                int fixtureCnt = db_fixtures.Count();
                int loop = 0;
                foreach (var db_fixture in db_fixtures)
                {
                    loop++;
                    mainWindow.Set_Lable(mainWindow._lbl_check_completed_fixtures, $"Check completed fixtures ({loop}/{fixtureCnt})");

                    if (cancellationToken.IsCancellationRequested)
                        return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);

                    // Call API
                    var api_fixture = Singleton.Get<ApiLogic.FootballWebAPI>().GetFixturesByFixtureID(db_fixture.id);

                    if (!Singleton.Get<CheckValidation>().IsValidFixtureStatus(api_fixture.Status))
                    {
                        Database.FootballDBFacade.DeleteFixtures(db_fixture.id);
                        continue;
                    }

                    // 6시간이 지났는데 아직 경기가 안끝났으면 삭제
                    if (api_fixture.EventDate < DateTime.UtcNow.AddHours(-6)
                        && (api_fixture.Status != ApiModel.Football.Enums.FixtureStatusType.FT // 종료
                        && api_fixture.Status != ApiModel.Football.Enums.FixtureStatusType.AET // 연장 후 종료
                        && api_fixture.Status != ApiModel.Football.Enums.FixtureStatusType.PEN)) // 승부차기 후 종료
                    {
                        Database.FootballDBFacade.DeleteFixtures(db_fixture.id);
                        continue;
                    }

                    // Update statistics
                    if (api_fixture.Statistic != null)
                    {
                        // Convert data for db model
                        DataConverter.CovertAppModelToDbModel(api_fixture.FixtureID, (short)api_fixture.HomeTeam.TeamID, (short)api_fixture.AwayTeam.TeamID, api_fixture.Statistic,
                            out FootballDB.Tables.FixtureStatistic[] coverted_fixtureStatistics);

                        Database.FootballDBFacade.UpdateFixtureStatistics(coverted_fixtureStatistics);
                    }

                    // DB Save
                    db_fixture.is_completed = true;
                    Database.FootballDBFacade.UpdateFixture(db_fixture);
                }

                return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);
            });
        }

        public static bool UpdateStandings(short leagueID, string leagueName, string countryName)
        {
            // Check already updated standings
            if (Database.FootballDBFacade.IsAlreadyUpdatedStandings(leagueID))
                return true;

            // Check league type
            var api_league = Database.FootballDBFacade.SelectLeagues(new FootballDB.Procedures.P_SELECT_LEAGUES.Input()
            {
                Where = $"id = {leagueID}",
            }).FirstOrDefault();

            if (api_league?.type.Equals("Cup") ?? false)
                return true;

            // Call API
            var api_standings = Singleton.Get<ApiLogic.FootballWebAPI>().GetStandingsByLeagueID(leagueID);

            // Check Standings validation
            if (!Singleton.Get<CheckValidation>().IsValidStandings(api_standings, leagueID, leagueName, countryName, true))
                return false;

            if (api_standings.Count == 0)
                return true;

            // Call DB
            return Database.FootballDBFacade.UpdateStanding(leagueID, countryName, api_standings.ToArray()) > 0;
        }

        /// <summary>
        /// Solve Errors
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<bool> SolveErrors(CancellationToken cancellationToken, Label lbl)
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                if (cancellationToken.IsCancellationRequested)
                    return false;

                mainWindow.Set_Lable(lbl, "Solve Errors");

                var leagueErrors = Singleton.Get<CheckValidation>().GetErrorLeauges(InvalidType.NotExistInDB, false);
                int errorLeagueCnt = leagueErrors.Length;
                int loop = 0;
                foreach (var errorLeague in leagueErrors)
                {
                    loop++;
                    mainWindow.Set_Lable(lbl, $"Solve League Errors ({loop}/{errorLeagueCnt})");

                    if (cancellationToken.IsCancellationRequested)
                        return false;

                    // Call API
                    var api_league = Singleton.Get<ApiLogic.FootballWebAPI>().GetLeagueByLeagueID(errorLeague.LeagueID);

                    // DB Save
                    Database.FootballDBFacade.UpdateLeague(api_league); // league
                    Database.FootballDBFacade.UpdateCoverage(api_league); // coverage

                    var api_teams = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllTeamsByLeagueID((short)api_league.LeagueID);
                    Database.FootballDBFacade.UpdateTeam((short)api_league.LeagueID, api_teams.ToArray());
                }

                if (cancellationToken.IsCancellationRequested)
                    return false;

                var teamErrors = Singleton.Get<CheckValidation>().GetErrorTeams(InvalidType.NotExistInDB, false);
                int errorTeamCnt = teamErrors.Length;
                loop = 0;
                foreach (var errorTeam in teamErrors)
                {
                    loop++;
                    mainWindow.Set_Lable(lbl, $"Solve Team Errors ({loop}/{errorTeamCnt})");

                    if (cancellationToken.IsCancellationRequested)
                        return false;

                    // Call API
                    var api_team = Singleton.Get<ApiLogic.FootballWebAPI>().GetTeamByTeamID(errorTeam.TeamsID);

                    // DB Save
                    if (api_team != null)
                        Database.FootballDBFacade.UpdateTeam(errorTeam.LeagueID, api_team);
                }

                return true;
            });
        }
    }
}