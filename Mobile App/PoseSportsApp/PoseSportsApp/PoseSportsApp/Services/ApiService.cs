using Acr.UserDialogs;
using Flurl.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using PosePacket.WebError;
using PoseSportsApp.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServiceShare;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsApp.Services
{
	public static class ApiService
	{
		static ApiService()
		{
			if (WebClient.ExceptionHandler == null)
				WebClient.ExceptionHandler = ApiService.ExceptionHandler;

			WebConfig.ServiceBaseUrl = "http://192.168.0.157:8888/";
		}

		#region Exception Handler
		private static async Task ExceptionHandler(Exception exception)
		{
			if (exception is FlurlHttpException flurlException)
			{
				if(flurlException.Call.Response == null)
				{
					// 서버에 연결할 수 없음
					await UserDialogs.Instance.AlertAsync("Service unavailable");

					// TODO: 로그인 화면으로 이동
					return;
				}
				else if (flurlException.Call.Response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
				{
					try
					{
						var error = await flurlException.GetResponseJsonAsync<ErrorDetail>();
						await UserDialogs.Instance.AlertAsync(error.Message, $"ErrorCode: {error.ErrorCode}");
					}
					catch (Exception)
					{
						// 알 수 없는 서버 에러
						await UserDialogs.Instance.AlertAsync("Unkown service error");
					}
				}
				else
				{
#if DEBUG
					await UserDialogs.Instance.AlertAsync(flurlException.Message
					, $"ErrorCode: {((int)flurlException.Call.Response.StatusCode).ToString()}");
#else
					await UserDialogs.Instance.AlertAsync(httpEx.Call.Response.StatusCode.ToString()
					, $"Service ErrorCode: {((int)flurlException.Call.Response.StatusCode).ToString()}");
#endif
				}
			}
			else
			{
				// 알 수 없는 Flurl Client 에러
				await UserDialogs.Instance.AlertAsync("Unkown flurl error");
			}
		}

		private static bool CheckInternetConnected()
		{
			if (!CrossConnectivity.Current.IsConnected)
				return false;

			return true;
		}
		#endregion

		public static async Task<TOut> RequestAsync<TOut>(WebConfig.WebMethodType methodType, string serviceUrl, string segmentGroup, object data = null)
		{
			TOut result = default;

			if (!CheckInternetConnected())
			{
				await UserDialogs.Instance.AlertAsync(AppStringResource.Check_Internet_Connection);
				return result;

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
	}
}
