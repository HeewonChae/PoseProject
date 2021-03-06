﻿using LogicCore.Utility;
using Pose_sports_statistics.Logic.RapidAPI;
using Repository.Data.Redis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

using WebFormModel = Pose_sports_statistics.Models;

namespace Pose_sports_statistics.Controls
{
    public partial class FootballStanding : System.Web.UI.UserControl
    {
        private int[] _searchTeamIDs;
        private int _searchLeagueID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
        }

        public void SetSearchTeamID(params int[] searchTeamID)
        {
            _searchTeamIDs = searchTeamID;
        }

        public void SetSearchLeagueID(int searchLeagueID)
        {
            _searchLeagueID = searchLeagueID;
        }

        public IEnumerable<WebFormModel.FootballStandings> GetStandings()
        {
            if (_searchLeagueID == 0 || _searchTeamIDs == null || _searchTeamIDs.Length < 2)
                return null;

            var standings = Singleton.Get<RedisCacheManager>()
                .Get<IList<WebFormModel.FootballStandings>>
                (
                    () => RequestLoader.FootballStandingsByLeagueId(_searchLeagueID),
                    RequestLoader.Locker_FootballStandingsByLeagueId,
                    DateTime.Now.AddHours(1),
                    RedisKeyMaker.FootballStandingsByLeagueId(_searchLeagueID)
                );

            return standings;
        }

        protected void GV_Standings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as WebFormModel.FootballStandings;
                if (dataItem != null)
                {
                    var isSearchTeam = _searchTeamIDs.Any(elem => elem == dataItem.TeamId);
                    if (isSearchTeam)
                    {
                        e.Row.BackColor = Color.LemonChiffon;
                    }

                    if (dataItem.Forme != null)
                    {
                        for (int i = 0; i < dataItem.Forme.Length; i++)
                        {
                            var label = e.Row.Cells[10].FindControl($"form_{i}") as Label;
                            label.Text = dataItem.Forme[dataItem.Forme.Length - 1 - i].ToString();

                            if (label.Text.Equals("W"))
                                label.ForeColor = Color.Green;
                            else if (label.Text.Equals("L"))
                                label.ForeColor = Color.Red;
                            else
                                label.ForeColor = Color.Orange;
                        }
                    }
                }
            }
        }
    }
}