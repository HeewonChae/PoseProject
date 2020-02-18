using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PosePacket
{
	public static class ServiceErrorCode
	{
		public static class Root
		{
			public const int Authenticate = 100000000;
			public const int WebService = 200000000;
			public const int Mysql = 300000000;
			public const int Redis = 400000000;

			public const int Max = 900000000;
		};

		public static class Authenticate
		{
			public const int CredentialsExpire = Root.Authenticate + 1;
		}

		public static class WebService
		{
			public const int HelloWorld = Root.WebService + 01000000;
			public const int Auth = Root.WebService + 02000000;

			public const int Max = Root.Max + 99000000;
		};

		public static class Database
		{
			public const int FootballDB = Root.Mysql + 10000000;

			public const int Max = Root.Max + 90000000;
		}

		public static class Redis
		{
		}

		public static class WebMethod
		{
			// HelloWorld
			public const int P_Hello = WebService.HelloWorld + 0001000;

			// Auth
			public const int P_GetCredentials = WebService.Auth + 0001000;
		}

		public static class Procedure
		{
		}
	}
}