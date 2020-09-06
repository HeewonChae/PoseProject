using MessagePack;
using PosePacket.Service.Football.Models;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.ViewModels.Football.Team;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.Logics
{
    public static class PageUriLinker
    {
        public static PageDetailType PageType { get; set; }
        public static byte[] QueryData { get; set; }
        public static bool IsExistQueryData => QueryData != null;

        public static void FromUriQuery(string query)
        {
            var uriDatas = ParseQuery(query);
            if (uriDatas.Length != 2)
                return;

            uriDatas[0].TryParseEnum(out PageDetailType pageType);

            try
            {
                PageType = pageType;
                QueryData = Convert.FromBase64String(uriDatas[1]);
            }
            catch
            {
                QueryData = null;
            }
        }

        public static string MakePageUrl(PageDetailType PageType, object data)
        {
            var baseUrl = AppConfig.PAGE_BASE_LINK;

            object reversed;
            switch (data)
            {
                case FootballMatchInfo matchInfo:
                    reversed = ShinyHost.Resolve<FixtureDetailToMatchInfo>().Reverse(matchInfo);
                    break;

                case FootballLeagueInfo leagueInfo:
                    reversed = ShinyHost.Resolve<LeagueDetailToLeagueInfo>().Reverse(leagueInfo);
                    break;

                case FootballTeamInfo teamInfo:
                    reversed = ShinyHost.Resolve<TeamDetailToTeamInfo>().Reverse(teamInfo);
                    break;

                default:
                    return null;
            }

            var serializeData = MessagePackSerializer.Serialize(reversed);
            var dataString = Convert.ToBase64String(serializeData);

            return $"{baseUrl}?page={PageType}&data={dataString}";
        }

        public static async Task GoUrlLinkedPage()
        {
            if (!IsExistQueryData)
                return;

            try
            {
                switch (PageType)
                {
                    case PageDetailType.Match:
                        {
                            var converted = MessagePackSerializer.Deserialize<FootballFixtureDetail>(QueryData);
                            var matchInfo = ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(converted);
                            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);
                        }
                        break;

                    case PageDetailType.League:
                        {
                            var converted = MessagePackSerializer.Deserialize<FootballLeagueDetail>(QueryData);
                            var leagueInfo = ShinyHost.Resolve<LeagueDetailToLeagueInfo>().Convert(converted);
                            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), leagueInfo);
                        }
                        break;

                    case PageDetailType.Team:
                        {
                            var converted = MessagePackSerializer.Deserialize<FootballTeamDetail>(QueryData);
                            var teamInfo = ShinyHost.Resolve<TeamDetailToTeamInfo>().Convert(converted);
                            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>(), teamInfo);
                        }
                        break;

                    default:
                        {
                            QueryData = null;
                            return;
                        }
                }
            }
            catch
            {
                QueryData = null;
                return;
            }

            QueryData = null;
        }

        private static string[] ParseQuery(string query)
        {
            List<string> datas = new List<string>();

            var pageAttrIdx = query.IndexOf("?page=");
            var andPercendIdx = query.IndexOf('&');
            var pageTypeStartIdx = pageAttrIdx + 6;
            var pageType = query.Substring(pageTypeStartIdx, andPercendIdx - pageTypeStartIdx);
            datas.Add(pageType);

            var dataAttrIdx = query.IndexOf("&data=");
            var dataStartIdx = dataAttrIdx + 6;
            var data = query.Substring(dataStartIdx);
            datas.Add(data);

            return datas.ToArray();
        }
    }
}