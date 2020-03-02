using PosePacket.Service.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceShare.ExternAuthentication
{
    public interface IOAuthService
    {
        bool IsAuthenticated { get; }

        ExternAuthUser AuthenticatedUser { get; }

        Task OAuthLoginAsync(SNSProviderType provider);

        Task<bool> IsAuthenticatedAndValid();

        Task Logout();
    }
}