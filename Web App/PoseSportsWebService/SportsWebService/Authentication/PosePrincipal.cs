using LogicCore.Converter;
using System.Security.Principal;
using System.Threading;

namespace SportsWebService.Authentication
{
	public class PosePrincipal : IPrincipal
	{
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

			return result && (int)roleType <= (int)Role;
		}

		protected virtual void EnsureRoles()
		{
			// get the roles for the identity from a database (or other source)
			// and cache them for subsequent requests

			// TODO: Test
			_role = ServiceRoleType.Admin;
		}
	}
}