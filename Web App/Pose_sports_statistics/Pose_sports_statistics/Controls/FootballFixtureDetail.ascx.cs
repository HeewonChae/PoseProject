using LogicCore;
using Pose_sports_statistics.Logic;
using Pose_sports_statistics.Logic.RapidAPI;
using RapidAPI;
using Repository.Data.Redis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebFormModel = Pose_sports_statistics.Models;


namespace Pose_sports_statistics.Controls
{
	public partial class FootballFixtureDetail : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
				return;
		}

		public WebFormModel.FootballFixture GetFootballFixture([RouteData] int fixtureID)
		{
			var fixture = Singleton.Get<RedisCacheManager>()
				.Get<WebFormModel.FootballFixture>
				(
					() => RequestLoader.FootballFixtureByID(fixtureID),
					RequestLoader.Locker_FootballFixtureByID,
					DateTime.Now.AddHours(1),
					RedisKeyMaker.FootballFixtureByID(fixtureID)
				);

			var homefixtures = Singleton.Get<RedisCacheManager>()
				.Get<IList<WebFormModel.FootballFixture>>
				(
					() => RequestLoader.FootballFixtureByTeamID(fixture.HomeTeam.TeamID),
					RequestLoader.Locker_FootballFixtureByTeamID,
					DateTime.Now.AddHours(12),
					RedisKeyMaker.FootballFixtureByTeamID(fixture.HomeTeam.TeamID)
				).Where(elem => elem.EventDate < DateTime.Now)
				.OrderByDescending(elem => elem.EventDate);

			var awayfixtures = Singleton.Get<RedisCacheManager>()
				.Get<IList<WebFormModel.FootballFixture>>
				(
					() => RequestLoader.FootballFixtureByTeamID(fixture.AwayTeam.TeamID),
					RequestLoader.Locker_FootballFixtureByTeamID,
					DateTime.Now.AddHours(12),
					RedisKeyMaker.FootballFixtureByTeamID(fixture.AwayTeam.TeamID)
				).Where(elem => elem.EventDate < DateTime.Now)
				.OrderByDescending(elem => elem.EventDate);

			// 홈팀 최근 6경기 득실점
			int fixtureCnt = 0;
			int goalCnt = 0;
			int goalAgainst = 0;
			foreach (var selectedfixture in homefixtures)
			{
				if (string.IsNullOrEmpty(selectedfixture.Score.FullTime) 
					|| selectedfixture.LeagueID != fixture.LeagueID)
					continue;

				// 최근 5경기만..
				if (fixtureCnt >= 6)
					break;

				bool isHomeTeam = selectedfixture.HomeTeam.TeamID == fixture.HomeTeam.TeamID;
				string[] scoreSplit = selectedfixture.Score.FullTime.Split('-');

				goalCnt += int.Parse(scoreSplit[isHomeTeam ? 0 : 1]);
				goalAgainst += int.Parse(scoreSplit[isHomeTeam ? 1 : 0]);

				fixtureCnt++;
			}
			fixture.HomeLateSixGoalPoints = $"{goalCnt} : {goalAgainst}";


			// 홈팀 최근 홈3경기 득실점
			fixtureCnt = 0;
			goalCnt = 0;
			goalAgainst = 0;
			foreach (var selectedfixture in homefixtures)
			{
				bool isHomeTeam = selectedfixture.HomeTeam.TeamID == fixture.HomeTeam.TeamID;
				if (string.IsNullOrEmpty(selectedfixture.Score.FullTime)
					|| selectedfixture.LeagueID != fixture.LeagueID
					|| !isHomeTeam)
					continue;

				// 최근 3경기만..
				if (fixtureCnt >= 3)
					break;

				
				string[] scoreSplit = selectedfixture.Score.FullTime.Split('-');

				goalCnt += int.Parse(scoreSplit[0]);
				goalAgainst += int.Parse(scoreSplit[1]);

				fixtureCnt++;
			}
			fixture.HomeLateThreeGoalPoints = $"{goalCnt} : {goalAgainst}";

			// 어웨이팀 최근 6경기 득실점
			fixtureCnt = 0;
			goalCnt = 0;
			goalAgainst = 0;
			foreach (var selectedfixture in awayfixtures)
			{
				if (string.IsNullOrEmpty(selectedfixture.Score.FullTime)
					|| selectedfixture.LeagueID != fixture.LeagueID)
					continue;

				// 최근 6경기만..
				if (fixtureCnt >= 6)
					break;

				bool isHomeTeam = selectedfixture.HomeTeam.TeamID == fixture.AwayTeam.TeamID;
				string[] scoreSplit = selectedfixture.Score.FullTime.Split('-');

				goalCnt += int.Parse(scoreSplit[isHomeTeam ? 0 : 1]);
				goalAgainst += int.Parse(scoreSplit[isHomeTeam ? 1 : 0]);

				fixtureCnt++;
			}
			fixture.AwayLateSixGoalPoints = $"{goalCnt} : {goalAgainst}";

			// 어웨이팀 최근 어웨이3경기 득실점
			fixtureCnt = 0;
			goalCnt = 0;
			goalAgainst = 0;
			foreach (var selectedfixture in awayfixtures)
			{
				bool isAwayTeam = selectedfixture.AwayTeam.TeamID == fixture.AwayTeam.TeamID;
				if (string.IsNullOrEmpty(selectedfixture.Score.FullTime)
					|| selectedfixture.LeagueID != fixture.LeagueID
					|| !isAwayTeam)
					continue;

				// 최근 3경기만..
				if (fixtureCnt >= 3)
					break;


				string[] scoreSplit = selectedfixture.Score.FullTime.Split('-');

				goalCnt += int.Parse(scoreSplit[1]);
				goalAgainst += int.Parse(scoreSplit[0]);

				fixtureCnt++;
			}
			fixture.AwayLateThreeGoalPoints = $"{goalCnt} : {goalAgainst}";

			// 홈팀 회복기간
			var homeLastFixtures = homefixtures.FirstOrDefault();
			if(homeLastFixtures != null)
			{
				fixture.HomeRecoveryDays = $"{(fixture.EventDate - homeLastFixtures.EventDate).Days}일";
			}

			// 원정팀 회복기간
			var awayLastFixtures = awayfixtures.FirstOrDefault();
			if (awayLastFixtures != null)
			{
				fixture.AwayRecoveryDays = $"{(fixture.EventDate - awayLastFixtures.EventDate).Days}일";
			}

			return fixture;
		}

		protected void Form_fixture_DataBound(object sender, EventArgs e)
		{
			if (form_fixture.Row != null)
			{
				var dataItem = this.form_fixture.DataItem as WebFormModel.FootballFixture;

				// Load data
				var standings = Singleton.Get<RedisCacheManager>()
				.Get<IList<IList<WebFormModel.FootballStanding>>>
				(
					() => RequestLoader.FootballStandingsByLeagueID(dataItem.LeagueID),
					RequestLoader.Locker_FootballStandingsByLeagueID,
					DateTime.Now.AddHours(1),
					RedisKeyMaker.FootballStandingsByLeagueID(dataItem.LeagueID)
				).SelectMany(elem => elem);

				var homeTeamStatistics = Singleton.Get<RedisCacheManager>()
				.Get<WebFormModel.FootballTeamStatistics>
				(
					() => RequestLoader.FootballTeamStatisticsByLeagueIDAndTeamID(dataItem.LeagueID, dataItem.HomeTeam.TeamID),
					RequestLoader.Locker_FootballTeamStatisticsByLeagueIDAndTeamID,
					DateTime.Now.AddHours(12),
					RedisKeyMaker.FootballTeamStatisticsByLeagueIDAndTeamID(dataItem.LeagueID, dataItem.HomeTeam.TeamID)
				);

				var awayTeamStatistics = Singleton.Get<RedisCacheManager>()
				.Get<WebFormModel.FootballTeamStatistics>
				(
					() => RequestLoader.FootballTeamStatisticsByLeagueIDAndTeamID(dataItem.LeagueID, dataItem.AwayTeam.TeamID),
					RequestLoader.Locker_FootballTeamStatisticsByLeagueIDAndTeamID,
					DateTime.Now.AddHours(12),
					RedisKeyMaker.FootballTeamStatisticsByLeagueIDAndTeamID(dataItem.LeagueID, dataItem.AwayTeam.TeamID)
				);

				// page process
				var homeStandingInfo = standings.Where(elem => elem.TeamID == dataItem.HomeTeam.TeamID).FirstOrDefault();
				var awayStandingInfo = standings.Where(elem => elem.TeamID == dataItem.AwayTeam.TeamID).FirstOrDefault();

				// 홈 순위, 최근 결과
				if (homeStandingInfo != null)
				{
					var lbl_homeRank = this.form_fixture.Row.FindControl("lbl_homeRank") as Label;
					lbl_homeRank.Text = homeStandingInfo.Rank.ToString();

					if(homeStandingInfo.Forme != null)
					{
						for (int i = 0; i < homeStandingInfo.Forme.Length; i++)
						{
							var label = this.form_fixture.Row.FindControl($"home_form_{i}") as Label;
							label.Text = homeStandingInfo.Forme[homeStandingInfo.Forme.Length - 1 - i].ToString();

							if (label.Text.Equals("W"))
								label.ForeColor = Color.Green;
							else if (label.Text.Equals("L"))
								label.ForeColor = Color.Red;
							else
								label.ForeColor = Color.Orange;
						}
					}

					var lbl_homePoint = this.form_fixture.Row.FindControl("lbl_homePoint") as Label;
					lbl_homePoint.Text = homeStandingInfo.Points.ToString();
				}

				// 원정 순위, 최근 결과
				if (awayStandingInfo != null)
				{
					var lbl_awayRank = this.form_fixture.Row.FindControl("lbl_awyaRank") as Label;
					lbl_awayRank.Text = awayStandingInfo.Rank.ToString();

					if(awayStandingInfo.Forme != null)
					{
						for (int i = 0; i < awayStandingInfo.Forme.Length; i++)
						{
							var label = this.form_fixture.Row.FindControl($"away_form_{i}") as Label;
							label.Text = awayStandingInfo.Forme[awayStandingInfo.Forme.Length - 1 - i].ToString();

							if (label.Text.Equals("W"))
								label.ForeColor = Color.Green;
							else if (label.Text.Equals("L"))
								label.ForeColor = Color.Red;
							else
								label.ForeColor = Color.Orange;
						}
					}

					var lbl_awayPoint = this.form_fixture.Row.FindControl("lbl_awayPoint") as Label;
					lbl_awayPoint.Text = awayStandingInfo.Points.ToString();
				}

				// 전적
				var lbl_homeTotalRecord = this.form_fixture.Row.FindControl("lbl_homeTotalRecord") as Label;
				lbl_homeTotalRecord.Text = $"{homeTeamStatistics.Matchs.Wins.Total}/{homeTeamStatistics.Matchs.Draws.Total}/{homeTeamStatistics.Matchs.Loses.Total} " +
					$"({homeTeamStatistics.Matchs.Wins.Home}/{homeTeamStatistics.Matchs.Draws.Home}/{homeTeamStatistics.Matchs.Loses.Home})";

				var lbl_awayTotalRecord = this.form_fixture.Row.FindControl("lbl_awayTotalRecord") as Label;
				lbl_awayTotalRecord.Text = $"({awayTeamStatistics.Matchs.Wins.Away}/{awayTeamStatistics.Matchs.Draws.Away}/{awayTeamStatistics.Matchs.Loses.Away}) " +
					$"{awayTeamStatistics.Matchs.Wins.Total}/{awayTeamStatistics.Matchs.Draws.Total}/{awayTeamStatistics.Matchs.Loses.Total}";

				// 평균 득점
				var lbl_homeGoalsAvg = this.form_fixture.Row.FindControl("lbl_homeGoalsAvg") as Label;
				lbl_homeGoalsAvg.Text = $"{homeTeamStatistics.GoalsAvg.GoalsFor.Total} ({homeTeamStatistics.GoalsAvg.GoalsFor.Home})";

				var lbl_awayGoalsAvg = this.form_fixture.Row.FindControl("lbl_awayGoalsAvg") as Label;
				lbl_awayGoalsAvg.Text = $"({awayTeamStatistics.GoalsAvg.GoalsFor.Away}) {awayTeamStatistics.GoalsAvg.GoalsFor.Total}";

				// 평균 실점
				var lbl_homeGoalAgainst = this.form_fixture.Row.FindControl("lbl_homeGoalAgainst") as Label;
				lbl_homeGoalAgainst.Text = $"{homeTeamStatistics.GoalsAvg.GoalsAgainst.Total} ({homeTeamStatistics.GoalsAvg.GoalsAgainst.Home})";

				var lbl_awayGoalAgainst = this.form_fixture.Row.FindControl("lbl_awayGoalAgainst") as Label;
				lbl_awayGoalAgainst.Text = $"({awayTeamStatistics.GoalsAvg.GoalsAgainst.Away}) {awayTeamStatistics.GoalsAvg.GoalsAgainst.Total}";

				// 최근 6경기 득실점
				var lbl_homeLastSixPoints = this.form_fixture.Row.FindControl("lbl_homeLastSixPoints") as Label;
				lbl_homeLastSixPoints.Text = dataItem.HomeLateSixGoalPoints;

				var lbl_awayLastSixPoints = this.form_fixture.Row.FindControl("lbl_awayLastSixPoints") as Label;
				lbl_awayLastSixPoints.Text = dataItem.AwayLateSixGoalPoints;

				// 홈/원정 각 3경기 득실점
				var lbl_homeLastThreePoints = this.form_fixture.Row.FindControl("lbl_homeLastThreePoints") as Label;
				lbl_homeLastThreePoints.Text = dataItem.HomeLateThreeGoalPoints;

				var lbl_awayLastThreePoints = this.form_fixture.Row.FindControl("lbl_awayLastThreePoints") as Label;
				lbl_awayLastThreePoints.Text = dataItem.AwayLateThreeGoalPoints;

				// 회복기간
				var lbl_homeRecoveryDays = this.form_fixture.Row.FindControl("lbl_homeRecoveryDays") as Label;
				lbl_homeRecoveryDays.Text = dataItem.HomeRecoveryDays;

				var lbl_awayRecoveryDays = this.form_fixture.Row.FindControl("lbl_awayRecoveryDays") as Label;
				lbl_awayRecoveryDays.Text = dataItem.AwayRecoveryDays;
			}
		}
	}
}