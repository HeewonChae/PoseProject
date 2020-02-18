using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using LogicCore.Converter;
using SportsWebService.Utility;
using SportsWebService.WebBehavior.Common.HeaderProcess;

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

            var timeSpan = Math.Abs(LogicCore.Utility.LogicTime.TIME() - credentials.CertifiedTime);
            if (timeSpan < PoseCredentials.EffectiveTime.TotalMilliseconds)
            {
                credentials.IsAuthenticated = true;
            }

            return credentials;
        }
    }
}