using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Auth
{
	public class I_Login
	{
		public string PlatformId { get; set; }
	}

	public class O_Login
	{
		public string PoseToken { get; set; }
		public int TokenExpireIn { get; set; }
	}
}