using PosePacket.Proxy;
using PosePacket.Service.Enum;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.Services.Cache.Loader
{
    public static class FootballDataLoader
    {
        public static async Task<object> FixturesByDate(DateTime startTime, DateTime endTime)
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            return await webApiService.RequestRawAsyncWithToken(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_DATE,
                PostData = new I_GET_FIXTURES_BY_DATE
                {
                    StartTime = startTime,
                    EndTime = endTime,
                }
            });
        }

        #region Match

        public static async Task<object> MatchOverview(int fixtureId)
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            return await webApiService.RequestRawAsyncWithToken(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_MATCH_OVERVIEW,
                PostData = new I_GET_MATCH_OVERVIEW
                {
                    FixtureId = fixtureId,
                }
            });
        }

        public static async Task<object> MatchH2H(int fixtureId, short homeTeamId, short awayTeamInfo)
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            return await webApiService.RequestRawAsyncWithToken(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_MATCH_H2H,
                PostData = new I_GET_MATCH_H2H
                {
                    FixtureId = fixtureId,
                    HomeTeamId = homeTeamId,
                    AwayTeamId = awayTeamInfo,
                }
            });
        }

        public static async Task<object> MatchOdds(int fixtureId)
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            return await webApiService.RequestRawAsyncWithToken(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_MATCH_ODDS,
                PostData = new I_GET_MATCH_ODDS
                {
                    FixtureId = fixtureId,
                }
            });
        }

        public static async Task<object> MatchPredictions(int fixtureId)
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            return await webApiService.RequestRawAsyncWithToken(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_MATCH_PREDICTIONS,
                PostData = new I_GET_MATCH_PREDICTIONS
                {
                    FixtureId = fixtureId,
                }
            });
        }

        #endregion Match

        #region League

        public static async Task<object> FixturesByLeague(string coutry, string league, SearchFixtureStatusType searchFixtureStatusType)
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            return await webApiService.RequestRawAsyncWithToken(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_LEAGUE,
                PostData = new I_GET_FIXTURES_BY_LEAGUE
                {
                    SearchFixtureStatusType = searchFixtureStatusType,
                    CountryName = coutry,
                    LeagueName = league,
                }
            });
        }

        public static async Task<object> LeagueOverview(string coutry, string league)
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            return await webApiService.RequestRawAsyncWithToken(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_LEAGUE_OVERVIEW,
                PostData = new I_GET_LEAGUE_OVERVIEW
                {
                    CountryName = coutry,
                    LeagueName = league,
                }
            });
        }

        #endregion League

        #region Team

        public static async Task<object> FixturesByTeam(short teamId, SearchFixtureStatusType searchFixtureStatusType)
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            return await webApiService.RequestRawAsyncWithToken(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_TEAM,
                PostData = new I_GET_FIXTURES_BY_TEAM
                {
                    SearchFixtureStatusType = searchFixtureStatusType,
                    TeamId = teamId,
                }
            });
        }

        public static async Task<object> TeamOverview(short teamId)
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            return await webApiService.RequestRawAsyncWithToken(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_TEAM_OVERVIEW,
                PostData = new I_GET_TEAM_OVERVIEW
                {
                    TeamId = teamId,
                }
            });
        }

        #endregion Team
    }
}