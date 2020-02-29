using PosePacket.Service.Auth;
using SportsWebService.Utility;
using System;
using System.Threading.Tasks;

namespace SportsWebService.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
	public class Auth : Contract.IAuth
	{
		public async Task<ExternAuthUser> P_E_CheckVaildOAuthUser(string e_input)
		{
			var input = e_input.JsonDeserialize<I_CheckVaildOAuthUser>();

			var result = await Commands.Auth.P_E_CheckVaildOAuthUser.Execute(input);

			return result;
		}

		public O_Login P_E_Login(string e_input)
		{
			var input = e_input.JsonDeserialize<I_Login>();

			var result = Commands.Auth.P_E_Login.Execute(input);

			return result;
		}

		public string P_E_TokenRefresh()
		{
			throw new NotImplementedException();
		}
	}
}