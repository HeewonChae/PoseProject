using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PosePacket.Proxy;
using PosePacket.WebError;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WebServiceShare;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace ReferenceText.NetCore
{
	class Program
	{
		private static Task ExceptionHandler(Exception exception)
		{
			return Task.Run(async () =>
			{
				if (exception is FlurlHttpException)
				{
					var flurlException = exception as FlurlHttpException;
					if (flurlException.Call.Response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
					{
						var error = await flurlException.GetResponseJsonAsync<ErrorDetail>();
						Console.WriteLine($"ErrorCode: {error.ErrorCode} Message: {error.Message}");
					}
					else
					{
						Console.WriteLine($"{flurlException.Message}");
					}
				}
				else
				{
					Console.WriteLine($"{exception.Message}");
				}
			});
		}

		static void Main(string[] args)
		{
			WebConfig.ServiceBaseUrl = "http://192.168.0.157:8888/";

			WebClient.ExceptionHandler = Program.ExceptionHandler;

			ClientContext.eCredentials = WebClient.RequestAsync<byte[]>(new RequestContext()
			{
				MethodType = WebConfig.WebMethodType.POST,
				ServiceUrl = AuthProxy.ServiceUrl,
				SegmentGroup = AuthProxy.P_GetCredentials,
			}).Result;

			Console.ReadLine();
		}
	}
}
