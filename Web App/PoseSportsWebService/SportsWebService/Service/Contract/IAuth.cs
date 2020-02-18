using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SportsWebService.Service.Contract
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract(Name = "Auth")]
	public interface IAuth
	{
		[OperationContract]
		[WebInvoke(Method = "POST", UriTemplate = "GetCredentials"
			, RequestFormat = WebMessageFormat.Json
			, ResponseFormat = WebMessageFormat.Json)]
		byte[] P_GetCredentials(string i_json);
	}
}
