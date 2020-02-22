using Flurl;
using Flurl.Http;
using PosePacket;
using PosePacket.Header;
using PosePacket.Proxy;
using PosePacket.WebError;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;

namespace WebServiceShare.WebServiceClient
{
	public static class WebClient
	{
		#region Exception Handle Delegate

		public delegate Task ExceptionHandlerDelegate(Exception exception);

		private static ExceptionHandlerDelegate _exceptionHandler = null;

		public static ExceptionHandlerDelegate ExceptionHandler
		{
			get
			{
				return _exceptionHandler;
			}
			set
			{
				_exceptionHandler += value;
			}
		}

		#endregion Exception Handle Delegate

		public static async Task<TOut> RequestAsync<TOut>(RequestContext requestContext)
		{
			PoseHeader serviceHeader = new PoseHeader();
			ClientContext.CopyTo(serviceHeader);

			var endPointAddr = new Url(WebConfig.ServiceBaseUrl).AppendPathSegment(requestContext.ServiceUrl);
			var flurlReqeust = new FlurlClient(endPointAddr)
				.WithHeader(PoseHeader.HEADER_NAME, serviceHeader)
				.Request();

			var convertedSegments = ConvertSegments(requestContext.SegmentGroup, requestContext.InputData);
			foreach (var convertedSegment in convertedSegments)
			{
				flurlReqeust.AppendPathSegment(convertedSegment);
			}

			return await SendAsync<TOut>(flurlReqeust, requestContext);
		}

		private static async Task<TOut> SendAsync<TOut>(IFlurlRequest flurlRequest, RequestContext requestContext)
		{
			TOut result = default;
			try
			{
				requestContext.AttemptCnt++;

				if (requestContext.MethodType == WebConfig.WebMethodType.GET)
				{
					result = await flurlRequest
						.GetJsonAsync<TOut>();
				}
				else if (requestContext.MethodType == WebConfig.WebMethodType.POST)
				{
					result = await flurlRequest
						.PostJsonAsync(requestContext.InputDataJsonSerialize())
						.ReceiveJson<TOut>();
				}
			}
			catch (FlurlHttpException flurlException)
			{
				if (requestContext.AttemptCnt < 3)
					result = await RequestRetryPolicy<TOut>(flurlException, requestContext);
				else
					_exceptionHandler?.Invoke(flurlException);
			}
			catch (Exception ex)
			{
				_exceptionHandler?.Invoke(ex);
			}

			return result;
		}

		#region Utility Function

		private static List<string> ConvertSegments(string segmentGroup, object data)
		{
			var convertedSegments = new List<string>();
			var segments = segmentGroup.Split('/');

			foreach (var segment in segments)
			{
				if (segment.StartsWith("{"))
				{
					var propertyName = String.Join("", segment.Split('{', '}'));
					convertedSegments.Add((string)data.GetType().GetProperty(propertyName).GetValue(data, null));
				}
				else
				{
					convertedSegments.Add(segment);
				}
			}

			return convertedSegments;
		}

		private static async Task<TOut> RequestRetryPolicy<TOut>(FlurlHttpException flurlException, RequestContext requestContext)
		{
			TOut result = default;

			// 인증토큰 리프레쉬
			if (flurlException.Call.Response != null
				&& flurlException.Call.Response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
			{
				try
				{
					var error = await flurlException.GetResponseJsonAsync<ErrorDetail>();
					if (error.ErrorCode == ServiceErrorCode.Authenticate.CredentialsExpire)
					{
						ClientContext.eCredentials = await WebClient.RequestAsync<byte[]>(new RequestContext()
						{
							MethodType = WebConfig.WebMethodType.POST,
							ServiceUrl = AuthProxy.ServiceUrl,
							SegmentGroup = AuthProxy.P_GetCredentials,
						});
					}
				}
				catch (Exception)
				{
					_exceptionHandler?.Invoke(flurlException);
					return result;
				}
			}

			// 재시도
			if (flurlException.Call.Response != null)
			{
				result = await WebClient.RequestAsync<TOut>(requestContext);
			}

			return result;
		}

		#endregion Utility Function
	}
}