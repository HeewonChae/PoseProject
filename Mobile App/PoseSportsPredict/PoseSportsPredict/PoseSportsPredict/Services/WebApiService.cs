﻿using Flurl.Http;
using Plugin.Connectivity;
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
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.Services
{
    public sealed class WebApiService : IWebApiService
    {
        private CryptoService _cryptoService;

        public WebApiService(CryptoService cryptoService)
        {
            if (WebClient.ExceptionHandler == null)
                WebClient.ExceptionHandler = this.ExceptionHandler;

            _cryptoService = cryptoService;
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

                    // 로그인 화면으로 이동
                    await ShinyHost.Resolve<IOAuthService>().Logout();
                    return;
                }
                else if (flurlException.Call.Response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    try
                    {
                        var error = await flurlException.GetResponseJsonAsync<ErrorDetail>();

                        if (error.ErrorCode == (ServiceErrorCode.Authenticate.Credentials + 1))
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
                            await MaterialDialog.Instance.AlertAsync(error.Message, $"ErrorCode: {error.ErrorCode}",
                                LocalizeString.Ok,
                                DialogConfiguration.DefaultAlterDialogConfiguration);
                        }
                    }
                    catch
                    {
                        await MaterialDialog.Instance.AlertAsync(LocalizeString.Service_Not_Available);
                    }
                }
                else
                {
#if DEBUG

                    await MaterialDialog.Instance.AlertAsync(flurlException.Message
                        , $"ErrorCode: {flurlException.Call.Response.StatusCode}",
                        LocalizeString.Ok,
                        DialogConfiguration.DefaultAlterDialogConfiguration);
#else
					await UserDialogs.Instance.AlertAsync(httpEx.Call.Response.StatusCode.ToString(),
					    $"Service ErrorCode: {flurlException.Call.Response.StatusCode}",
                        LocalizeString.Ok,
                        DialogConfiguration.DefaultAlterDialogConfiguration);
#endif
                }
            }
        }

        #endregion Exception Handler

        public async Task<TOut> RequestAsync<TOut>(WebRequestContext reqContext)
        {
            TOut result = default;

            if (!await CheckInternetConnection())
            {
                await MaterialDialog.Instance.AlertAsync(LocalizeString.Check_Internet_Connection,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.DefaultAlterDialogConfiguration);
                return result;
            }

            result = await WebClient.RequestAsync<TOut>(reqContext);

            return result;
        }

        public async Task<TOut> RequestAsyncWithToken<TOut>(WebRequestContext reqContext)
        {
            TOut result = default;

            if (!await CheckInternetConnection())
            {
                return result;
            }

            // 토큰 만료 시간 체크
            if (ClientContext.TokenExpireIn < DateTime.UtcNow.AddMinutes(5))
            {
                // P_E_TokenRefresh
                var refreshToken = await this.EncryptRequestAsync<O_TokenRefresh>(new WebRequestContext
                {
                    MethodType = WebMethodType.GET,
                    BaseUrl = AppConfig.PoseWebBaseUrl,
                    ServiceUrl = AuthProxy.ServiceUrl,
                    SegmentGroup = AuthProxy.P_E_TokenRefresh,
                });

                if (refreshToken == null)
                {
                    return result;
                }

                ClientContext.SetCredentialsFrom(refreshToken.PoseToken);
                ClientContext.TokenExpireIn = DateTime.UtcNow.AddMilliseconds(refreshToken.TokenExpireIn);
            }

            result = await WebClient.RequestAsync<TOut>(reqContext, (PoseHeader.HEADER_NAME, ClientContext.MakeHeader()));

            return result;
        }

        public async Task<TOut> EncryptRequestAsync<TOut>(WebRequestContext reqContext)
        {
            TOut result = default;

            if (!await CheckInternetConnection())
            {
                await MaterialDialog.Instance.AlertAsync(LocalizeString.Check_Internet_Connection,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.DefaultAlterDialogConfiguration);
                return result;
            }

            // Encrypt PostData
            if (reqContext.PostData != null)
            {
                reqContext.PostData = _cryptoService.Encrypt_AES(reqContext.JsonSerialize());
            }

            string eResult = await WebClient.RequestAsync<string>(reqContext, (PoseHeader.HEADER_NAME, ClientContext.MakeHeader()));

            // Decrpyt output
            if (!string.IsNullOrEmpty(eResult))
            {
                result = _cryptoService.Decrypt_AES(eResult)
                    .JsonDeserialize<TOut>();
            }

            return result;
        }

        public async Task<TOut> EncrpytRequestAsyncWithToken<TOut>(WebRequestContext reqContext)
        {
            TOut result = default;

            if (!await CheckInternetConnection())
            {
                return result;
            }

            // Encrypt PostData
            if (reqContext.PostData != null)
            {
                reqContext.PostData = _cryptoService.Encrypt_AES(reqContext.JsonSerialize());
            }

            // 토큰 만료 시간 체크
            if (ClientContext.TokenExpireIn < DateTime.UtcNow.AddMinutes(5))
            {
                // P_E_TokenRefresh
                var refreshToken = await this.EncryptRequestAsync<O_TokenRefresh>(new WebRequestContext
                {
                    MethodType = WebMethodType.GET,
                    BaseUrl = AppConfig.PoseWebBaseUrl,
                    ServiceUrl = AuthProxy.ServiceUrl,
                    SegmentGroup = AuthProxy.P_E_TokenRefresh,
                });

                if (refreshToken == null)
                {
                    return result;
                }

                ClientContext.SetCredentialsFrom(refreshToken.PoseToken);
                ClientContext.TokenExpireIn = DateTime.UtcNow.AddMilliseconds(refreshToken.TokenExpireIn);
            }

            string eResult = await WebClient.RequestAsync<string>(reqContext, (PoseHeader.HEADER_NAME, ClientContext.MakeHeader()));

            // Decrpyt output
            if (!string.IsNullOrEmpty(eResult))
            {
                result = _cryptoService.Decrypt_AES(eResult)
                    .JsonDeserialize<TOut>();
            }

            return result;
        }

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
    }
}