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

		public static async Task<TOut> RequestAsync<TOut>(WebRequestContext requestContext, bool isIncludePoseHeader = true)
		{
			PoseHeader serviceHeader = new PoseHeader();
			ClientContext.CopyTo(serviceHeader);

			var endPointAddr = new Url(requestContext?.BaseUrl ?? "").AppendPathSegment(requestContext?.BaseUrl ?? "");
			var flurlClient = new FlurlClient(endPointAddr);
			if (isIncludePoseHeader)
				flurlClient.WithHeader(PoseHeader.HEADER_NAME, serviceHeader);

			var flurlReqeust = new FlurlClient(endPointAddr)
				.Request();

			var convertedSegments = ConvertSegments(requestContext.SegmentGroup, requestContext.SegmentData);
			flurlReqeust.AppendPathSegments(convertedSegments);

			List<(string, string)> convertedParams = ConvertQueryParams(requestContext.QueryParamGroup, requestContext.QueryParamData);
			foreach (var (name, value) in convertedParams)
			{
				flurlReqeust.SetQueryParam(name, value);
			}

			return await SendAsync<TOut>(flurlReqeust, requestContext);
		}

		private static async Task<TOut> SendAsync<TOut>(IFlurlRequest flurlRequest, WebRequestContext requestContext)
		{
			TOut result = default;
			try
			{
				requestContext.AttemptCnt++;

				if (requestContext.MethodType == WebMethodType.GET)
				{
					result = await flurlRequest
						.GetJsonAsync<TOut>();
				}
				else if (requestContext.MethodType == WebMethodType.POST)
				{
					result = await flurlRequest
						.PostJsonAsync(requestContext.PostDataJsonSerialize())
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

			if (string.IsNullOrEmpty(segmentGroup))
				return convertedSegments;

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

		private static List<(string name, string value)> ConvertQueryParams(string queryParamsGroup, object data)
		{
			var convertedParams = new List<(string name, string value)>();

			if (string.IsNullOrEmpty(queryParamsGroup))
				return convertedParams;

			var queryParams = queryParamsGroup.Split('&');

			foreach (var queryParam in queryParams)
			{
				var splitParam = queryParam.Split('=');
				if (splitParam.Length != 2)
					continue;

				string queryName = splitParam[0];
				string queryData = splitParam[1];

				if (queryData.StartsWith("{"))
				{
					var propertyName = String.Join("", queryData.Split('{', '}'));
					queryData = (string)data.GetType().GetProperty(propertyName).GetValue(data, null);
				}

				convertedParams.Add((queryName, queryData));
			}

			return convertedParams;
		}

		private static async Task<TOut> RequestRetryPolicy<TOut>(FlurlHttpException flurlException, WebRequestContext requestContext)
		{
			TOut result = default;

			// 인증토큰 리프레쉬
			//if (flurlException.Call.Response != null
			//	&& flurlException.Call.Response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
			//{
			//	try
			//	{
			//		var error = await flurlException.GetResponseJsonAsync<ErrorDetail>();
			//		if (error.ErrorCode == ServiceErrorCode.Authenticate.CredentialsExpire)
			//		{
			//			ClientContext.SetCredentialsFrom(await WebClient.RequestAsync<string>(new RequestContext()
			//			{
			//				MethodType = WebConfig.WebMethodType.POST,
			//				ServiceUrl = AuthProxy.ServiceUrl,
			//				SegmentGroup = AuthProxy.P_PoseToken,
			//			}));
			//		}
			//	}
			//	catch (Exception)
			//	{
			//		_exceptionHandler?.Invoke(flurlException);
			//		return result;
			//	}
			//}

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