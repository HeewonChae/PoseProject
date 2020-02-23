using LogicCore.Converter;
using PosePacket;
using SportsWebService.Authentication;
using SportsWebService.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SportsWebService.Command.Auth
{
	[WebModelType(InputType = typeof(string), OutputType = typeof(byte[]))]
	public static class P_PoseToken
	{
		public class RowCode
		{
			[Description("Invalid input value")]
			public const int InvalidInputValue = ServiceErrorCode.WebMethod.P_PoseToken + 1;
		}

		public static string Execute(string input)
		{
			//if (string.IsNullOrEmpty(input))
			//ErrorHandler.OccurException(RowCode.InvalidInputValue);

			ServerContext.Current.CreateCredentials();
			byte[] eCredential = ServerContext.Current.EncryptCredentials();

			return Convert.ToBase64String(eCredential);
		}
	}
}