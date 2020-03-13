using PosePacket.Service.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SportsWebService.Services.Contract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFootball" in both code and config file together.
    [ServiceContract(Name = "Football")]
    public interface IFootball
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetFixturesByDate"
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json)]
        O_GET_FIXTURES_BY_DATE P_GET_FIXTURES_BY_DATE(string i_json);
    }
}