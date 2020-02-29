using PosePacket;
using PosePacket.Service.Auth;
using SportsWebService.Authentication.ExternOAuth;
using SportsWebService.Utility;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SportsWebService.Commands.Auth
{
	[WebModelType(InputType = typeof(I_CheckVaildOAuthUser), OutputType = typeof(ExternAuthUser))]
	public static class P_E_CheckVaildOAuthUser
	{
		public static class RowCode
		{
			[Description("Invalid input value")]
			public const int Invalid_InputValue = ServiceErrorCode.WebMethod_Auth.P_E_CheckVaildOAuthUser + 1;

			[Description("Invalid AccessToken")]
			public const int Invalid_AccessToken = ServiceErrorCode.WebMethod_Auth.P_E_CheckVaildOAuthUser + 2;

			[Description("Failed OAuth")]
			public const int Failed_OAuth = ServiceErrorCode.WebMethod_Auth.P_E_CheckVaildOAuthUser + 3;
		}

		public static async Task<ExternAuthUser> Execute(I_CheckVaildOAuthUser input)
		{
			if (input == null)
				ErrorHandler.OccurException(RowCode.Invalid_InputValue);

			if (string.IsNullOrEmpty(input.AccessToken))
				ErrorHandler.OccurException(RowCode.Invalid_AccessToken);

			var oAuth = OAuthProviderFactory.CreateProvider(input.SNSProvider);
			var externAuthUser = await oAuth.GetUserInfoAsync(input.AccessToken);

			if (externAuthUser == null)
				ErrorHandler.OccurException(RowCode.Failed_OAuth);

			// TODO: Check DB

			return externAuthUser;
		}
	}
}