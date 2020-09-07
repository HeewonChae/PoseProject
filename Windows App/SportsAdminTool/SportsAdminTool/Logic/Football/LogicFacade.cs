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
using LogicCore.Debug;
using RapidAPI.Models.Football.Enums;
using SportsAdminTool.Model.Football;
using LogicCore.Converter;
using PosePacket.Service.Football.Models.Enums;
using PosePacket.Service.Enum;

using SportsAdminTool.Logic.WebAPI;

namespace SportsAdminTool.Logic.Football
{
    public static class LogicFacade
    {
        public static void UpdateAllFixturesByLeague(short leagueId)
        {
            // Call API
            var api_fixtures = Singleton.Get<ApiLogic.FootballWebAPI>().GetFixturesByLeagueId(leagueId);
            var api_filteredFixtures = api_fixtures.Where(elem => Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status, elem.MatchTime));
            var api_deleteFixtures = api_fixtures.Except(api_filteredFixtures);
            //var api_odds = Singleton.Get<ApiLogic.FootballWebAPI>().GetOddsByLeagueIdAndLabelId(leagueId, (int)OddsLabelType.Match_Winner);

            // DB Save
            Database.FootballDBFacade.DeleteFixtures(api_deleteFixtures.ToArray());
            Database.FootballDBFacade.UpdateFixture(api_filteredFixtures.ToArray());
            //Database.FootballDBFacade.UpdateOdds(api_odds.ToArray());
        }

        public static int UpdateOdds(int fixtureId)
        {
            // Call API
            var api_odds = Singleton.Get<ApiLogic.FootballWebAPI>().GetOddsByFixtureId(fixtureId);
            if (api_odds == null)
                return 0;

            // DB Save
            Database.FootballDBFacade.UpdateOdds(fixtureId, api_odds.Bookmakers);

            return api_odds.Bookmakers.Length;
        }

        /// <summary>
        /// 예측 데이터 적중 판별
        /// </summary>
        /// <param name="fixtureId"></param>
        public static void DiscernPrediction(Fixture api_fixture, bool isNotify = true)
        {
            var db_predictions = Database.FootballDBFacade.SelectPredictions(where: $"fixture_id = {api_fixture.FixtureId}");
            foreach (var db_prediction in db_predictions)
            {
                ((int)db_prediction.main_label).TryParseEnum(out FootballPredictionType predictionType);
                bool isHit = false;
                switch (predictionType)
                {
                    case FootballPredictionType.Match_Winner:
                        ((int)db_prediction.sub_label).TryParseEnum(out FootballMatchWinnerType matchWinnerSubType);
                        isHit = PredictionFacade.DiscernMatchWinner(matchWinnerSubType, api_fixture.GoalsHomeTeam, api_fixture.GoalsAwayTeam);
                        break;

                    case FootballPredictionType.Both_Teams_to_Score:
                        ((int)db_prediction.sub_label).TryParseEnum(out YesNoType bothToScoreSubType);
                        isHit = PredictionFacade.DiscernBothToScore(bothToScoreSubType, api_fixture.GoalsHomeTeam, api_fixture.GoalsAwayTeam);
                        break;

                    case FootballPredictionType.Under_Over:
                        ((int)db_prediction.sub_label).TryParseEnum(out FootballUnderOverType underOverSubType);
                        isHit = PredictionFacade.DiscernUnderOver(underOverSubType, api_fixture.GoalsHomeTeam, api_fixture.GoalsAwayTeam);
                        break;

                    default:
                        break;
                }

                db_prediction.is_hit = isHit;
                Logic.Database.FootballDBFacade.UpdatePrediction(db_prediction);
#if LINE_NOTIFY
                if (db_prediction.is_recommended && db_prediction.is_hit && isNotify)
                {
                    string notiMSG = Logic.Football.PredictionFacade.MakeHitNotificationMessage(api_fixture, db_prediction);

                    if (!string.IsNullOrEmpty(notiMSG))
                        Singleton.Get<LineNotifyAPI>().SendMessage(LineNotifyType.Football_Picks, notiMSG);
                }
#endif
            }
        }

