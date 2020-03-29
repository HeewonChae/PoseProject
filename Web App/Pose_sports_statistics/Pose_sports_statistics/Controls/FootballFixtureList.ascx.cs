using LogicCore.Utility;
using Newtonsoft.Json;
using Pose_sports_statistics.Logic.RapidAPI;
using Repository.Data.Redis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;

using WebFormModel = Pose_sports_statistics.Models;

namespace Pose_sports_statistics.Controls
{
    public partial class FootballFixtureList : System.Web.UI.UserControl
    {
        public class DDL_NameValue
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            DDLBindData();
        }

        public void DDLBindData()
        {
            List<DDL_NameValue> ddlDataList = new List<DDL_NameValue>();
            ddlDataList.Add(new DDL_NameValue { Name = "Filtered by country" });

            var fixtures = Singleton.Get<RedisCacheManager>()
                .Get<IList<WebFormModel.FootballFixture>>
                (
                    () => RequestLoader.FootballFixturesByDate(DateTime.Now),
                    RequestLoader.Locker_FootballFixturesByDate,
                    DateTime.Now.AddHours(4),
                    RedisKeyMaker.FootballFixturesByDate(DateTime.Now)
                );

            // 종료된 경기 필터링
            var exclusive_finished = fixtures.Where(elem => elem.MatchTime > DateTime.Now.AddHours(-2));

            var group_query = exclusive_finished.GroupBy(elem => new { CountryName = elem.League.Country });

            var bindData = from queryData in group_query
                           select new DDL_NameValue
                           {
                               Name = queryData.Key.CountryName,
                               Value = queryData.Key.CountryName,
                           };

            ddlDataList.AddRange(bindData);

            ddl_kindCountry.DataTextField = "Name";
            ddl_kindCountry.DataValueField = "Value";
            ddl_kindCountry.DataSource = ddlDataList;
            ddl_kindCountry.DataBind();
        }

        public IQueryable<dynamic> GetFixtures([RouteData] string countryName)
        {
            var fixtures = Singleton.Get<RedisCacheManager>()
                .Get<IList<WebFormModel.FootballFixture>>
                (
                    () => RequestLoader.FootballFixturesByDate(DateTime.Now),
                    RequestLoader.Locker_FootballFixturesByDate,
                    DateTime.Now.AddHours(4),
                    RedisKeyMaker.FootballFixturesByDate(DateTime.Now)
                );

            // step1. 종료된 경기는 필터링
            IEnumerable<WebFormModel.FootballFixture> filteredByData;
            if (string.IsNullOrEmpty(countryName))
            {
                filteredByData = fixtures.Where(elem => elem.MatchTime > DateTime.Now.AddHours(-2));
            }
            else
            {
                filteredByData = fixtures.Where(elem => elem.MatchTime > DateTime.Now.AddHours(-2) && elem.League.Country.Equals(countryName));
            }

            // step2. 데이터를 리그, 시작시간으로 그룹화
            var group_query = filteredByData.GroupBy(elem => new { League = elem.LeagueId, StartTime = elem.MatchTime });

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

        protected void DDL_KindCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            var url = GetRouteUrl("FootballByCountry", new { CountryName = ddl_kindCountry.SelectedValue });
            Response.Redirect(url);
        }

        protected void GV_FootballFixtures_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as WebFormModel.FootballFixture;

                var chk_interestedFixture = e.Row.FindControl("chk_interestedFixture") as CheckBox;

                var homeHyperLink = e.Row.FindControl("hl_homeTeamLink") as HyperLink;
                var awayHyperLink = e.Row.FindControl("hl_awayTeamLink") as HyperLink;

                homeHyperLink.NavigateUrl = GetRouteUrl("FootballTeamById", new { TeamId = dataItem.AwayTeam.TeamId });
                awayHyperLink.NavigateUrl = GetRouteUrl("FootballTeamById", new { TeamId = dataItem.AwayTeam.TeamId });

