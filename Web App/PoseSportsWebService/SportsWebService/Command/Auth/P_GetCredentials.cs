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
	public static class P_GetCredentials
	{
		public class RowCode
		{
			[Description("Invalid input value")]
			public const int InvalidInputValue = ServiceErrorCode.WebMethod.P_GetCredentials + 1;
		}

		public static byte[] Execute(string input)
		{
			//if (string.IsNullOrEmpty(input))
			//ErrorHandler.OccurException(RowCode.InvalidInputValue);

			ServerContext.Current.CreateCredentials();
			byte[] eCredential = ServerContext.Current.EncryptCredentials();

			return eCredential;
		}
	}
}