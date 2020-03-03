using LogicCore.Converter;
using PosePacket;
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

        private readonly PoseCredentials _identity;
        private ServiceRoleType _role;

        public PosePrincipal(PoseCredentials identity)
        {
            this._identity = identity;
        }

        public static PosePrincipal Current
        {
            get
            {
                return Thread.CurrentPrincipal as PosePrincipal;
            }
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public ServiceRoleType Role
        {
            get { return _role; }
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
            // get the roles for the identity from a database (or other source)
            // and cache them for subsequent requests

            // TODO: Get RoleType
            _role = ServiceRoleType.User;
        }
    }
}