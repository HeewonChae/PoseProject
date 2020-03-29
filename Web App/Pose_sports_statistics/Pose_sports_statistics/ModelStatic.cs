using LogicCore.DataMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using APIModel = RapidAPI.Models.Football;
using WebModel = Pose_sports_statistics.Models;

namespace Pose_sports_statistics
{
    public static class ModelStatic
    {
        public static void InitModelMapping()
        {
            var footballFixtureMap = DataMapper.Resolve<APIModel.Fixture, WebModel.FootballFixture>();
            footballFixtureMap.Complex<APIModel.Fixture, WebModel.FootballFixture>(source => source.HomeTeam, dest => dest.HomeTeam);
            footballFixtureMap.Complex<APIModel.Fixture, WebModel.FootballFixture>(source => source.AwayTeam, dest => dest.AwayTeam);
            footballFixtureMap.Complex<APIModel.Fixture, WebModel.FootballFixture>(source => source.League, dest => dest.League);
            footballFixtureMap.Complex<APIModel.Fixture, WebModel.FootballFixture>(source => source.Score, dest => dest.Score);

            var footballStandingMap = DataMapper.Resolve<APIModel.Standings, WebModel.FootballStandings>();
            footballStandingMap.Complex<APIModel.Standings, WebModel.FootballStandings>(source => source.AllPlayedInfo, dest => dest.AllPlayedInfo);
            footballStandingMap.Complex<APIModel.Standings, WebModel.FootballStandings>(source => source.HomePlayedInfo, dest => dest.HomePlayedInfo);
            footballStandingMap.Complex<APIModel.Standings, WebModel.FootballStandings>(source => source.AwayPlayedInfo, dest => dest.AwayPlayedInfo);

            var footballTeamStatisticsMap = DataMapper.Resolve<APIModel.TeamStatistics, WebModel.FootballTeamStatistics>();
            footballTeamStatisticsMap.Complex<APIModel.TeamStatistics, WebModel.FootballTeamStatistics>(source => source.Matchs, dest => dest.Matchs);
            footballTeamStatisticsMap.Complex<APIModel.TeamStatistics, WebModel.FootballTeamStatistics>(source => source.Goals, dest => dest.Goals);
            footballTeamStatisticsMap.Complex<APIModel.TeamStatistics, WebModel.FootballTeamStatistics>(source => source.GoalsAvg, dest => dest.GoalsAvg);
            var footballTeamStatistics_Match = DataMapper.Resolve<APIModel.TeamStatistics.MatchsInfo, WebModel.FootballTeamStatistics.MatchsInfo>();
            footballTeamStatistics_Match.Complex<APIModel.TeamStatistics.MatchsInfo, WebModel.FootballTeamStatistics.MatchsInfo>(source => source.MatchsPlayed, dest => dest.MatchsPlayed);
            footballTeamStatistics_Match.Complex<APIModel.TeamStatistics.MatchsInfo, WebModel.FootballTeamStatistics.MatchsInfo>(source => source.Wins, dest => dest.Wins);
            footballTeamStatistics_Match.Complex<APIModel.TeamStatistics.MatchsInfo, WebModel.FootballTeamStatistics.MatchsInfo>(source => source.Draws, dest => dest.Draws);
            footballTeamStatistics_Match.Complex<APIModel.TeamStatistics.MatchsInfo, WebModel.FootballTeamStatistics.MatchsInfo>(source => source.Loses, dest => dest.Loses);
            var footballTeamStatisticsMap_Goal = DataMapper.Resolve<APIModel.TeamStatistics.GoalsInfo, WebModel.FootballTeamStatistics.GoalsInfo>();
            footballTeamStatisticsMap_Goal.Complex<APIModel.TeamStatistics.GoalsInfo, WebModel.FootballTeamStatistics.GoalsInfo>(source => source.GoalsFor, dest => dest.GoalsFor);
            footballTeamStatisticsMap_Goal.Complex<APIModel.TeamStatistics.GoalsInfo, WebModel.FootballTeamStatistics.GoalsInfo>(source => source.GoalsAgainst, dest => dest.GoalsAgainst);
            var footballTeamStatisticsMap_GoalAvg = DataMapper.Resolve<APIModel.TeamStatistics.GoalsAvgInfo, WebModel.FootballTeamStatistics.GoalsAvgInfo>();
            footballTeamStatisticsMap_GoalAvg.Complex<APIModel.TeamStatistics.GoalsAvgInfo, WebModel.FootballTeamStatistics.GoalsAvgInfo>(source => source.GoalsFor, dest => dest.GoalsFor);
            footballTeamStatisticsMap_GoalAvg.Complex<APIModel.TeamStatistics.GoalsAvgInfo, WebModel.FootballTeamStatistics.GoalsAvgInfo>(source => source.GoalsAgainst, dest => dest.GoalsAgainst);

            var footballPalyerMap = DataMapper.Resolve<APIModel.Player, WebModel.FootballPlayer>();
            footballPalyerMap.Complex<APIModel.Player, WebModel.FootballPlayer>(source => source.Shots, dest => dest.Shots);
            footballPalyerMap.Complex<APIModel.Player, WebModel.FootballPlayer>(source => source.Goals, dest => dest.Goals);
            footballPalyerMap.Complex<APIModel.Player, WebModel.FootballPlayer>(source => source.Passes, dest => dest.Passes);
            footballPalyerMap.Complex<APIModel.Player, WebModel.FootballPlayer>(source => source.Tackles, dest => dest.Tackles);
            footballPalyerMap.Complex<APIModel.Player, WebModel.FootballPlayer>(source => source.Dribbles, dest => dest.Dribbles);
            footballPalyerMap.Complex<APIModel.Player, WebModel.FootballPlayer>(source => source.Games, dest => dest.Games);

            var footballLeagueMap = DataMapper.Resolve<APIModel.LeagueDetatil, WebModel.FootballLeague>();
            footballLeagueMap.Complex<APIModel.LeagueDetatil, WebModel.FootballLeague>(source => source.Coverage, dest => dest.Coverage);

            var footballPredictionMap = DataMapper.Resolve<APIModel.Prediction, WebModel.FootballPrediction>();
            footballPredictionMap.Complex<APIModel.Prediction, WebModel.FootballPrediction>(source => source.WinningPG, dest => dest.WinningPG);
            footballPredictionMap.Complex<APIModel.Prediction, WebModel.FootballPrediction>(source => source.Comparison, dest => dest.Comparison);
            var footballPrediction_ComparisonMap = DataMapper.Resolve<APIModel.Prediction.ComparisonInfo, WebModel.FootballPrediction.ComparisonInfo>();
            footballPrediction_ComparisonMap.Complex<APIModel.Prediction.ComparisonInfo, WebModel.FootballPrediction.ComparisonInfo>(source => source.Forme, dest => dest.Forme);
            footballPrediction_ComparisonMap.Complex<APIModel.Prediction.ComparisonInfo, WebModel.FootballPrediction.ComparisonInfo>(source => source.Attack, dest => dest.Attack);
            footballPrediction_ComparisonMap.Complex<APIModel.Prediction.ComparisonInfo, WebModel.FootballPrediction.ComparisonInfo>(source => source.Defense, dest => dest.Defense);
            footballPrediction_ComparisonMap.Complex<APIModel.Prediction.ComparisonInfo, WebModel.FootballPrediction.ComparisonInfo>(source => source.H2H, dest => dest.H2H);
            footballPrediction_ComparisonMap.Complex<APIModel.Prediction.ComparisonInfo, WebModel.FootballPrediction.ComparisonInfo>(source => source.GoalsH2H, dest => dest.GoalsH2H);
        }
    }
}