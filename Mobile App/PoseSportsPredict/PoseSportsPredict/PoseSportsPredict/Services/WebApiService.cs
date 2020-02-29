using Acr.UserDialogs;
using Flurl.Http;
using Plugin.Connectivity;
using PosePacket.WebError;
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
	public sealed class WebApiService : IWebApiService
	{
		public WebApiService()
		{
			if (WebClient.ExceptionHandler == null)
				WebClient.ExceptionHandler = this.ExceptionHandler;
		}

		#region Exception Handler

		private async void ExceptionHandler(Exception exception)
		{
			if (exception is FlurlHttpException flurlException)
			{
				if (flurlException.Call.Response == null)
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
					result = await WebClient.RequestAsync<TOut>(reqContext, false);
				}
			}
			else
			{
				result = await WebClient.RequestAsync<TOut>(reqContext, false);
			}

			return result;
		}

		public static bool CheckInternetConnection()
		{
			return !CrossConnectivity.IsSupported || CrossConnectivity.Current.IsConnected;
		}
	}
}