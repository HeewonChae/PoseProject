using Newtonsoft.Json;
using Pose_sports_statistics.Logic.Football;
using RapidAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using APIModel = RapidAPI.Models.Football;
using WebFormModel = Pose_sports_statistics.Models;

namespace Pose_sports_statistics.Logic.RapidAPI
{
	public static class RequestLoader
	{
		// 관심경기 데이터 저장용..
		public static object Locker_FootballInterestedFixture = new object();

		private static readonly FootballAPI FootballAPI;

		static RequestLoader()
		{
			var endPoint = ConfigurationManager.AppSettings["football_api_url"];
			var host = ConfigurationManager.AppSettings["football_api_host"];
			var key = ConfigurationManager.AppSettings["football_api_key"];

			FootballAPI = new FootballAPI();
			FootballAPI.Init(endPoint, host, key);
		}

		public static object Locker_FootballFixtureByID = new object();

		public static Func<int, string> FootballFixtureByID = (fixtureId) =>
		{
			FootballAPI.RequestEx(FootballAPI.FootballFixturesByID
				, fixtureId
				, out WebFormModel.FootballFixture fixture);

			return JsonConvert.SerializeObject(fixture);
		};

		public static object Locker_FootballFixturesByDate = new object();

		public static Func<DateTime, string> FootballFixturesByDate = (date) =>
		{
			// fixtureList 얻어오기
			FootballAPI.RequestEx(FootballAPI.FootballFixturesByDate
				, date.AddDays(-1)
				, out IList<WebFormModel.FootballFixture> fixtures_yester);

			FootballAPI.RequestEx(FootballAPI.FootballFixturesByDate
				, date
				, out IList<WebFormModel.FootballFixture> fixtures_today);

			FootballAPI.RequestEx(FootballAPI.FootballFixturesByDate
				, date.AddDays(1)
				, out IList<WebFormModel.FootballFixture> fixtures_DayAdd1);

			FootballAPI.RequestEx(FootballAPI.FootballFixturesByDate
				, date.AddDays(2)
				, out IList<WebFormModel.FootballFixture> fixtures_DayAdd2);

			FootballAPI.RequestEx(FootballAPI.FootballFixturesByDate
				, date.AddDays(3)
				, out IList<WebFormModel.FootballFixture> fixtures_DayAdd3);

			// step1. 픽스처 합침
			var fixtures = fixtures_yester.ToList();
			fixtures.AddRange(fixtures_today);
			fixtures.AddRange(fixtures_DayAdd1);
			fixtures.AddRange(fixtures_DayAdd2);
			fixtures.AddRange(fixtures_DayAdd3);

			IList<WebFormModel.FootballFixture> dateOrderedFixtures = null;
			// step2. 정보가 부족한 경기 필터링
			if (fixtures.Count > 0)
			{
				var fixturesGroupByLeagueID = fixtures.GroupBy(elem => elem.LeagueID).ToList();

				List<Task<IList<WebFormModel.FootballFixture>>> taskList = new List<Task<IList<WebFormModel.FootballFixture>>>();
				foreach (var groupingFixtures in fixturesGroupByLeagueID)
				{
					taskList.Add(Task.Run(() =>
					{
						IList<WebFormModel.FootballFixture> result = new List<WebFormModel.FootballFixture>();

						FootballAPI.RequestEx(FootballAPI.FootballSeasonsByLeagueID
						, groupingFixtures.Key
						, out IList<WebFormModel.FootballLeague> leagues);
						var curSeasonLeague = leagues.Where(elem => elem.IsCurrent == 1).FirstOrDefault();

						// 정보 부족 리그는 무시
						if (curSeasonLeague == null
						|| !curSeasonLeague.Coverage.Odds
						//|| !curSeasonLeague.Coverage.Standings
						//|| !curSeasonLeague.Coverage.Players
						//|| !curSeasonLeague.Coverage.TopScorers
						)
							return result;

						// load oddsInfo
						//var OddsList = FootballAPI.OddsByLeagueIDAndLableID(groupingFixtures.Key, (int)BetLabelType._Match_Winner);

						//if (OddsList.Count > 0)
						//{
						//	foreach (var fixture in groupingFixtures)
						//	{
						//		var Betvalues = OddsList.Where(oddsInfo => oddsInfo.FixtureMini.FixtureID == fixture.FixtureID).FirstOrDefault()?
						//		.Bookmakers.SelectMany(bookmaker => bookmaker.BetInfos[0].BetValues).ToList();

						//		if (Betvalues != null)
						//		{
						//			var homeOddsList = Betvalues
						//			.Where(value => value.Name.Equals(BetValueNameType.Home.ToString()))
						//			.Select(value => value.Odds);
						//			fixture.HomeOdds = $"{homeOddsList.Min()} - {homeOddsList.Max()}";

						//			var DrawOddsList = Betvalues
						//			.Where(value => value.Name.Equals(BetValueNameType.Draw.ToString()))
						//			.Select(value => value.Odds);
						//			fixture.DrawOdds = $"{DrawOddsList.Min()} - {DrawOddsList.Max()}";

						//			var awayOddsList = Betvalues
						//			.Where(value => value.Name.Equals(BetValueNameType.Away.ToString()))
						//			.Select(value => value.Odds);
						//			fixture.AwayOdds = $"{awayOddsList.Min()} - {awayOddsList.Max()}";
						//		}

						//		result.Add(fixture);
						//	}
						//}

						return groupingFixtures.ToList();
					}));
				}

				Task.WaitAll(taskList.ToArray());

				var filteredFixtures = new List<WebFormModel.FootballFixture>();
				foreach (var completeTask in taskList)
				{
					filteredFixtures.AddRange(completeTask.Result);
				}

				dateOrderedFixtures = filteredFixtures.OrderBy(elem => elem.EventDate).ToArray();
			}

			//dateOrderedFixtures = fixtures.OrderBy(elem => elem.EventDate).ToArray();

			return JsonConvert.SerializeObject(dateOrderedFixtures);
		};

