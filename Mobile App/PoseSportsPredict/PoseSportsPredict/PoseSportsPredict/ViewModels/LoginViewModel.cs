using Plugin.LocalNotification;
using PosePacket.Service.Auth;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		#region BaseViewModel

		public override async Task<bool> PrepareView(params object[] data)
		{
			var request = new NotificationRequest
			{
				NotificationId = 1,
				Title = "Test",
				Description = $"Notification Activate",
				NotifyTime = DateTime.Now.AddSeconds(5),
				ReturningData = "dummy",
				Android = new Plugin.LocalNotification.AndroidOptions
				{
					IconName = "round_android_black_24",
				},
			};

			NotificationCenter.Current.Show(request);

			//await _OAuthService.OAuthLoginAsync(SNSProvider.Facebook);

			return true;
		}

		#endregion BaseViewModel

		#region Services

		private IOAuthService _OAuthService;

		#endregion Services

		#region Constructors

		public LoginViewModel(LoginPage page
			, IOAuthService OAuthService) : base(page)
		{
			_OAuthService = OAuthService;
		}

		#endregion Constructors
	}
}