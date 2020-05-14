using LogicCore.Converter;
using PosePacket;
using SportsWebService.Logics;
using SportsWebService.Utilities;
using System.ComponentModel;
using System.Security.Principal;
using System.Threading;

namespace SportsWebService.Authentication
{
    public class PosePrincipal : IPrincipal
    {
        public static class RowCode
        {
            [Description("Don't have permission to use the service")]
            public const int None_Service_Permission = ServiceErrorCode.Authenticate.Principal + 1;
        }

        public static PosePrincipal Current => Thread.CurrentPrincipal as PosePrincipal;

        private readonly PoseCredentials _identity;
        private ServiceRoleType _role;

        public IIdentity Identity => _identity;
        public ServiceRoleType Role => _role;

        public PosePrincipal(PoseCredentials identity)
        {
            this._identity = identity;
        }

        public bool IsInRole(string role)
        {
            EnsureRoles();

            var result = role.TryParseEnum(out ServiceRoleType roleType);

            if (!(result && (int)roleType <= (int)Role))
                ErrorHandler.OccurException(RowCode.None_Service_Permission);

            return true;
        }

        protected virtual void EnsureRoles()
        {
            _identity.ServiceRoleType.TryParseEnum<ServiceRoleType>(out _role);
        }
    }
}