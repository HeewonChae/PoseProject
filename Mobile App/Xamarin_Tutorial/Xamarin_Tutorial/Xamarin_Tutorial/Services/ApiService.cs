using Acr.UserDialogs;
using Flurl.Http;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace Xamarin_Tutorial.Services
{
	public static class ApiService
	{
		static ApiService()
		{
			if (WebClient.ExceptionHandler == null)
				WebClient.ExceptionHandler = ApiService.ExceptionHandler;

			WebConfig.ServiceBaseUrl = "";
		}

		#region Exception Handler
		private static async Task ExceptionHandler(Exception exception)
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
		#endregion

		public static async Task<TOut> RequestAsync<TOut>(WebConfig.WebMethodType methodType, string serviceUrl, string segmentGroup, object data = null)
		{
			TOut result = default;

			if (!CheckInternetConnected())
			{
				await UserDialogs.Instance.AlertAsync("Please check internet connection");
				return default;

			}

			using (UserDialogs.Instance.Loading())
			{
				result = await WebClient.RequestAsync<TOut>(new RequestContext()
				{
					MethodType = methodType,
					ServiceUrl = serviceUrl,
					SegmentGroup = segmentGroup,
					InputData = data,
				});
			}

			return result;
		}

		private static bool CheckInternetConnected()
		{
			if (!CrossConnectivity.Current.IsConnected)
				return false;

			return true;
		}
	}
}