        public static void DiscernPrediction(FootballDB.Tables.Prediction[] db_predictions, int home_score, int away_score)
        {
            foreach (var db_prediction in db_predictions)
            {
                ((int)db_prediction.main_label).TryParseEnum(out FootballPredictionType predictionType);
                bool isHit = false;
                switch (predictionType)
                {
                    case FootballPredictionType.Match_Winner:
                        ((int)db_prediction.sub_label).TryParseEnum(out FootballMatchWinnerType matchWinnerSubType);
                        isHit = PredictionFacade.DiscernMatchWinner(matchWinnerSubType, home_score, away_score);
                        break;

                    case FootballPredictionType.Both_Teams_to_Score:
                        ((int)db_prediction.sub_label).TryParseEnum(out YesNoType bothToScoreSubType);
                        isHit = PredictionFacade.DiscernBothToScore(bothToScoreSubType, home_score, away_score);
                        break;

                    case FootballPredictionType.Under_Over:
                        ((int)db_prediction.sub_label).TryParseEnum(out FootballUnderOverType underOverSubType);
                        isHit = PredictionFacade.DiscernUnderOver(underOverSubType, home_score, away_score);
                        break;

                    default:
                        break;
                }

                db_prediction.is_hit = isHit;
            }
        }

        public static void UpdateFixtureStatistics(int fixtureId)
        {
            if (IsAlreadyUpdatedStatistics(fixtureId))
                return;

            // Call API
            var api_fixtureStatistic = Singleton.Get<ApiLogic.FootballWebAPI>().GetFixtureStatisticByFixtureId(fixtureId);
            if (api_fixtureStatistic == null)
                return;

            var db_fixtures = Database.FootballDBFacade.SelectFixtures(where: $"id = {fixtureId}").FirstOrDefault();
            Dev.Assert(db_fixtures != null, $"UpdateFixtureStatistics db_fixtures is null FixtureId: {fixtureId}");

            // Convert api model to db model
            DataConverter.CovertApiModelToDbModel(fixtureId, db_fixtures.home_team_id, db_fixtures.away_team_id, api_fixtureStatistic,
                out FootballDB.Tables.FixtureStatistics[] coverted_fixtureStatistics);

            // DB Save
            Database.FootballDBFacade.UpdateFixtureStatistics(coverted_fixtureStatistics);
        }

        public static void UpdateH2H(short teamId1, short teamId2)
        {
            // Call API
            var api_h2h = Singleton.Get<ApiLogic.FootballWebAPI>().GetH2HFixtures(teamId1, teamId2);
            var api_filteredFixtures = api_h2h.Where(elem => Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status, elem.MatchTime));

