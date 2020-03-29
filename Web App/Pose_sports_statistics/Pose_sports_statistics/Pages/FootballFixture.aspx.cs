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

            int fixtureId = int.Parse(this.Page.RouteData.Values["FixtureId"].ToString());

            var fixture = Singleton.Get<RedisCacheManager>()
                .Get<WebFormModel.FootballFixture>
                (
                    () => RequestLoader.FootballFixtureById(fixtureId),
                    RequestLoader.Locker_FootballFixtureById,
                    DateTime.Now.AddHours(1),
                    RedisKeyMaker.FootballFixtureById(fixtureId)
                );

            var curLeague = Singleton.Get<RedisCacheManager>()
                .Get<WebFormModel.FootballLeague>
                (
                    () => RequestLoader.FootballSeasonsByLeagueId(fixture.LeagueId),
                    RequestLoader.Locker_FootballSeasonsByLeagueId,
                    DateTime.Now.AddDays(1),
                    RedisKeyMaker.FootballSeasonsByLeagueId(fixture.LeagueId)
                );

            // Standing
            ctrl_footballStanding.SetSearchTeamID(fixture.HomeTeam.TeamId, fixture.AwayTeam.TeamId);
            ctrl_footballStanding.SetSearchLeagueID(fixture.LeagueId);

            // H2H
            ctrl_FootballH2HFixtureList.SetSearchTeamID(fixture.HomeTeam.TeamId, fixture.AwayTeam.TeamId);

            // FixturesResult
            ctrl_footballTeamFixtureResults.SetSearchTeamID(fixture.HomeTeam.TeamId, fixture.AwayTeam.TeamId);
            ctrl_footballTeamFixtureResults.SetSearchTeamName(fixture.HomeTeam.TeamName, fixture.AwayTeam.TeamName);
            ctrl_footballTeamFixtureResults.SetSearchLeagueID(fixture.LeagueId);

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