                if (dataItem != null)
                {
                    // 관심경기 선택 여부

                    // 관심 경기 데이터 가져오기
                    var interestedFixtures = Singleton.Get<RedisCacheManager>()
                        .GetNullable<IList<WebFormModel.FootballFixture>>
                        (
                            RedisKeyMaker.FootballInterestedFixture()
                        );
                    bool isInterestedFixture = interestedFixtures?.Where(elem => elem.FixtureId == dataItem.FixtureId).FirstOrDefault() != null;
                    chk_interestedFixture.Checked = isInterestedFixture;

                    // 리그 5위 이상 팀 Bold체
                    var leaugeStandings = Singleton.Get<RedisCacheManager>()
                    .Get<IList<WebFormModel.FootballStandings>>
                    (
                        () => RequestLoader.FootballStandingsByLeagueId(dataItem.LeagueId),
                        RequestLoader.Locker_FootballStandingsByLeagueId,
                        DateTime.Now.AddHours(4),
                        RedisKeyMaker.FootballStandingsByLeagueId(dataItem.LeagueId)
                    );

                    var homeStandingInfo = leaugeStandings.Where(elem => elem.TeamId == dataItem.HomeTeam.TeamId).FirstOrDefault();
                    var awayStandingInfo = leaugeStandings.Where(elem => elem.TeamId == dataItem.AwayTeam.TeamId).FirstOrDefault();

                    if (homeStandingInfo?.Rank <= 5)
                    {
                        homeHyperLink.Font.Bold = true;
                    }

                    if (awayStandingInfo?.Rank <= 5)
                    {
                        awayHyperLink.Font.Bold = true;
                    }

                    // 팀 통계에 따른 폰트 색상
                    //var homeTeamStatistics = Singleton.Get<RedisCacheManager>()
                    //.Get<WebFormModel.FootballTeamStatistics>
                    //(
                    //	() => RequestLoader.FootballTeamStatisticsByLeagueIDAndTeamID(dataItem.LeagueID, dataItem.HomeTeam.TeamID),
                    //	RequestLoader.Locker_FootballTeamStatisticsByLeagueIDAndTeamID,
                    //	DateTime.Now.AddHours(12),
                    //	RedisKeyMaker.FootballTeamStatisticsByLeagueIDAndTeamID(dataItem.LeagueID, dataItem.HomeTeam.TeamID)
                    //);

                    //var awayTeamStatistics = Singleton.Get<RedisCacheManager>()
                    //.Get<WebFormModel.FootballTeamStatistics>
                    //(
                    //	() => RequestLoader.FootballTeamStatisticsByLeagueIDAndTeamID(dataItem.LeagueID, dataItem.AwayTeam.TeamID),
                    //	RequestLoader.Locker_FootballTeamStatisticsByLeagueIDAndTeamID,
                    //	DateTime.Now.AddHours(12),
                    //	RedisKeyMaker.FootballTeamStatisticsByLeagueIDAndTeamID(dataItem.LeagueID, dataItem.AwayTeam.TeamID)
                    //);

                    //CheckTeamStaticAndApplyColor(homeTeamStatistics, homeHyperLink);
                    //CheckTeamStaticAndApplyColor(awayTeamStatistics, awayHyperLink);

                    // 팀 최근 6경기 득실점에 따른 폰트 색상
                    //var homefixtures = Singleton.Get<RedisCacheManager>()
                    //	.Get<IList<WebFormModel.FootballFixture>>
                    //	(
                    //		() => RequestLoader.FootballFixtureByTeamID(dataItem.HomeTeam.TeamID),
                    //		RequestLoader.Locker_FootballFixtureByTeamID,
                    //		DateTime.Now.AddHours(12),
                    //		RedisKeyMaker.FootballFixtureByTeamID(dataItem.HomeTeam.TeamID)
                    //	).Where(elem => elem.EventDate < DateTime.Now)
                    //	.OrderByDescending(elem => elem.EventDate);

                    //// 홈팀 최근 6경기 득실점
                    //int fixtureCnt = 0;
                    //int goalCnt = 0;
                    //int goalAgainst = 0;
                    //foreach (var selectedfixture in homefixtures)
                    //{
                    //	if (string.IsNullOrEmpty(selectedfixture.Score.FullTime)
                    //		|| selectedfixture.LeagueID != dataItem.LeagueID)
                    //		continue;

                    //	// 최근 5경기만..
                    //	if (fixtureCnt >= 6)
                    //		break;

                    //	bool isHomeTeam = selectedfixture.HomeTeam.TeamID == dataItem.HomeTeam.TeamID;
                    //	string[] scoreSplit = selectedfixture.Score.FullTime.Split('-');

                    //	goalCnt += int.Parse(scoreSplit[isHomeTeam ? 0 : 1]);
                    //	goalAgainst += int.Parse(scoreSplit[isHomeTeam ? 1 : 0]);

                    //	fixtureCnt++;
                    //}

                    //CheckTeamGoalAgainstAndApplyColor(goalCnt, goalAgainst, homeHyperLink);

                    //var awayfixtures = Singleton.Get<RedisCacheManager>()
                    //	.Get<IList<WebFormModel.FootballFixture>>
                    //	(
                    //		() => RequestLoader.FootballFixtureByTeamID(dataItem.AwayTeam.TeamID),
                    //		RequestLoader.Locker_FootballFixtureByTeamID,
                    //		DateTime.Now.AddHours(12),
                    //		RedisKeyMaker.FootballFixtureByTeamID(dataItem.AwayTeam.TeamID)
                    //	).Where(elem => elem.EventDate < DateTime.Now)
                    //	.OrderByDescending(elem => elem.EventDate);

                    //// 어웨이팀 최근 6경기 득실점
                    //fixtureCnt = 0;
                    //goalCnt = 0;
                    //goalAgainst = 0;
                    //foreach (var selectedfixture in awayfixtures)
                    //{
                    //	if (string.IsNullOrEmpty(selectedfixture.Score.FullTime)
                    //		|| selectedfixture.LeagueID != dataItem.LeagueID)
                    //		continue;

                    //	// 최근 6경기만..
                    //	if (fixtureCnt >= 6)
                    //		break;

                    //	bool isHomeTeam = selectedfixture.HomeTeam.TeamID == dataItem.AwayTeam.TeamID;
                    //	string[] scoreSplit = selectedfixture.Score.FullTime.Split('-');

                    //	goalCnt += int.Parse(scoreSplit[isHomeTeam ? 0 : 1]);
                    //	goalAgainst += int.Parse(scoreSplit[isHomeTeam ? 1 : 0]);

                    //	fixtureCnt++;
                    //}

                    //CheckTeamGoalAgainstAndApplyColor(goalCnt, goalAgainst, awayHyperLink);

                    //경기 예측(단폴픽 체크)
                    var prediction = Singleton.Get<RedisCacheManager>()
                    .Get<IList<WebFormModel.FootballPrediction>>
                    (
                        () => RequestLoader.FootballPredictionByFixtureId(dataItem.FixtureId),
                        RequestLoader.Locker_FootballPredictionByFixtureId,
                        DateTime.Now.AddDays(1),
                        RedisKeyMaker.FootballPredictionByFixtureId(dataItem.FixtureId)
                    ).FirstOrDefault();

                    if (prediction?.MatchWinner?.Trim(' ').Length == 1)
                    {
                        e.Row.BackColor = Color.LightGreen;
                    }
                }
            }
        }

        private void CheckTeamStaticAndApplyColor(WebFormModel.FootballTeamStatistics teamStatic, HyperLink control)
        {
            // 평균 득점 1.9 초과
            if (teamStatic.GoalsAvg.GoalsFor.Total > 1.9f)
            {
                control.ForeColor = Color.Red;
            }
            // 평균 득점 1 미만
            else if (teamStatic.GoalsAvg.GoalsFor.Total < 1f)
            {
                control.ForeColor = Color.Blue;
            }

            // 평균 실점 1 미만
            if (teamStatic.GoalsAvg.GoalsAgainst.Total < 1f)
            {
                control.BackColor = Color.LightPink;
            }
            // 평균 실점 1.9 초과
            if (teamStatic.GoalsAvg.GoalsAgainst.Total > 1.9f)
            {
                control.BackColor = Color.LightSteelBlue;
            }
        }

        private void CheckTeamGoalAgainstAndApplyColor(int goal, int against, HyperLink control)
        {
            // 12골 이상
            if (goal > 12)
            {
                control.ForeColor = Color.Red;
            }
            // 6골 미만
            else if (goal < 6)
            {
                control.ForeColor = Color.Blue;
            }

            // 실점 6골 미만
            if (against < 6)
            {
                control.BackColor = Color.LightPink;
            }
            // 실점 12골 초과
            if (against > 12)
            {
                control.BackColor = Color.LightSteelBlue;
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

            // 관심경기에 추가하는 경우
            if (chkbox.Checked)
            {
                // 모든 경기 가져오기
                var fixtures = Singleton.Get<RedisCacheManager>()
                    .Get<IList<WebFormModel.FootballFixture>>
                    (
                        () => RequestLoader.FootballFixturesByDate(DateTime.Now),
                        RequestLoader.Locker_FootballFixturesByDate,
                        DateTime.Now.AddHours(4),
                        RedisKeyMaker.FootballFixturesByDate(DateTime.Now)
                    );

                // 관심 경기 검색
                int interestedFixtureID = int.Parse(chkbox.ToolTip);
                var interestedFixture = fixtures.Where(elem => elem.FixtureId == interestedFixtureID).FirstOrDefault();

                if (interestedFixture == null)
                    return;

                // 관심 경기 데이터 가져오기
                var interestedFixtures = Singleton.Get<RedisCacheManager>()
                    .GetNullable<IList<WebFormModel.FootballFixture>>
                    (
                        RedisKeyMaker.FootballInterestedFixture()
                    );

                if (interestedFixtures == null)
                    interestedFixtures = new List<WebFormModel.FootballFixture>();

                // 관심 경기에 추가
                interestedFixtures.Add(interestedFixture);

                // 레디스에 저장
                Singleton.Get<RedisCacheManager>().Set
                    (
                        JsonConvert.SerializeObject(interestedFixtures),
                        RequestLoader.Locker_FootballInterestedFixture,
                        new DateTime(9999, 12, 31),
                        RedisKeyMaker.FootballInterestedFixture()
                    );
            }
            else // 관심경기에서 제거하는 경우
            {
                // 관심 경기 데이터 가져오기
                var interestedFixtures = Singleton.Get<RedisCacheManager>()
                    .GetNullable<IList<WebFormModel.FootballFixture>>
                    (
                        RedisKeyMaker.FootballInterestedFixture()
                    );

                // 제거할 경기
                int interestedFixtureID = int.Parse(chkbox.ToolTip);
                var interestedFixture = interestedFixtures.Where(elem => elem.FixtureId == interestedFixtureID).FirstOrDefault();

                if (interestedFixture == null)
                    return; // 이미 제거된 경기

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
            }
        }
    }
}