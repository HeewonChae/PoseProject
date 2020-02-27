using PosePacket.Service.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceShare.ExternAuthentication
{
	public interface IAuthenticationService
	{
		bool IsAuthenticated { get; }

		ExternAuthUser AuthenticatedUser { get; }

		void OAuthLoginAsync(SNSProvider provider);

		Task<bool> IsAuthenticatedAndValid();

		void Logout();
	}
}