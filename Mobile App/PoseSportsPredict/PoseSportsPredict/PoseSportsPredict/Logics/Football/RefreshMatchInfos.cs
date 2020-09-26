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

namespace PoseSportsPredict.Logics.Football
{
    public static class RefreshMatchInfos
    {
        public static async Task<IReadOnlyCollection<FootballMatchInfo>> Execute(int[] indexes)
        {
            try
            {
                if (indexes == null || indexes.Length == 0)
                    return new FootballMatchInfo[0];

                var webApiService = ShinyHost.Resolve<IWebApiService>();
                var bookmarkService = ShinyHost.Resolve<IBookmarkService>();
                var notificationService = ShinyHost.Resolve<INotificationService>();

                // call server
                var server_result = await webApiService.RequestAsyncWithToken<O_GET_FIXTURES_BY_INDEX>(new WebRequestContext
                {
                    SerializeType = SerializeType.MessagePack,
                    MethodType = WebMethodType.POST,
                    BaseUrl = AppConfig.PoseWebBaseUrl,
                    ServiceUrl = FootballProxy.ServiceUrl,
                    SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_INDEX,
                    PostData = new I_GET_FIXTURES_BY_INDEX
                    {
                        FixtureIds = indexes,
                    }
                });

                if (server_result == null)
                    return new FootballMatchInfo[0];

                var bookmarkedMatches = await bookmarkService.GetAllBookmark<FootballMatchInfo>();
                var notifications = await notificationService.GetAllNotification(SportsType.Football, NotificationType.MatchStart);

                var matchList = new List<FootballMatchInfo>();
                foreach (var fixture in server_result.Fixtures)
                {
                    var convertedMatchInfo = ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(fixture);

                    var bookmarkedMatch = bookmarkedMatches.FirstOrDefault(elem => elem.PrimaryKey == convertedMatchInfo.PrimaryKey);
                    var notifiedMatch = notifications.FirstOrDefault(elem => elem.Id == convertedMatchInfo.Id);

                    convertedMatchInfo.IsBookmarked = bookmarkedMatch?.IsBookmarked ?? false;
                    convertedMatchInfo.IsAlarmed = notifiedMatch != null ? true : false;

                    matchList.Add(convertedMatchInfo);
                }

                return matchList;
            }
            catch
            {
                return new FootballMatchInfo[0];
            }
        }
    }
}