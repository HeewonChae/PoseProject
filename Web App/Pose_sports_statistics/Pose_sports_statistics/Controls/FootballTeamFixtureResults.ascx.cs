using LogicCore.Utility;
using Pose_sports_statistics.Logic.RapidAPI;
using Repository.Data.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

using WebFormModel = Pose_sports_statistics.Models;

namespace Pose_sports_statistics.Controls
{
    public partial class FootballTeamFixtureList : System.Web.UI.UserControl
    {
        private int _searchLeagueID;
        private int[] _searchTeamIDs;
        private string[] _searchTeamNames;
        private IList<WebFormModel.FootballStandings> _leagueStanding;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            CreateFixtureListView();
        }

        public void SetSearchTeamID(params int[] searchTeamID)
        {
            _searchTeamIDs = searchTeamID;
        }

        public void SetSearchTeamName(params string[] searchTeamName)
        {
            _searchTeamNames = searchTeamName;
        }

        public void SetSearchLeagueID(int searchLeagueID)
        {
            _searchLeagueID = searchLeagueID;
        }

        private void CreateFixtureListView()
        {
            if (_searchTeamIDs == null
                || _searchTeamNames == null
                || _searchTeamIDs.Length != _searchTeamNames.Length)
                return;

            // 텝 셋팅
            for (int i = 0; i < _searchTeamIDs.Length; i++)
            {
                tabContainer.Items.Add(new MenuItem($"&nbsp{_searchTeamNames[i].ToString()}&nbsp", i.ToString()));
            }

            // set standing
            _leagueStanding = Singleton.Get<RedisCacheManager>()
                .Get<IList<WebFormModel.FootballStandings>>
                (
                    () => RequestLoader.FootballStandingsByLeagueId(_searchLeagueID),
                    RequestLoader.Locker_FootballStandingsByLeagueId,
                    DateTime.Now.AddHours(1),
                    RedisKeyMaker.FootballStandingsByLeagueId(_searchLeagueID)
                );

            // 그리드 아이템 셋팅
            BindData(_searchTeamIDs);
        }

        private void BindData(params int[] searchTeamIDs)
        {
            // view1
            if (searchTeamIDs.Length > 0)
            {
                var homefixtures = Singleton.Get<RedisCacheManager>()
                .Get<IList<WebFormModel.FootballFixture>>
                (
                    () => RequestLoader.FootballFixtureByTeamId(_searchTeamIDs[0]),
                    RequestLoader.Locker_FootballFixtureByTeamId,
                    DateTime.Now.AddHours(12),
                    RedisKeyMaker.FootballFixtureByTeamId(_searchTeamIDs[0])
                );

                // 지난 경기
                var homeLateFixtures = homefixtures.Where(elem => elem.MatchTime < DateTime.Now)
                                                .OrderByDescending(elem => elem.MatchTime)
                                                .Take(20);

                if (homeLateFixtures?.Count() > 0)
                {
                    gv_homeLateFixtures.DataSource = homeLateFixtures;
                    gv_homeLateFixtures.DataBind();
                }

                // 예정된 경기
                var homeReserveFixtures = homefixtures.Where(elem => elem.MatchTime > DateTime.Now)
                                                    .Take(10);
                if (homeReserveFixtures?.Count() > 0)
                {
                    gv_homeReserveFixtures.DataSource = homeReserveFixtures;
                    gv_homeReserveFixtures.DataBind();
                }
            }

            //view2
            if (searchTeamIDs.Length > 1)
            {
                var awayfixtures = Singleton.Get<RedisCacheManager>()
                .Get<IList<WebFormModel.FootballFixture>>
                (
                    () => RequestLoader.FootballFixtureByTeamId(_searchTeamIDs[1]),
                    RequestLoader.Locker_FootballFixtureByTeamId,
                    DateTime.Now.AddHours(12),
                    RedisKeyMaker.FootballFixtureByTeamId(_searchTeamIDs[1])
                );

                // 지난 경기
                var awayLateFixtures = awayfixtures.Where(elem => elem.MatchTime < DateTime.Now)
                                                .OrderByDescending(elem => elem.MatchTime)
                                                .Take(20);

                if (awayLateFixtures?.Count() > 0)
                {
                    gv_awayLateFixtures.DataSource = awayLateFixtures;
                    gv_awayLateFixtures.DataBind();
                }

                // 예정된 경기
                var awayReserveFixtures = awayfixtures.Where(elem => elem.MatchTime > DateTime.Now)
                                                    .Take(10);
                if (awayReserveFixtures?.Count() > 0)
                {
                    gv_awayReserveFixtures.DataSource = awayReserveFixtures;
                    gv_awayReserveFixtures.DataBind();
                }
            }
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            var tabMenu = sender as Menu;
            mview_Teamfixtures.ActiveViewIndex = int.Parse(tabMenu.SelectedValue);
        }

