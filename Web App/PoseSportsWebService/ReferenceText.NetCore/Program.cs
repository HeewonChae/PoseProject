using Flurl.Http;
using MessagePack;
using PoseCrypto;
using PosePacket.Header;
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

        public static readonly string ServiceBaseUrl = "http://192.168.0.157:8888/";

        private static void Main(string[] args)
        {
            WebClient.ExceptionHandler = Program.ExceptionHandler;

            // RSA Key
            string serverPubKey = WebClient.RequestAsync<string>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.GET,
                BaseUrl = ServiceBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_PUBLISH_KEY,
            }, ClientContext.Token).Result;

            CryptoFacade.Instance.RSA_FromXmlString(serverPubKey);
            ClientContext.eSignature = CryptoFacade.Instance.GetEncryptedSignature();
            ClientContext.eSignatureIV = CryptoFacade.Instance.GetEncryptedSignatureIV();

            // Login
            O_Login login_output = WebClient.RequestAsync<O_Login>(new WebRequestContext()
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = ServiceBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_E_LOGIN,
                NeedEncrypt = true,
                PostData = new I_Login
                {
                    PlatformId = "test",
                }
            }, ClientContext.Token).Result;

            ClientContext.SetCredentialsFrom(login_output.PoseToken);
            ClientContext.TokenExpireIn = DateTime.UtcNow.AddMilliseconds(login_output.TokenExpireIn);
            ClientContext.LastLoginTime = login_output.LastLoginTime.ToLocalTime();

            // Serializer benchmark
            //Task.Factory.StartNew(() =>
            //{
            //    var serializerBenchmark = new SerializerBenchmark();
            //    serializerBenchmark.StartBenchmark();
            //}, TaskCreationOptions.LongRunning);

            Console.ReadLine();
        }
    }
}