            foreach (var api_fixture in api_filteredFixtures)
            {
                // DB Save
                Database.FootballDBFacade.UpdateFixture(api_fixture);
                UpdateFixtureStatistics(api_fixture.FixtureId);
            }
        }

        public static void UpdateTeamLastFixtures(short teamId, byte count)
        {
            // Call API
            var api_fixtures = Singleton.Get<ApiLogic.FootballWebAPI>().GetLastFixturesByTeamId(teamId, count);
            var api_filteredFixtures = api_fixtures.Where(elem => Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status, elem.MatchTime));

            foreach (var api_fixture in api_filteredFixtures)
            {
                // DB Save
                Database.FootballDBFacade.UpdateFixture(api_fixture);
                UpdateFixtureStatistics(api_fixture.FixtureId);
            }
        }

        public static Task SolveErrors(Label lbl)
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                var leagueErrors = Singleton.Get<CheckValidation>().GetErrorLeauges(InvalidType.NotExistInDB, false);
                int errorLeagueCnt = leagueErrors.Length;
                int loop = 0;
                foreach (var errorLeague in leagueErrors)
                {
                    loop++;
                    mainWindow.Set_Lable(lbl, $"Solve League Errors ({loop}/{errorLeagueCnt})");

                    // Call API
                    var api_league = Singleton.Get<ApiLogic.FootballWebAPI>().GetLeagueByLeagueId(errorLeague.LeagueId);
                    //Dev.Assert(api_league != null, $"api_league is null leagueId: {errorLeague.LeagueId}");

                    if (api_league != null)
                    {
#if LINE_NOTIFY
                        // 추가된 리그 메시지로 전송
                        if (errorLeagueCnt < 50)
                        {
                            Singleton.Get<LineNotifyAPI>().SendMessage(LineNotifyType.Dev, $"새로운 리그가 추가 됐습니다. ID: {api_league.LeagueId}, 국가: {api_league.Country}, 이름: {api_league.Name}");
                        }
#endif

                        api_league.Coverage.Predictions = CoverageLeague.HasLeague(api_league.Country, api_league.Name, api_league.Type);

                        // Update League
                        Database.FootballDBFacade.UpdateCoverage(api_league);
                        Database.FootballDBFacade.UpdateLeague(api_league);

                        // Update All Teams
                        var api_teams = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllTeamsByLeagueId((short)api_league.LeagueId);
                        Logic.Database.FootballDBFacade.UpdateTeam((short)api_league.LeagueId, api_teams.ToArray());

                        // Update All Fixtures
                        LogicFacade.UpdateAllFixturesByLeague((short)api_league.LeagueId);
                        LogicFacade.UpdateStandings((short)api_league.LeagueId);

                        Singleton.Get<CheckValidation>().DeleteErrorLeague(errorLeague);
                    }
                }

                var teamErrors = Singleton.Get<CheckValidation>().GetErrorTeams(InvalidType.NotExistInDB, false);
                int errorTeamCnt = teamErrors.Length;
                loop = 0;
                foreach (var errorTeam in teamErrors)
                {
                    loop++;
                    mainWindow.Set_Lable(lbl, $"Solve Team Errors ({loop}/{errorTeamCnt})");

                    // Call API
                    var api_team = Singleton.Get<ApiLogic.FootballWebAPI>().GetTeamByTeamId(errorTeam.TeamId);
                    //Dev.Assert(api_team != null, $"api_team is null teamId: {errorTeam.TeamsId}");

                    if (api_team != null)
                    {
                        // DB Save
                        Database.FootballDBFacade.UpdateTeam(errorTeam.LeagueId, api_team);

                        Singleton.Get<CheckValidation>().DeleteErrorTeam(errorTeam);
                    }
                }
            });
        }

        public static bool UpdateStandings(short leagueId)
        {
            var db_league = Database.FootballDBFacade.SelectLeagues(where: $"id = {leagueId}").FirstOrDefault();

            // Call API
            var api_standings = Singleton.Get<ApiLogic.FootballWebAPI>().GetStandingsByLeagueId(leagueId);
            if (api_standings.Count == 0)
                return true;

            // Call DB
            return Database.FootballDBFacade.UpdateStandings(leagueId, db_league.country_name, api_standings.ToArray());
        }

        public static bool IsAlreadyUpdatedOdds(int fixtureId)
        {
            return Database.FootballDBFacade.SelectOdds(where: $"fixture_id = {fixtureId}").FirstOrDefault() != null;
        }

        public static bool IsAlreadyUpdatedStatistics(int fixtureId)
        {
            return Database.FootballDBFacade.SelectStatistics(where: $"fixture_id = {fixtureId}").FirstOrDefault() != null;
        }

        public static bool IsExistFixture(int fixtureId)
        {
            return Database.FootballDBFacade.SelectFixtures(where: $"id = {fixtureId}").FirstOrDefault() != null;
        }
    }
}