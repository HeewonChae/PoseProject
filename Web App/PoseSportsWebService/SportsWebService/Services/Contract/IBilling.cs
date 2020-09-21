using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SportsWebService.Services.Contract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBilling" in both code and config file together.
    [ServiceContract(Name = "Billing", SessionMode = SessionMode.Allowed)]
    public interface IBilling
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "InsertInAppBillingByGoogle")]
        Stream P_E_INSERT_IN_APP_BILLING_BY_GOOGLE(Stream e_stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateInAppBillingByGoogle")]
        Task<Stream> P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE(Stream e_stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "CheckMembershipByGoogle")]
        Task<Stream> P_E_CHECK_MEMBERSHIP_BY_GOOGLE(Stream e_stream);
    }
}