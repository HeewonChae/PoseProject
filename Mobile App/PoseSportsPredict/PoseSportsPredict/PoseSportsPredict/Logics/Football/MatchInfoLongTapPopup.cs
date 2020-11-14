using Plugin.Vibrate;
using PosePacket.Proxy;
using PosePacket.Service.Enum;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.Logics.Football
{
    public static class MatchInfoLongTapPopup
    {
        public static async void Execute(FootballMatchInfo matchInfo)
        {
            CrossVibrate.Current.Vibration(new TimeSpan(0, 0, 0, 0, 100));

            var leagueInfo = ShinyHost.Resolve<MatchInfoToLeagueInfo>().Convert(matchInfo);
            var homeTeamInfo = ShinyHost.Resolve<MatchInfoToTeamInfo>().Convert(matchInfo, TeamCampType.Home);
            var awayTeamInfo = ShinyHost.Resolve<MatchInfoToTeamInfo>().Convert(matchInfo, TeamCampType.Away);

            IBookmarkService bookmarkService = ShinyHost.Resolve<IBookmarkService>();

            var bookmarkedMatch = await bookmarkService.GetBookmark<FootballMatchInfo>(matchInfo.PrimaryKey);
            matchInfo.IsBookmarked = bookmarkedMatch?.IsBookmarked ?? false;

            var bookmarkedLeague = await bookmarkService.GetBookmark<FootballLeagueInfo>(leagueInfo.PrimaryKey);
            leagueInfo.IsBookmarked = bookmarkedLeague?.IsBookmarked ?? false;

            var bookmarkedHomeTeam = await bookmarkService.GetBookmark<FootballTeamInfo>(homeTeamInfo.PrimaryKey);
            homeTeamInfo.IsBookmarked = bookmarkedHomeTeam?.IsBookmarked ?? false;

            var bookmarkedAwayTeam = await bookmarkService.GetBookmark<FootballTeamInfo>(awayTeamInfo.PrimaryKey);
            awayTeamInfo.IsBookmarked = bookmarkedAwayTeam?.IsBookmarked ?? false;

            string strMatchBookmark = matchInfo.IsBookmarked ? LocalizeString.Delete_Match : LocalizeString.Add_Match;
            string strLeagueBookmark = leagueInfo.IsBookmarked ? LocalizeString.Delete_League : LocalizeString.Add_League;
            string strHomeTeamBookmark = homeTeamInfo.IsBookmarked ? LocalizeString.Delete_Home_Team : LocalizeString.Add_Home_Team;
            string strAwayTeamBookmark = awayTeamInfo.IsBookmarked ? LocalizeString.Delete_Away_Team : LocalizeString.Add_Away_Team;

            var actions = new string[]
            {
                strMatchBookmark,
                strLeagueBookmark,
                strHomeTeamBookmark,
                strAwayTeamBookmark,
            };

            int intResult = await MaterialDialog.Instance.SelectActionAsync(LocalizeString.Bookmark_Management, actions, DialogConfiguration.DefaultSimpleDialogConfiguration);
            if (intResult == -1)
                return;

            switch (intResult)
            {
                case 0: // match
                    if (matchInfo.IsBookmarked)
                        await bookmarkService.RemoveBookmark<FootballMatchInfo>(matchInfo, SportsType.Football, PageDetailType.Match);
                    else
                        await bookmarkService.AddBookmark<FootballMatchInfo>(matchInfo, SportsType.Football, PageDetailType.Match);
                    break;

                case 1: // league
                    if (leagueInfo.IsBookmarked)
                        await bookmarkService.RemoveBookmark<FootballLeagueInfo>(leagueInfo, SportsType.Football, PageDetailType.League);
                    else
                        await bookmarkService.AddBookmark<FootballLeagueInfo>(leagueInfo, SportsType.Football, PageDetailType.League);
                    break;

                case 2: // homeTeam
                    if (homeTeamInfo.IsBookmarked)
                        await bookmarkService.RemoveBookmark<FootballTeamInfo>(homeTeamInfo, SportsType.Football, PageDetailType.Team);
                    else
                        await bookmarkService.AddBookmark<FootballTeamInfo>(homeTeamInfo, SportsType.Football, PageDetailType.Team);
                    break;

                case 3: // awayTeam
                    if (awayTeamInfo.IsBookmarked)
                        await bookmarkService.RemoveBookmark<FootballTeamInfo>(awayTeamInfo, SportsType.Football, PageDetailType.Team);
                    else
                        await bookmarkService.AddBookmark<FootballTeamInfo>(awayTeamInfo, SportsType.Football, PageDetailType.Team);
                    break;

                default:
                    break;
            }
        }

        public static async Task<bool> Admin_DeletePick(FootballVIPMatchInfo matchInfo)
        {
            var actions = matchInfo.PredictionPicks.Select(elem => elem.Title).ToArray();

            int intResult = await MaterialDialog.Instance.SelectActionAsync("추천픽 관리", actions, DialogConfiguration.DefaultSimpleDialogConfiguration);
            if (intResult == -1)
                return false;

            var selectedPick = matchInfo.PredictionPicks[intResult];

            // Server Request
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            var server_result = await webApiService.RequestAsyncWithToken<O_DELETE_VIP_PICK>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_DELETE_VIP_PICK,
                PostData = new I_DELETE_VIP_PICK
                {
                    FixtureId = matchInfo.Id,
                    MainLabel = selectedPick.MainLabel,
                    SubLabel = selectedPick.SubLabel,
                }
            });

            if (server_result == null)
                return false;

            if (server_result.IsSuccess)
            {
                matchInfo.PredictionPicks.RemoveAt(intResult);
            }

            return server_result.IsSuccess;
        }
    }
}