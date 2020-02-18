using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsWebService.Authentication

{
	public enum ServiceRoleType
	{
		_NONE_,
		User = int.MaxValue - 4000,
		VIP = int.MaxValue - 3000,
		VVIP = int.MaxValue - 2000,
		Manager = int.MaxValue - 1000,
		Admin = int.MaxValue,
	}
}
