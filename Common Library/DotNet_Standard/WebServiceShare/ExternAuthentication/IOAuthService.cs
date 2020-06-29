using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using System;
using System.Threading.Tasks;

namespace WebServiceShare.ExternAuthentication
{
    public interface IOAuthService
    {
        bool IsAuthenticated { get; }

        ExternAuthUser AuthenticatedUser { get; }

        Task OAuthLoginAsync(SNSProviderType provider);

        void OnOAuthComplete(object sender, EventArgs args);

        void OnOAuthError(object sender, EventArgs args);

        Task<bool> IsAuthenticatedAndValid();

        Task Logout();
    }
}