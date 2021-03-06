﻿using PosePacket.Service.Football;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SportsWebService.Services.Contract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFootball" in both code and config file together.
    [ServiceContract(Name = "Football", SessionMode = SessionMode.Allowed)]
    public interface IFootball
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetFixturesByDate")]
        Stream P_GET_FIXTURES_BY_DATE(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetFixturesByIndex")]
        Stream P_GET_FIXTURES_BY_INDEX(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetFixturesByLeague")]
        Stream P_GET_FIXTURES_BY_LEAGUE(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetFixturesByTeam")]
        Stream P_GET_FIXTURES_BY_TEAM(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetMatchOverview")]
        Stream P_GET_MATCH_OVERVIEW(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetMatchH2H")]
        Stream P_GET_MATCH_H2H(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetLeagueOverview")]
        Stream P_GET_LEAGUE_OVERVIEW(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetTeamOverview")]
        Stream P_GET_TEAM_OVERVIEW(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetMatchOdds")]
        Stream P_GET_MATCH_ODDS(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetMatchPredictions")]
        Stream P_GET_MATCH_PREDICTIONS(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetMatchVip")]
        Stream P_GET_MATCH_VIP();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetMatchVipHistory")]
        Stream P_GET_MATCH_VIP_HISTORY();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "DeleteVIPPick")]
        Stream P_DELETE_VIP_PICK(Stream stream);

        #region For Benchmark

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "GetFixturesByDate-Json"
        //    , RequestFormat = WebMessageFormat.Json
        //    , ResponseFormat = WebMessageFormat.Json)]
        //O_GET_FIXTURES_BY_DATE P_GET_FIXTURES_BY_DATE_JSON(string i_json);

        //[WebInvoke(Method = "POST", UriTemplate = "GetFixturesByDate-MessagePack")]
        //Stream P_GET_FIXTURES_BY_DATE_MessagePack(Stream stream);

        #endregion For Benchmark
    }
}