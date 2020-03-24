using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
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