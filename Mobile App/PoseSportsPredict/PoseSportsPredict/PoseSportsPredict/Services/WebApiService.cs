using Flurl.Http;
using MessagePack;
using Plugin.Connectivity;
using PoseCrypto;
using PosePacket;
using PosePacket.Header;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PosePacket.WebError;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities;
using Shiny;
using System;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using XF.Material.Forms.UI.Dialogs;

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

        private async Task ExceptionHandler(Exception exception)
        {
            if (exception is FlurlHttpException flurlException)
            {
                if (flurlException.Call.Response == null)
                {
                    // 서버에 연결할 수 없음
                    await MaterialDialog.Instance.AlertAsync(LocalizeString.Service_Not_Available,
                        LocalizeString.App_Title,
                        LocalizeString.Ok,
                        DialogConfiguration.DefaultAlterDialogConfiguration);

                    return;
                }
                else if (flurlException.Call.Response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    try
                    {
                        var errorString = await flurlException.Call.Response.Content.ReadAsStringAsync();

                        if (errorString.Contains("ErrorCode") && errorString.Contains("Message"))
                        {
                            var startIndex = errorString.IndexOf("<ErrorCode>") + 11;
                            var length = errorString.IndexOf("</ErrorCode>") - startIndex;
                            var str_errorCode = errorString.Substring(startIndex, length);
                            int errorCode = int.Parse(str_errorCode);

                            startIndex = errorString.IndexOf("<Message>") + 9;
                            length = errorString.IndexOf("</Message>") - startIndex;
                            var errorMessage = errorString.Substring(startIndex, length);

                            if (errorCode == (ServiceErrorCode.Authenticate.Credentials + 1))
                            {
                                await MaterialDialog.Instance.AlertAsync(LocalizeString.Not_Authenticated_Credencials,
                                    LocalizeString.App_Title,
                                    LocalizeString.Ok,
                                    DialogConfiguration.DefaultAlterDialogConfiguration);

                                // 로그인 화면으로 이동
                                await ShinyHost.Resolve<IOAuthService>().Logout();
                                return;
                            }
                            else
                            {
                                await MaterialDialog.Instance.AlertAsync($"E_{errorCode}", "Error",
                                    LocalizeString.Ok,
                                    DialogConfiguration.DefaultAlterDialogConfiguration);
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.AlertAsync(LocalizeString.Service_Not_Available,
                            DialogConfiguration.DefaultAlterDialogConfiguration);
                        }
                    }
                    catch
                    {
                        await MaterialDialog.Instance.AlertAsync(LocalizeString.Service_Not_Available,
                            DialogConfiguration.DefaultAlterDialogConfiguration);
                    }

                    return;
                }
                else
                {
#if DEBUG

                    await MaterialDialog.Instance.AlertAsync(flurlException.Message
                        , $"ErrorCode: {flurlException.Call.Response.StatusCode}",
                        LocalizeString.Ok,
                        DialogConfiguration.DefaultAlterDialogConfiguration);
#else
                    await MaterialDialog.Instance.AlertAsync(flurlException.Call.Response.StatusCode.ToString(),
                        $"ErrorCode: {flurlException.Call.Response.StatusCode}",
                        LocalizeString.Ok,
                        DialogConfiguration.DefaultAlterDialogConfiguration);
#endif
                    return;
                }
            }

            await MaterialDialog.Instance.AlertAsync(exception.Message,
                        LocalizeString.App_Title,
                        LocalizeString.Ok,
                        DialogConfiguration.DefaultAlterDialogConfiguration);

            return;
        }

        #endregion Exception Handler

        #region IWebApiService

        public async Task<TOut> RequestAsync<TOut>(WebRequestContext reqContext)
        {
            TOut result = default;

            if (!await CheckInternetConnection())
                return result;

            result = await WebClient.RequestAsync<TOut>(reqContext, ClientContext.Token);

            return result;
        }

        public async Task<TOut> RequestAsyncWithToken<TOut>(WebRequestContext reqContext)
        {
            TOut result = default;

            if (!await CheckInternetConnection() || !await RefrashToken())
                return result;

            result = await WebClient.RequestAsync<TOut>(reqContext, ClientContext.Token);

            return result;
        }

        public async Task<byte[]> RequestRawAsyncWithToken(WebRequestContext reqContext)
        {
            byte[] result = null;

            if (!await CheckInternetConnection() || !await RefrashToken())
                return result;

            result = await WebClient.RequestRawAsync(reqContext, ClientContext.Token);

            return result;
        }

        #endregion IWebApiService

        public static async Task<bool> CheckInternetConnection()
        {
            if (CrossConnectivity.IsSupported && !CrossConnectivity.Current.IsConnected)
            {
                await MaterialDialog.Instance.AlertAsync(LocalizeString.Check_Internet_Connection,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.DefaultAlterDialogConfiguration);
                return false;
            }

            return true;
        }

        public async Task<bool> RefrashToken()
        {
            // 토큰 만료 시간 체크
            if (ClientContext.TokenExpireIn < DateTime.UtcNow.AddMinutes(5))
            {
                // P_E_TokenRefresh
                var refreshToken = await this.RequestAsync<O_TokenRefresh>(new WebRequestContext
                {
                    SerializeType = SerializeType.MessagePack,
                    MethodType = WebMethodType.GET,
                    BaseUrl = AppConfig.PoseWebBaseUrl,
                    ServiceUrl = AuthProxy.ServiceUrl,
                    SegmentGroup = AuthProxy.P_E_TokenRefresh,
                    NeedEncrypt = true,
                });

                if (refreshToken == null)
                {
                    return false;
                }

                ClientContext.SetCredentialsFrom(refreshToken.PoseToken);
                ClientContext.TokenExpireIn = DateTime.UtcNow.AddMilliseconds(refreshToken.TokenExpireIn);
            }

            return true;
        }
    }
}