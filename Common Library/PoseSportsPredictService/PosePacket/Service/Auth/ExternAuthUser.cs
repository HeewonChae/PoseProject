﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Auth
{
	public class ExternAuthUser
	{
		public string Id { get; set; }

		public string Token { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string PictureUrl { get; set; }

		public SNSProvider Provider { get; set; }

		public DateTime ExpiresIn { get; set; }
	}
}