		public static object Locker_FootballStandingsByLeagueID = new object();

		public static Func<int, string> FootballStandingsByLeagueID = (leagueID) =>
		{
			FootballAPI.RequestEx(FootballAPI.StandingsByLeagueID
				, leagueID
				, out IList<WebFormModel.FootballStanding> standings);

			return JsonConvert.SerializeObject(standings);
		};

		public static object Locker_FootballTeamStatisticsByLeagueIDAndTeamID = new object();

		public static Func<int, int, string> FootballTeamStatisticsByLeagueIDAndTeamID = (leagueID, teamID) =>
	   {
		   FootballAPI.RequestEx(FootballAPI.TeamStatisticsByLeagueIDAndTeamID
			   , leagueID
			   , teamID
			   , out WebFormModel.FootballTeamStatistics standings);

		   return JsonConvert.SerializeObject(standings);
	   };

		public static object Locker_FootballFixtureByTeamID = new object();

		public static Func<int, string> FootballFixtureByTeamID = (teamID) =>
		{
			FootballAPI.RequestEx(FootballAPI.TeamFixturesByTeamID
				, teamID
				, out IList<WebFormModel.FootballFixture> teamFixtures);

			var fixturesGroupByLeagueID = teamFixtures.GroupBy(elem => elem.LeagueID).ToList();

			// 현재 시즌경기들만 필터링
			var filteredTeamFixture = new List<WebFormModel.FootballFixture>();
			foreach (var groupingFixtures in fixturesGroupByLeagueID)
			{
				FootballAPI.RequestEx(FootballAPI.FootballSeasonsByLeagueID
						, groupingFixtures.Key
						, out IList<WebFormModel.FootballLeague> leagues);

				var curSeasonLeague = leagues.Where(elem => elem.IsCurrent == 1).FirstOrDefault();
				filteredTeamFixture.AddRange(groupingFixtures.Where(elem => elem.EventDate > curSeasonLeague.SeasonStart));
			}

			return JsonConvert.SerializeObject(filteredTeamFixture);
		};

		public static object Locker_FootballH2HFixtureByTeamID = new object();

		public static Func<int, int, string> FootballH2HFixtureByTeamID = (teamID1, teamID2) =>
		{
			FootballAPI.RequestEx(FootballAPI.FootballH2HFixtureByTeamID
				, teamID1
				, teamID2
				, out IList<WebFormModel.FootballFixture> fixtures);

			return JsonConvert.SerializeObject(fixtures);
		};

		public static object Locker_FootballSeasonsByLeagueID = new object();

		public static Func<int, string> FootballSeasonsByLeagueID = (leagueID) =>
		{
			FootballAPI.RequestEx(FootballAPI.FootballSeasonsByLeagueID
				, leagueID
				, out IList<WebFormModel.FootballLeague> leagues);

			var curSeasonLeague = leagues.Where(elem => elem.IsCurrent == 1).FirstOrDefault();

			return JsonConvert.SerializeObject(curSeasonLeague);
		};

		public static object Locker_FootballPlayersByTeamIDAndSeason = new object();

		public static Func<int, string, string> FootballPlayersByTeamIDAndSeason = (teamID, season) =>
		{
			FootballAPI.RequestEx(FootballAPI.FootballPlayersByTeamIDAndSeason
				, teamID
				, season
				, out IList<WebFormModel.FootballPlayer> players);

			return JsonConvert.SerializeObject(players);
		};

		public static object Locker_FootballPredictionByFixtureID = new object();

		public static Func<int, string> FootballPredictionByFixtureID = (fixtureID) =>
		{
			FootballAPI.RequestEx(FootballAPI.FootballPredictionByFixtureID
				, fixtureID
				, out IList<WebFormModel.FootballPrediction> predictions);

			return JsonConvert.SerializeObject(predictions);
		};

		public static object Locker_FootballTopScorerByLeagueID = new object();

		public static Func<int, string> FootballTopScorerByLeagueID = (leagueID) =>
		{
			FootballAPI.RequestEx(FootballAPI.FootballTopScorerByLeagueID
				, leagueID
				, out IList<WebFormModel.FootballPlayer> players);

			return JsonConvert.SerializeObject(players);
		};

		public static Func<int, APIModel.Odds> FootballOddsByFixtureID = (fixtureID) =>
		{
			var oddsInfo = FootballAPI.OddsByFixtureID(fixtureID);

			return oddsInfo;
		};
	}
}