using SportsWebService.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using Flurl.Http;
using System.Net;
using PosePacket.WebError;
using SportsWebService.Utility;
using LogicCore.Utility;
using System.Security.Permissions;
using Newtonsoft.Json;

namespace SportsWebService.Service
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
	public class Auth : Contract.IAuth
	{
		public string P_PoseToken(string i_json)
		{
			var input = i_json.JsonDeserialize<string>();

			return Command.Auth.P_PoseToken.Execute(input);
		}
	}
}