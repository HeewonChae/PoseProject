using PosePacket.Service.Football;
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
        [WebInvoke(Method = "POST", UriTemplate = "GetMatchOverview")]
        Stream P_GET_MATCH_OVERVIEW(Stream stream);

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