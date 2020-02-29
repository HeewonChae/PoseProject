using PosePacket;
using PosePacket.Service.Auth;
using SportsWebService.Authentication;
using SportsWebService.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SportsWebService.Commands.Auth
{
	[WebModelType(InputType = typeof(I_Login), OutputType = typeof(O_Login))]
	public static class P_E_Login
	{
		public static class RowCode
		{
			[Description("Invalid input value")]
			public const int Invalid_InputValue = ServiceErrorCode.WebMethod_Auth.P_E_Login + 1;

			[Description("Invalid platform Id")]
			public const int Invalid_PlatformId = ServiceErrorCode.WebMethod_Auth.P_E_Login + 2;
		}

		public static O_Login Execute(I_Login input)
		{
			if (input == null)
				ErrorHandler.OccurException(RowCode.Invalid_InputValue);

			if (string.IsNullOrEmpty(input.PlatformId))
				ErrorHandler.OccurException(RowCode.Invalid_PlatformId);

			ServerContext.Current.CreateCredentials();
			byte[] eCredential = ServerContext.Current.EncryptCredentials();

			return new O_Login
			{
				PoseToken = Convert.ToBase64String(eCredential),
				TokenExpireIn = 60 * 60 // 3600초 (1시간)
			};
		}
	}
}