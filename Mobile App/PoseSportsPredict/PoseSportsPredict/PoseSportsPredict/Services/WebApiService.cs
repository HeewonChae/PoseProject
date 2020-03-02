using Acr.UserDialogs;
using Flurl.Http;
using Plugin.Connectivity;
using PosePacket;
using PosePacket.Header;
using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PosePacket.WebError;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logic.Utilities;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities.LocalStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

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

        private async void ExceptionHandler(Exception exception)
        {
            if (exception is FlurlHttpException flurlException)
            {
                if (flurlException.Call.Response == null)
                {
                    // 서버에 연결할 수 없음
                    await UserDialogs.Instance.AlertAsync(LocalizeString.Service_Not_Available);

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
                    catch
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

            if (!await CheckInternetConnection())
            {
                await UserDialogs.Instance.AlertAsync(LocalizeString.Check_Internet_Connection);
                return result;
            }

            if (isIndicateLoading)
            {
                using (UserDialogs.Instance.Loading())
                {
                    result = await WebClient.RequestAsync<TOut>(reqContext);
                }
            }
            else
            {
                result = await WebClient.RequestAsync<TOut>(reqContext);
            }

            return result;
        }

        public async Task<TOut> RequestAsyncWithToken<TOut>(WebRequestContext reqContext, bool isIndicateLoading = true)
        {
            TOut result = default;

            if (!await CheckInternetConnection())
            {
                return result;
            }

            // 토큰 만료 시간 체크
            LocalStorage.Storage.GetValueOrDefault<DateTime>(LocalStorageKey.PoseTokenExpireTime, out DateTime expireIn);
            if (expireIn < DateTime.UtcNow.AddMinutes(5))
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
                LocalStorage.Storage.AddOrUpdateValue(LocalStorageKey.PoseTokenExpireTime, DateTime.UtcNow.AddMilliseconds(refreshToken.TokenExpireIn));
            }

            if (isIndicateLoading)
            {
                using (UserDialogs.Instance.Loading())
                {
                    result = await WebClient.RequestAsync<TOut>(reqContext, (PoseHeader.HEADER_NAME, ClientContext.MakeHeader()));
                }
            }
            else
            {
                result = await WebClient.RequestAsync<TOut>(reqContext, (PoseHeader.HEADER_NAME, ClientContext.MakeHeader()));
            }

            return result;
        }

        public async Task<TOut> EncryptRequestAsync<TOut>(WebRequestContext reqContext, bool isIndicateLoading = true)
        {
            TOut result = default;

            if (!await CheckInternetConnection())
            {
                await UserDialogs.Instance.AlertAsync(LocalizeString.Check_Internet_Connection);
                return result;
            }

            // Encrypt PostData
            if (reqContext.PostData != null)
            {
                reqContext.PostData = _cryptoService.Encrypt_AES(reqContext.JsonSerialize());
            }

            string eResult;
            if (isIndicateLoading)
            {
                using (UserDialogs.Instance.Loading())
                {
                    eResult = await WebClient.RequestAsync<string>(reqContext, (PoseHeader.HEADER_NAME, ClientContext.MakeHeader()));
                }
            }
            else
            {
                eResult = await WebClient.RequestAsync<string>(reqContext, (PoseHeader.HEADER_NAME, ClientContext.MakeHeader()));
            }

            // Decrpyt output
            if (!string.IsNullOrEmpty(eResult))
            {
                result = _cryptoService.Decrypt_AES(eResult)
                    .JsonDeserialize<TOut>();
            }

            return result;
        }

        public async Task<TOut> EncrpytRequestAsyncWithToken<TOut>(WebRequestContext reqContext, bool isIndicateLoading = true)
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
            LocalStorage.Storage.GetValueOrDefault<DateTime>(LocalStorageKey.PoseTokenExpireTime, out DateTime expireIn);
            if (expireIn < DateTime.UtcNow.AddMinutes(5))
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
                LocalStorage.Storage.AddOrUpdateValue(LocalStorageKey.PoseTokenExpireTime, DateTime.UtcNow.AddMilliseconds(refreshToken.TokenExpireIn));
            }

            string eResult;
            if (isIndicateLoading)
            {
                using (UserDialogs.Instance.Loading())
                {
                    eResult = await WebClient.RequestAsync<string>(reqContext, (PoseHeader.HEADER_NAME, ClientContext.MakeHeader()));
                }
            }
            else
            {
                eResult = await WebClient.RequestAsync<string>(reqContext, (PoseHeader.HEADER_NAME, ClientContext.MakeHeader()));
            }

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
                await UserDialogs.Instance.AlertAsync(LocalizeString.Check_Internet_Connection);
                return false;
            }

            return true;
        }
    }
}