using PosePacket.Service.HelloWorld;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SportsWebService.Service.Contract
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract(Name = "HelloWorld")]
	public interface IHelloWorld
	{
		[OperationContract]
		[WebInvoke(Method = "POST", UriTemplate = "Hello"
			, RequestFormat = WebMessageFormat.Json
			, ResponseFormat = WebMessageFormat.Json)]
		O_Hello P_Hello(string i_json);
	}
}