        protected void GV_HomeFixtures_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as WebFormModel.FootballFixture;
                if (dataItem != null)
                {
                    e.Row.Cells[0].Text = $"{dataItem.League.Name}&nbsp&nbsp";
                    e.Row.Cells[1].Text = $"{dataItem.MatchTime.ToString("yyyy-MM-dd")}&nbsp&nbsp";

                    e.Row.Cells[3].Text = $"{dataItem.Score.FullTime}";

                    var homeStandingInfo = _leagueStanding?.Where(elem => elem.TeamId == dataItem.HomeTeam.TeamId).FirstOrDefault();
                    var awayStandingInfo = _leagueStanding?.Where(elem => elem.TeamId == dataItem.AwayTeam.TeamId).FirstOrDefault();

                    if (homeStandingInfo != null && awayStandingInfo != null)
                    {
                        e.Row.Cells[2].Text = $"&nbsp{dataItem.HomeTeam.TeamName}&nbsp{homeStandingInfo?.Rank}&nbsp({homeStandingInfo?.Points})&nbsp&nbsp&nbsp";
                        e.Row.Cells[4].Text = $"&nbsp&nbsp&nbsp{dataItem.AwayTeam.TeamName}&nbsp{awayStandingInfo?.Rank}&nbsp({awayStandingInfo?.Points})&nbsp";
                    }
                    else
                    {
                        e.Row.Cells[2].Text = $"&nbsp{dataItem.HomeTeam.TeamName}&nbsp&nbsp";
                        e.Row.Cells[4].Text = $"&nbsp&nbsp{dataItem.AwayTeam.TeamName}&nbsp";
                    }

                    var isHomeTeam = _searchTeamIDs[0] == dataItem.HomeTeam.TeamId;
                    if (isHomeTeam && dataItem.LeagueId == _searchLeagueID)
                    {
                        e.Row.Cells[2].Font.Bold = true;
                        e.Row.BackColor = Color.LemonChiffon;
                    }

                    var isAwayTeam = _searchTeamIDs[0] == dataItem.AwayTeam.TeamId;
                    if (isAwayTeam && dataItem.LeagueId == _searchLeagueID)
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

        protected void GV_AwayFixtures_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as WebFormModel.FootballFixture;
                if (dataItem != null)
                {
                    e.Row.Cells[0].Text = $"&nbsp{dataItem.League.Name}&nbsp";
                    e.Row.Cells[1].Text = $"&nbsp{dataItem.MatchTime.ToString("yyyy-MM-dd")}&nbsp";

                    e.Row.Cells[3].Text = $"{dataItem.Score.FullTime}";

                    var homeStandingInfo = _leagueStanding?.Where(elem => elem.TeamId == dataItem.HomeTeam.TeamId).FirstOrDefault();
                    var awayStandingInfo = _leagueStanding?.Where(elem => elem.TeamId == dataItem.AwayTeam.TeamId).FirstOrDefault();

                    if (homeStandingInfo != null && awayStandingInfo != null)
                    {
                        e.Row.Cells[2].Text = $"&nbsp{dataItem.HomeTeam.TeamName}&nbsp{homeStandingInfo?.Rank}&nbsp({homeStandingInfo?.Points})&nbsp&nbsp&nbsp";
                        e.Row.Cells[4].Text = $"&nbsp&nbsp&nbsp{dataItem.AwayTeam.TeamName}&nbsp{awayStandingInfo?.Rank}&nbsp({awayStandingInfo?.Points})&nbsp";
                    }
                    else
                    {
                        e.Row.Cells[2].Text = $"&nbsp{dataItem.HomeTeam.TeamName}&nbsp&nbsp";
                        e.Row.Cells[4].Text = $"&nbsp&nbsp{dataItem.AwayTeam.TeamName}&nbsp";
                    }

                    var isHomeTeam = _searchTeamIDs[1] == dataItem.HomeTeam.TeamId;
                    if (isHomeTeam && dataItem.LeagueId == _searchLeagueID)
                    {
                        e.Row.Cells[2].Font.Bold = true;
                    }

                    var isAwayTeam = _searchTeamIDs[1] == dataItem.AwayTeam.TeamId;
                    if (isAwayTeam && dataItem.LeagueId == _searchLeagueID)
                    {
                        e.Row.Cells[4].Font.Bold = true;
                        e.Row.BackColor = Color.LemonChiffon;
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

        // 홈팀 예정된 경기
        protected void GV_HomeReserveFixtures_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as WebFormModel.FootballFixture;
                if (dataItem != null)
                {
                    e.Row.Cells[0].Text = $"&nbsp{dataItem.League.Name}&nbsp";
                    e.Row.Cells[1].Text = $"&nbsp{dataItem.MatchTime.ToString("yyyy-MM-dd")}&nbsp";

                    var homeStandingInfo = _leagueStanding?.Where(elem => elem.TeamId == dataItem.HomeTeam.TeamId).FirstOrDefault();
                    var awayStandingInfo = _leagueStanding?.Where(elem => elem.TeamId == dataItem.AwayTeam.TeamId).FirstOrDefault();

                    if (homeStandingInfo != null)
                        e.Row.Cells[2].Text = $"&nbsp{dataItem.HomeTeam.TeamName}&nbsp{homeStandingInfo?.Rank}&nbsp({homeStandingInfo?.Points})&nbsp";
                    else
                        e.Row.Cells[2].Text = $"&nbsp{dataItem.HomeTeam.TeamName}&nbsp";

                    if (awayStandingInfo != null)
                        e.Row.Cells[3].Text = $"&nbsp{dataItem.AwayTeam.TeamName}&nbsp{awayStandingInfo?.Rank}&nbsp({awayStandingInfo?.Points})&nbsp";
                    else
                        e.Row.Cells[3].Text = $"&nbsp{dataItem.AwayTeam.TeamName}&nbsp";

                    e.Row.Cells[4].Text = $"&nbsp{(dataItem.MatchTime - DateTime.Now).Days}일 후&nbsp";
                }
            }
        }

        //	어웨이팀 예정된 경기
        protected void GV_AwayReserveFixtures_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as WebFormModel.FootballFixture;
                if (dataItem != null)
                {
                    e.Row.Cells[0].Text = $"&nbsp{dataItem.League.Name}&nbsp";
                    e.Row.Cells[1].Text = $"&nbsp{dataItem.MatchTime.ToString("yyyy-MM-dd")}&nbsp";

                    var homeStandingInfo = _leagueStanding?.Where(elem => elem.TeamId == dataItem.HomeTeam.TeamId).FirstOrDefault();
                    var awayStandingInfo = _leagueStanding?.Where(elem => elem.TeamId == dataItem.AwayTeam.TeamId).FirstOrDefault();

                    if (homeStandingInfo != null)
                        e.Row.Cells[2].Text = $"&nbsp{dataItem.HomeTeam.TeamName}&nbsp{homeStandingInfo?.Rank}&nbsp({homeStandingInfo?.Points})&nbsp";
                    else
                        e.Row.Cells[2].Text = $"&nbsp{dataItem.HomeTeam.TeamName}&nbsp";

                    if (awayStandingInfo != null)
                        e.Row.Cells[3].Text = $"&nbsp{dataItem.AwayTeam.TeamName}&nbsp{awayStandingInfo?.Rank}&nbsp({awayStandingInfo?.Points})&nbsp";
                    else
                        e.Row.Cells[3].Text = $"&nbsp{dataItem.AwayTeam.TeamName}&nbsp";

                    e.Row.Cells[4].Text = $"&nbsp{(dataItem.MatchTime - DateTime.Now).Days}일 후&nbsp";
                }
            }
        }
    }
}