using LogicCore.Utility;
using Pose_sports_statistics.Logic.RapidAPI;
using Repository.Data.Redis;
using System;

using WebFormModel = Pose_sports_statistics.Models;

namespace Pose_sports_statistics.Pages
{
	public partial class FootballFixture : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
				return;

			int fixtureID = int.Parse(this.Page.RouteData.Values["fixtureID"].ToString());

			var fixture = Singleton.Get<RedisCacheManager>()
				.Get<WebFormModel.FootballFixture>
				(
					() => RequestLoader.FootballFixtureByID(fixtureID),
					RequestLoader.Locker_FootballFixtureByID,
					DateTime.Now.AddHours(1),
					RedisKeyMaker.FootballFixtureByID(fixtureID)
				);

			var curLeague = Singleton.Get<RedisCacheManager>()
				.Get<WebFormModel.FootballLeague>
				(
					() => RequestLoader.FootballSeasonsByLeagueID(fixture.LeagueID),
					RequestLoader.Locker_FootballSeasonsByLeagueID,
					DateTime.Now.AddDays(1),
					RedisKeyMaker.FootballSeasonsByLeagueID(fixture.LeagueID)
				);

			// Standing
			ctrl_footballStanding.SetSearchTeamID(fixture.HomeTeam.TeamID, fixture.AwayTeam.TeamID);
			ctrl_footballStanding.SetSearchLeagueID(fixture.LeagueID);

			// H2H
			ctrl_FootballH2HFixtureList.SetSearchTeamID(fixture.HomeTeam.TeamID, fixture.AwayTeam.TeamID);

			// FixturesResult
			ctrl_footballTeamFixtureResults.SetSearchTeamID(fixture.HomeTeam.TeamID, fixture.AwayTeam.TeamID);
			ctrl_footballTeamFixtureResults.SetSearchTeamName(fixture.HomeTeam.TeamName, fixture.AwayTeam.TeamName);
			ctrl_footballTeamFixtureResults.SetSearchLeagueID(fixture.LeagueID);

			// Player
			//ctrl_footballPlayerList.SetSearchTeamID(fixture.HomeTeam.TeamID, fixture.AwayTeam.TeamID);
			//ctrl_footballPlayerList.SetSearchTeamName(fixture.HomeTeam.TeamName, fixture.AwayTeam.TeamName);
			//ctrl_footballPlayerList.SetSearchLeagueID(curLeague.LeagueID);

			//int startYear = curLeague.SeasonStart.Year;
			//int endYear = curLeague.SeasonEnd.Year;
			//if (startYear != endYear)
			//	ctrl_footballPlayerList.SetSearchSeason($"{startYear}-{endYear}");
			//else
			//	ctrl_footballPlayerList.SetSearchSeason($"{startYear}");
		}
	}
}