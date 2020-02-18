using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Web;

namespace SportsWebService.Authentication
{
    public class AuthorizationPolicy : IAuthorizationPolicy
    {
        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            IPrincipal user = OperationContext.Current.IncomingMessageProperties["Principal"] as IPrincipal;
            evaluationContext.Properties["Principal"] = user;
            evaluationContext.Properties["Identities"] = new List<IIdentity> { user.Identity };

            return false;
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public string Id
        {
            get { return ServerContext.Current.Credentials.SessionID; }
        }
    }
}