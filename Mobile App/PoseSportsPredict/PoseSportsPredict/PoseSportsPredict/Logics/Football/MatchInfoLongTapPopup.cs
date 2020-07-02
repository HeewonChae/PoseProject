using Plugin.Vibrate;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
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
                        await bookmarkService.RemoveBookmark<FootballMatchInfo>(matchInfo, SportsType.Football, BookMarkType.Match);
                    else
                        await bookmarkService.AddBookmark<FootballMatchInfo>(matchInfo, SportsType.Football, BookMarkType.Match);
                    break;

                case 1: // league
                    if (leagueInfo.IsBookmarked)
                        await bookmarkService.RemoveBookmark<FootballLeagueInfo>(leagueInfo, SportsType.Football, BookMarkType.League);
                    else
                        await bookmarkService.AddBookmark<FootballLeagueInfo>(leagueInfo, SportsType.Football, BookMarkType.League);
                    break;

                case 2: // homeTeam
                    if (homeTeamInfo.IsBookmarked)
                        await bookmarkService.RemoveBookmark<FootballTeamInfo>(homeTeamInfo, SportsType.Football, BookMarkType.Team);
                    else
                        await bookmarkService.AddBookmark<FootballTeamInfo>(homeTeamInfo, SportsType.Football, BookMarkType.Team);
                    break;

                case 3: // awayTeam
                    if (awayTeamInfo.IsBookmarked)
                        await bookmarkService.RemoveBookmark<FootballTeamInfo>(awayTeamInfo, SportsType.Football, BookMarkType.Team);
                    else
                        await bookmarkService.AddBookmark<FootballTeamInfo>(awayTeamInfo, SportsType.Football, BookMarkType.Team);
                    break;

                default:
                    break;
            }
        }
    }
}