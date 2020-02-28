using Acr.UserDialogs;
using Flurl.Http;
using Plugin.Connectivity;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.Services
{
	public class WebApiService : IWebApiService
	{
		public WebApiService()
		{
			if (WebClient.ExceptionHandler == null)
				WebClient.ExceptionHandler = this.ExceptionHandler;
		}

		#region Exception Handler

		private async Task ExceptionHandler(Exception exception)
		{
			if (exception is FlurlHttpException flurlException)
			{
				if (flurlException.Call.Response == null)
				{
					// 서버에 연결할 수 없음
					await UserDialogs.Instance.AlertAsync("Service unavailable");
				}
				else
				{
					await UserDialogs.Instance.AlertAsync(flurlException.Message
					, $"ErrorCode: {((int)flurlException.Call.Response.StatusCode).ToString()}");
				}
			}
			else
			{
				// 알 수 없는 Flurl Client 에러
				await UserDialogs.Instance.AlertAsync(exception.Message);
			}
		}

		#endregion Exception Handler

		public async Task<TOut> RequestAsync<TOut>(WebRequestContext reqContext, bool isIndicateLoading = true)
		{
			TOut result = default;

			if (!CheckInternetConnection())
			{
				await UserDialogs.Instance.AlertAsync(LocalizeString.Check_Internet_Connection);
				return result;
			}

			if (isIndicateLoading)
			{
				using (UserDialogs.Instance.Loading())
				{
					result = await WebClient.RequestAsync<TOut>(reqContext);
				}
			}
			else
			{
				result = await WebClient.RequestAsync<TOut>(reqContext);
			}

			return result;
		}

		public static bool CheckInternetConnection()
		{
			return !CrossConnectivity.IsSupported || CrossConnectivity.Current.IsConnected;
		}
	}
}