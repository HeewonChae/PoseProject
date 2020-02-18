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
	public partial class FootballPlayerList : System.Web.UI.UserControl
	{
		private string _searchSeason;
		private int[] _searchTeamIDs;
		private string[] _searchTeamNames;
		private int _searchLeagID;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
				return;

			CreatePlayerListView();

			// databind top scorer 
			gv_topScorer.DataSource = Singleton.Get<RedisCacheManager>()
				.Get<IList<WebFormModel.FootballPlayer>>
				(
					() => RequestLoader.FootballTopScorerByLeagueID(_searchLeagID),
					RequestLoader.Locker_FootballTopScorerByLeagueID,
					DateTime.Now.AddDays(1),
					RedisKeyMaker.FootballTopScorerByLeagueID(_searchLeagID)
				);
			gv_topScorer.DataBind();
		}

		public void SetSearchTeamID(params int[] searchTeamID)
		{
			_searchTeamIDs = searchTeamID;
		}

		public void SetSearchTeamName(params string[] searchTeamName)
		{
			_searchTeamNames = searchTeamName;
		}

		public void SetSearchSeason(string searchSeason)
		{
			_searchSeason = searchSeason;
		}

		public void SetSearchLeagueID(int searchLeagueID)
		{
			_searchLeagID = searchLeagueID;
		}

		private void CreatePlayerListView()
		{
			if (_searchTeamIDs == null
				|| _searchTeamNames == null
				|| _searchTeamIDs.Length != _searchTeamNames.Length)
				return;

			// 텝 셋팅
			int i = 0;
			for (; i < _searchTeamIDs.Length; i++)
			{
				this.menu_playerTeam.Items.Add(new MenuItem($"&nbsp{_searchTeamNames[i].ToString()}&nbsp", i.ToString()));
			}

			this.menu_playerTeam.Items.Add(new MenuItem($"&nbsp TopScorer &nbsp", i.ToString()));

			CreatePlayerListByLeague(_searchTeamIDs);
		}

		private void CreatePlayerListByLeague(params int[] searchTeamIDs)
		{
			// view1
			if (searchTeamIDs.Length > 0)
			{
				var homePlayers = Singleton.Get<RedisCacheManager>()
				.Get<IList<WebFormModel.FootballPlayer>>
				(
					() => RequestLoader.FootballPlayersByTeamIDAndSeason(searchTeamIDs[0], _searchSeason),
					RequestLoader.Locker_FootballPlayersByTeamIDAndSeason,
					DateTime.Now.AddDays(1),
					RedisKeyMaker.FootballPlayersByTeamIDAndSeason(searchTeamIDs[0], _searchSeason)
				).GroupBy(elem => elem.League);

				if (homePlayers?.Count() > 0)
				{
					BindPlayerByLeague(ref menu_homeLeagues, ref mview_homePlayers, ref homePlayers);

				}
			}

			//view2
			if (searchTeamIDs.Length > 1)
			{
				var awayPlayers = Singleton.Get<RedisCacheManager>()
				.Get<IList<WebFormModel.FootballPlayer>>
				(
					() => RequestLoader.FootballPlayersByTeamIDAndSeason(searchTeamIDs[1], _searchSeason),
					RequestLoader.Locker_FootballPlayersByTeamIDAndSeason,
					DateTime.Now.AddDays(1),
					RedisKeyMaker.FootballPlayersByTeamIDAndSeason(searchTeamIDs[1], _searchSeason)
				).GroupBy(elem => elem.League);

				if (awayPlayers?.Count() > 0)
				{
					BindPlayerByLeague(ref menu_awayLeagues, ref mview_awayPlayers, ref awayPlayers);
				}
			}
		}

		private void BindPlayerByLeague(ref Menu menu, ref MultiView mview, ref IEnumerable<IGrouping<string, WebFormModel.FootballPlayer>> players)
		{
			int value = 0;
			foreach (var playersByLeague in players)
			{
				menu.Items.Add(new MenuItem($"&nbsp{playersByLeague.Key}&nbsp", value.ToString()));

				var foundView = mview.Views[value];
				var foundGrid = foundView.Controls[1] as GridView;

				// field
				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Num",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Name",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Pos",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Age",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Height",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Rate",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Goal",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Assist",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Shot(%)",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Pass(%)",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Dribble(%)",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Lineup",
				});

				foundGrid.Columns.Add(new BoundField
				{
					HeaderText = "Injured",
					DataField = "Injured"
				});

				foundGrid.DataSource = playersByLeague.Select(elem => elem).OrderBy(elem => elem.Position);
				foundGrid.DataBind();

				value++;
			}
		}

		protected void Menu_playerTeamClick(object sender, MenuEventArgs e)
		{
			var tabMenu = sender as Menu;
			mview_playerList.ActiveViewIndex = int.Parse(tabMenu.SelectedValue);
		}

		protected void Menu_HomeLeaguesClick(object sender, MenuEventArgs e)
		{
			var tabMenu = sender as Menu;
			mview_homePlayers.ActiveViewIndex = int.Parse(tabMenu.SelectedValue);
		}

		protected void Menu_AwayLeaguesClick(object sender, MenuEventArgs e)
		{
			var tabMenu = sender as Menu;
			mview_awayPlayers.ActiveViewIndex = int.Parse(tabMenu.SelectedValue);
		}

		protected void GV_Players_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var dataItem = e.Row.DataItem as WebFormModel.FootballPlayer;
				if (dataItem != null)
				{
					e.Row.Cells[0].Text = dataItem.Number.ToString();
					e.Row.Cells[1].Text = $"{dataItem.PalyerName}&nbsp&nbsp";
					e.Row.Cells[2].Text = $"{dataItem.Position}&nbsp&nbsp";
					e.Row.Cells[3].Text = $"{dataItem.Age}";
					e.Row.Cells[4].Text = $"{dataItem.Height}&nbsp&nbsp&nbsp";
					e.Row.Cells[5].Text = $"{dataItem.Rating?.ToString("0.00")}&nbsp&nbsp&nbsp";

					if (dataItem.Rating >= 6.8)
					{
						e.Row.Font.Bold = true;
						if (dataItem.Rating >= 7.0)
							e.Row.BackColor = Color.LemonChiffon;
					}

					e.Row.Cells[6].Width = 50;
					e.Row.Cells[6].Text = $"{dataItem.Goals.Total}&nbsp&nbsp";

					e.Row.Cells[7].Width = 50;
					e.Row.Cells[7].Text = $"{dataItem.Goals.Assists}&nbsp&nbsp";

					e.Row.Cells[8].Width = 70;
					if (dataItem.Shots.Total > 0)
					{
						float shotspg = (dataItem.Shots.On / dataItem.Shots.Total) * 100.0f;
						e.Row.Cells[8].Text = $"{shotspg.ToString("0.00")}&nbsp&nbsp";
					}

					e.Row.Cells[9].Width = 70;
					e.Row.Cells[9].Text = $"{dataItem.Passes.Accuracy}&nbsp&nbsp";

					e.Row.Cells[10].Width = 80;
					if (dataItem.Dribbles.Attempts > 0)
					{
						float dribblepg = (dataItem.Dribbles.Success / dataItem.Dribbles.Attempts) * 100.0f;
						e.Row.Cells[10].Text = $"{dribblepg.ToString("0.00")}&nbsp&nbsp";
					}

					e.Row.Cells[11].Width = 70;
					e.Row.Cells[11].Text = $"{dataItem.Games.Lineups}&nbsp&nbsp";

					e.Row.Cells[12].Width = 50;
					e.Row.Cells[12].Text = $"{dataItem.Injured}&nbsp&nbsp";
				}
			}
		}

		protected void GV_TopScorer_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var dataItem = e.Row.DataItem as WebFormModel.FootballPlayer;

				if (dataItem.Shots.Total > 0)
				{
					float shotspg = (dataItem.Shots.On / dataItem.Shots.Total) * 100.0f;
					e.Row.Cells[5].Text = $"{shotspg.ToString("0.00")}&nbsp&nbsp";
				}

				// 홈팀선수
				if (_searchTeamIDs.Length > 0 && _searchTeamIDs[0] == dataItem.TeamID)
				{
					e.Row.BackColor = Color.PaleGreen;
				}
				else if (_searchTeamIDs.Length > 1 && _searchTeamIDs[1] == dataItem.TeamID)
				{
					e.Row.BackColor = Color.LightSalmon;
				}
			}
		}
	}
}