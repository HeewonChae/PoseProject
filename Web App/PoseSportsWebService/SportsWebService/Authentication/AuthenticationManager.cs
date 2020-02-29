using LogicCore.Utility;
using SportsWebService.WebBehavior.Common.HeaderProcess;
using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SportsWebService.Authentication
{
	public class AuthenticationManager : ServiceAuthenticationManager
	{
		public override ReadOnlyCollection<IAuthorizationPolicy> Authenticate(ReadOnlyCollection<IAuthorizationPolicy> authPolicy, Uri listenUri, ref Message message)
		{
			// Read Header
			PoseHeaderMessage.ReadHeader(message);

			// Check Credentials
			var credentials = CheckCredentialValid();
			var principal = new PosePrincipal(credentials);
			OperationContext.Current.IncomingMessageProperties.Add("Principal", principal);

			return authPolicy;
		}

		public PoseCredentials CheckCredentialValid()
		{
			PoseCredentials credentials = ServerContext.Current.DecryptCredentials();
			credentials.IsAuthenticated = false;

			if (credentials != PoseCredentials.Default
				&& credentials.ExpireTime > LogicTime.TIME())
			{
				credentials.IsAuthenticated = true;
			}

			return credentials;
		}
	}
}