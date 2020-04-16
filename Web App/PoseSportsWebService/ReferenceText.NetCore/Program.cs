using Flurl.Http;
using PosePacket.Header;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PosePacket.WebError;
using ReferenceTest.NetCore;
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

        private static readonly CryptoService _cryptoService = new CryptoService();
        private static readonly string _serviceBaseUrl = "http://192.168.0.157:8888/";

        private static void Main(string[] args)
        {
            WebClient.ExceptionHandler = Program.ExceptionHandler;

            // RSA Key
            string serverPubKey = WebClient.RequestAsync<string>(new WebRequestContext
            {
                MethodType = WebMethodType.GET,
                BaseUrl = _serviceBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_PUBLISHKEY,
            }).Result;

            _cryptoService.RSA_FromXmlString(serverPubKey);
            ClientContext.eSignature = _cryptoService.GetEncryptedSignature();
            ClientContext.eSignatureIV = _cryptoService.GetEncryptedSignatureIV();

            O_Login login_output = EncryptRequestAsync<O_Login>(new WebRequestContext()
            {
                MethodType = WebMethodType.POST,
                BaseUrl = _serviceBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_E_Login,
                PostData = new I_Login
                {
                    PlatformId = "test",
                }
            }).Result;

            ClientContext.SetCredentialsFrom(login_output.PoseToken);

            Console.ReadLine();
        }

        public static async Task<TOut> EncryptRequestAsync<TOut>(WebRequestContext reqContext)
        {
            TOut result = default;

            // Encrypt PostData
            if (reqContext.PostData != null)
            {
                reqContext.PostData = _cryptoService.Encrypt_AES(reqContext.JsonSerialize());
            }

            string eResult = await WebClient.RequestAsync<string>(reqContext, (PoseHeader.HEADER_NAME, ClientContext.MakeHeader()));

            // Decrpyt output
            if (!string.IsNullOrEmpty(eResult))
            {
                result = _cryptoService.Decrypt_AES(eResult).JsonDeserialize<TOut>();
            }

            return result;
        }
    }
}