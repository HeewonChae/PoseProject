using Acr.UserDialogs;
using Flurl.Http;
using Newtonsoft.Json;
using PosePacket.WebError;
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
	public static class WebFacade
	{
		static WebFacade()
		{
			if (WebClient.ExceptionHandler == null)
				WebClient.ExceptionHandler = WebFacade.ExceptionHandler;
		}

		#region Exception Handler
		private static async Task ExceptionHandler(Exception exception)
		{
			if (exception is FlurlHttpException flurlException)
			{
				if(flurlException.Call.Response == null)
				{
					// 서버에 연결할 수 없음
					await UserDialogs.Instance.AlertAsync("Service Unavailable");
					return;
				}

				if (flurlException.Call.Response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
				{
					try
					{
						var error = await flurlException.GetResponseJsonAsync<ErrorDetail>();
						await UserDialogs.Instance.AlertAsync(error.Message, $"ErrorCode: {error.ErrorCode}");
					}
					catch (Exception)
					{
						// 알 수 없는 서버 에러
						await UserDialogs.Instance.AlertAsync("Unkown Service Error");
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
				await UserDialogs.Instance.AlertAsync("Unkown WebClient Error");
			}
		}
		#endregion

		public static async Task<TOut> RequestAsync<TOut>(WebConfig.WebMethodType methodType, string serviceUrl, string segmentGroup, object data = null)
		{
			TOut result = default;

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
