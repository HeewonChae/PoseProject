using LogicCore.DataMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiModel = RapidAPI.Models;

namespace SportsAdminTool.Model.Football.Mapper
{
    public static class FootballMapper
    {
        public static void Mapping()
        {
            // Country
            DataMapper.Resolve<ApiModel.Football.Country, Country>();

            // Leauge
            var leagueMapper = DataMapper.Resolve<ApiModel.Football.LeagueDetatil, League>();
            leagueMapper.Complex<ApiModel.Football.LeagueDetatil, League>(source => source.Coverage, dest => dest.Coverage);
            var leagueCoverageMapper = DataMapper.Resolve<ApiModel.Football.LeagueDetatil.CoverageInfo, League.CoverageInfo>();
            leagueCoverageMapper.Complex<ApiModel.Football.LeagueDetatil.CoverageInfo, League.CoverageInfo>(source => source.FixtureCoverage, dest => dest.FixtureCoverage);

            // Team
            DataMapper.Resolve<ApiModel.Football.Team, Team>();

            // Standing
            var standingMapper = DataMapper.Resolve<ApiModel.Football.Standings, Standings>();
            standingMapper.Complex<ApiModel.Football.Standings, Standings>(source => source.AllPlayedInfo, dest => dest.AllPlayedInfo);

            // Fixture
            var fixtureMapper = DataMapper.Resolve<ApiModel.Football.Fixture, Fixture>();
            fixtureMapper.Complex<ApiModel.Football.Fixture, Fixture>(source => source.League, dest => dest.League);
            fixtureMapper.Complex<ApiModel.Football.Fixture, Fixture>(source => source.HomeTeam, dest => dest.HomeTeam);
            fixtureMapper.Complex<ApiModel.Football.Fixture, Fixture>(source => source.AwayTeam, dest => dest.AwayTeam);
            fixtureMapper.Complex<ApiModel.Football.Fixture, Fixture>(source => source.Statistic, dest => dest.Statistic);

            // Fixture Statistic
            var fixtureStatisticMapper = DataMapper.Resolve<ApiModel.Football.FixtureStatistic, FixtureStatistic>();
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.TotalShorts, dest => dest.TotalShorts);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.ShotsOnGoal, dest => dest.ShotsOnGoal);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.ShotsOffGoal, dest => dest.ShotsOffGoal);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.BlockedShots, dest => dest.BlockedShots);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.ShotsInsideBox, dest => dest.ShotsInsideBox);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.ShotsOutsideBox, dest => dest.ShotsOutsideBox);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.GoalkeeperSaves, dest => dest.GoalkeeperSaves);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.Fouls, dest => dest.Fouls);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.CornerKicks, dest => dest.CornerKicks);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.Offsides, dest => dest.Offsides);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.YellowCards, dest => dest.YellowCards);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.RedCards, dest => dest.RedCards);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.BallPossession, dest => dest.BallPossession);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.TotalPasses, dest => dest.TotalPasses);
            fixtureStatisticMapper.Complex<ApiModel.Football.FixtureStatistic, FixtureStatistic>(source => source.AccuratePasses, dest => dest.AccuratePasses);

            // Odds
            var oddsMapper = DataMapper.Resolve<ApiModel.Football.Odds, Odds>();
            oddsMapper.Complex<ApiModel.Football.Odds, Odds>(source => source.FixtureMini, dest => dest.FixtureMini);
            oddsMapper.Add<ApiModel.Football.Odds, Odds>("Bookmakers", "SetBookmakers");
            var bookMakerMapper = DataMapper.Resolve<ApiModel.Football.Odds.BookmakerInfo, Odds.BookmakerInfo>();
            bookMakerMapper.Add<ApiModel.Football.Odds.BookmakerInfo, Odds.BookmakerInfo>("BetInfos", "SetBetInfos");
            var betInfoMapper = DataMapper.Resolve<ApiModel.Football.Odds.BookmakerInfo.BetInfo, Odds.BookmakerInfo.BetInfo>();
            betInfoMapper.Add<ApiModel.Football.Odds.BookmakerInfo.BetInfo, Odds.BookmakerInfo.BetInfo>("BetValues", "SetBetValues");
            var betValueMapper = DataMapper.Resolve<ApiModel.Football.Odds.BookmakerInfo.BetInfo.BetValue, Odds.BookmakerInfo.BetInfo.BetValue>();
        }
    }
}