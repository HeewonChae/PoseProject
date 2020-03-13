using Flurl.Http;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PosePacket.WebError;
using System;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace ReferenceText.NetCore
{
    internal class Program
    {
        private static async Task ExceptionHandler(Exception exception)
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
        }

        private static void Main(string[] args)
        {
            string serviceBaseUrl = "http://192.168.0.157:8888/";

            WebClient.ExceptionHandler = Program.ExceptionHandler;

            ClientContext.SetCredentialsFrom(WebClient.RequestAsync<string>(new WebRequestContext()
            {
                MethodType = WebMethodType.POST,
                BaseUrl = serviceBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_E_Login,
                PostData = new I_Login
                {
                    PlatformId = "test",
                },
            }).Result);

            Console.ReadLine();
        }
    }
}