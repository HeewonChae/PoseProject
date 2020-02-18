using LogicCore;
using Newtonsoft.Json;
using Pose_sports_statistics.Logic.RapidAPI;
using Repository.Data.Redis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebFormModel = Pose_sports_statistics.Models;

namespace Pose_sports_statistics.Controls
{
	public partial class InterestedEventList : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
				return;
		}

		public IQueryable<dynamic> GetFixtures()
		{
			// 관심 경기 데이터 가져오기
			var interestedFixtures = Singleton.Get<RedisCacheManager>()
				.GetNullable<IList<WebFormModel.FootballFixture>>
				(
					RedisKeyMaker.FootballInterestedFixture()
				);

			// step1. 시간 순으로 정렬
			var sortedByStartDate = interestedFixtures.OrderBy(elem => elem.EventDate);

			// step2. 데이터를 리그, 시작시간으로 그룹화
			var group_query = sortedByStartDate.GroupBy(elem => new { League = elem.LeagueID, StartTime = elem.EventDate });

			var result_query = from queryData in group_query
							   select new
							   {
								   League = queryData.FirstOrDefault().League.Name,
								   queryData.Key.StartTime,
								   queryData.FirstOrDefault()?.League.Flag,
								   Fixtures = queryData.ToArray()
							   };

			return result_query.AsQueryable();
		}

		protected void lv_footballFixtureList_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
		{
			//set current page startindex, max rows and rebind to false
			this.lv_DataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
		}

		protected void GV_FootballFixtures_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var dataItem = e.Row.DataItem as WebFormModel.FootballFixture;

				var chk_interestedFixture = e.Row.FindControl("chk_interestedFixture") as CheckBox;

				var homeHyperLink = e.Row.FindControl("hl_homeTeamLink") as HyperLink;
				var awayHyperLink = e.Row.FindControl("hl_awayTeamLink") as HyperLink;

				homeHyperLink.NavigateUrl = GetRouteUrl("FootballTeamByID", new { TeamID = dataItem.AwayTeam.TeamID });
				awayHyperLink.NavigateUrl = GetRouteUrl("FootballTeamByID", new { TeamID = dataItem.AwayTeam.TeamID });

				if (dataItem != null)
				{
					chk_interestedFixture.Checked = true;

					// 리그 5위 이상 팀 Bold체
					var leaugeStandings = Singleton.Get<RedisCacheManager>()
					.Get<IList<IList<WebFormModel.FootballStanding>>>
					(
						() => RequestLoader.FootballStandingsByLeagueID(dataItem.LeagueID),
						RequestLoader.Locker_FootballStandingsByLeagueID,
						DateTime.Now.AddHours(4),
						RedisKeyMaker.FootballStandingsByLeagueID(dataItem.LeagueID)
					).SelectMany(elem => elem);

					var homeStandingInfo = leaugeStandings.Where(elem => elem.TeamID == dataItem.HomeTeam.TeamID).FirstOrDefault();
					var awayStandingInfo = leaugeStandings.Where(elem => elem.TeamID == dataItem.AwayTeam.TeamID).FirstOrDefault();

					if (homeStandingInfo?.Rank <= 5)
					{
						homeHyperLink.Font.Bold = true;
					}

					if (awayStandingInfo?.Rank <= 5)
					{
						awayHyperLink.Font.Bold = true;
					}
				}

				// 단폴픽 체크
				var prediction = Singleton.Get<RedisCacheManager>()
					.Get<IList<WebFormModel.FootballPrediction>>
					(
						() => RequestLoader.FootballPredictionByFixtureID(dataItem.FixtureID),
						RequestLoader.Locker_FootballPredictionByFixtureID,
						DateTime.Now.AddDays(1),
						RedisKeyMaker.FootballPredictionByFixtureID(dataItem.FixtureID)
					).FirstOrDefault();

				if (prediction?.MatchWinner?.Trim(' ').Length == 1)
				{
					e.Row.BackColor = Color.LightGreen;
				}
			}
		}

		/// <summary>
		/// 관심 경기 체크
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void chk_fixture_InterestedIndexChanged(Object sender, EventArgs e)
		{
			CheckBox chkbox = sender as CheckBox;

			if (chkbox == null)
				return;

			// 관심 경기 데이터 가져오기
			var interestedFixtures = Singleton.Get<RedisCacheManager>()
				.GetNullable<IList<WebFormModel.FootballFixture>>
				(
					RedisKeyMaker.FootballInterestedFixture()
				);

			// 제거할 경기
			int interestedFixtureID = int.Parse(chkbox.ToolTip);
			var interestedFixture = interestedFixtures.Where(elem => elem.FixtureID == interestedFixtureID).FirstOrDefault();

			if (interestedFixture == null)
			{
				// 이미 제거된 경기

				// refrash
				Page.Response.Redirect(Page.Request.Url.ToString(), true);
				return; 
			}

			// 관심 경기에서 제거
			interestedFixtures.Remove(interestedFixture);

			// 레디스에 저장
			Singleton.Get<RedisCacheManager>().Set
				(
					JsonConvert.SerializeObject(interestedFixtures),
					RequestLoader.Locker_FootballInterestedFixture,
					new DateTime(9999, 12, 31),
					RedisKeyMaker.FootballInterestedFixture()
				);

			// refrash
			Page.Response.Redirect(Page.Request.Url.ToString(), true);
		}
	}
}