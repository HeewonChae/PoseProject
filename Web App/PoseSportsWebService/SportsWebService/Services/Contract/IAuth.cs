﻿using PosePacket.Service.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SportsWebService.Services.Contract
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract(Name = "Auth")]
	public interface IAuth
	{
		[OperationContract]
		[WebInvoke(Method = "GET", UriTemplate = "TokenRefresh"
			, RequestFormat = WebMessageFormat.Json
			, ResponseFormat = WebMessageFormat.Json)]
		string P_E_TokenRefresh();

		[OperationContract]
		[WebInvoke(Method = "POST", UriTemplate = "CheckVaildOAuthUser"
			, RequestFormat = WebMessageFormat.Json
			, ResponseFormat = WebMessageFormat.Json)]
		Task<ExternAuthUser> P_E_CheckVaildOAuthUser(string e_json);

		[OperationContract]
		[WebInvoke(Method = "POST", UriTemplate = "Login"
			, RequestFormat = WebMessageFormat.Json
			, ResponseFormat = WebMessageFormat.Json)]
		O_Login P_E_Login(string e_json);
	}
}