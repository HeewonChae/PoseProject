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
using System.Security.Permissions;
using Newtonsoft.Json;
using PosePacket.Service.Auth;
using System.Threading.Tasks;

namespace SportsWebService.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
	public class Auth : Contract.IAuth
	{
		public async Task<string> P_E_CheckVaildOAuthUser(string e_input)
		{
			var input = e_input.JsonDeserialize<I_CheckVaildOAuthUser>();

			var result = await Commands.Auth.P_E_CheckVaildOAuthUser.Execute(input);

			return result.JsonSerialize();
		}

		public string P_E_Login(string e_input)
		{
			var input = e_input.JsonDeserialize<I_Login>();

			var result = Commands.Auth.P_E_Login.Execute(input);

			return result.JsonSerialize();
		}

		public string P_E_TokenRefresh()
		{
			throw new NotImplementedException();
		}
	}
}