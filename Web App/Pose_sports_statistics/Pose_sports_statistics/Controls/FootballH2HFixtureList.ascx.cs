using LogicCore;
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
	public partial class FootballH2HFixtureList : System.Web.UI.UserControl
	{
		private int[] _searchTeamIDs;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
				return;
		}

		public void SetSearchTeamID(params int[] searchTeamID)
		{
			_searchTeamIDs = searchTeamID;
		}

		public IEnumerable<Pose_sports_statistics.Models.FootballFixture> GetH2HFixtures()
		{
			if (_searchTeamIDs == null || _searchTeamIDs.Length < 2)
				return null;

			var fixtures = Singleton.Get<RedisCacheManager>()
				.Get<IList<WebFormModel.FootballFixture>>
				(
					() => RequestLoader.FootballH2HFixtureByTeamID(_searchTeamIDs[0], _searchTeamIDs[1]),
					RequestLoader.Locker_FootballH2HFixtureByTeamID,
					DateTime.Now.AddHours(12),
					RedisKeyMaker.FootballH2HFixtureByTeamID(_searchTeamIDs[0], _searchTeamIDs[1])
				);

			return fixtures?.Where(elem => elem.EventDate > DateTime.Now.AddYears(-3) && elem.EventDate < DateTime.Now)
				.OrderByDescending(elem => elem.EventDate);
		}

		protected void GV_h2hFixtures_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var dataItem = e.Row.DataItem as WebFormModel.FootballFixture;
				if (dataItem != null)
				{
					e.Row.Cells[2].Text = $"{dataItem.HomeTeam.TeamName}&nbsp&nbsp";
					e.Row.Cells[4].Text = $"{dataItem.AwayTeam.TeamName}&nbsp&nbsp";

					var isHomeTeam = _searchTeamIDs[0] == dataItem.HomeTeam.TeamID;
					if (isHomeTeam)
					{
						e.Row.Cells[2].Font.Bold = true;
						e.Row.BackColor = Color.LemonChiffon;
					}

					var isAwayTeam = _searchTeamIDs[0] == dataItem.AwayTeam.TeamID;
					if (isAwayTeam)
					{
						e.Row.Cells[4].Font.Bold = true;
					}

					//결과
					string[] scores = dataItem.Score.FullTime?.Split('-');
					if (scores != null)
					{
						if (scores.Length > 1)
						{
							int homeScore = int.Parse(string.IsNullOrEmpty(scores[0]) ? "0" : scores[0]);
							int awayScore = int.Parse(string.IsNullOrEmpty(scores[1]) ? "0" : scores[1]);
							if ((isHomeTeam && homeScore > awayScore)
								|| (isAwayTeam && homeScore < awayScore))
							{
								e.Row.Cells[5].Text = "W";
								e.Row.Cells[5].Font.Bold = true;
								e.Row.Cells[5].ForeColor = Color.Green;
							}
							else if ((isHomeTeam && homeScore < awayScore)
								|| (isAwayTeam && homeScore > awayScore))
							{
								e.Row.Cells[5].Text = "L";
								e.Row.Cells[5].Font.Bold = true;
								e.Row.Cells[5].ForeColor = Color.Red;
							}
							else
							{
								e.Row.Cells[5].Text = "D";
								e.Row.Cells[5].Font.Bold = true;
								e.Row.Cells[5].ForeColor = Color.Orange;
							}
						}
					}
				}
			}
		}
	}
}