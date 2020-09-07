using PosePacket.Service.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SportsWebService.Services.Contract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Name = "Auth", SessionMode = SessionMode.Allowed)]
    public interface IAuth
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "server_check")]
        void P_CHECK_SERVER_STATE();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "PUBLISHKEY")]
        Stream P_PUBLISH_KEY();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "TokenRefresh")]
        Stream P_E_TokenRefresh();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "CheckVaildOAuthUser")]
        Task<Stream> P_E_CheckVaildOAuthUser(Stream e_stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Login")]
        Stream P_E_Login(Stream e_stream);
    }
}