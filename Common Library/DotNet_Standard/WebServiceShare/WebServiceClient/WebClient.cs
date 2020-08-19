using Flurl;
using Flurl.Http;
using MessagePack;
using PoseCrypto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
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

        public static async Task<TOut> RequestAsync<TOut>(WebRequestContext requestContext, string token = "")
        {
            var endPointAddr = new Url(requestContext.BaseUrl ?? "").AppendPathSegment(requestContext.ServiceUrl ?? "");
            var flurlRequest = new FlurlClient(endPointAddr).Request();

            //if (headers != null && headers.Length > 0)
            //{
            //    foreach (var (name, value) in headers)
            //    {
            //        flurlRequest.WithHeader(name, value);
            //    }
            //}

            flurlRequest.WithOAuthBearerToken(token);

            var convertedSegments = ConvertSegments(requestContext.SegmentGroup, requestContext.SegmentData);
            flurlRequest.AppendPathSegments(convertedSegments);

            List<(string, string)> convertedParams = ConvertQueryParams(requestContext.QueryParamGroup, requestContext.QueryParamData);
            foreach (var (name, value) in convertedParams)
            {
                flurlRequest.SetQueryParam(name, value);
            }

            requestContext.DataSerialize();
            if (requestContext.NeedEncrypt)
            {
                if (requestContext.PostData != null)
                    requestContext.PostData = CryptoFacade.Instance.Encrypt_AES((byte[])requestContext);

                return await EncryptSendAsync<TOut>(flurlRequest, requestContext);
            }
            else
                return await SendAsync<TOut>(flurlRequest, requestContext);
        }

        /// <summary>
        /// Get Packet RawData
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<byte[]> RequestRawAsync(WebRequestContext requestContext, string token = "")
        {
            var endPointAddr = new Url(requestContext.BaseUrl ?? "").AppendPathSegment(requestContext.ServiceUrl ?? "");
            var flurlRequest = new FlurlClient(endPointAddr).Request();

            //if (headers != null && headers.Length > 0)
            //{
            //    foreach (var (name, value) in headers)
            //    {
            //        flurlRequest.WithHeader(name, value);
            //    }
            //}

            flurlRequest.WithOAuthBearerToken(token);

            var convertedSegments = ConvertSegments(requestContext.SegmentGroup, requestContext.SegmentData);
            flurlRequest.AppendPathSegments(convertedSegments);

            List<(string, string)> convertedParams = ConvertQueryParams(requestContext.QueryParamGroup, requestContext.QueryParamData);
            foreach (var (name, value) in convertedParams)
            {
                flurlRequest.SetQueryParam(name, value);
            }

            requestContext.DataSerialize();
            return await SendAsync(flurlRequest, requestContext);
        }

        private static async Task<TOut> SendAsync<TOut>(IFlurlRequest flurlRequest, WebRequestContext requestContext)
        {
            TOut result = default;
            try
            {
                requestContext.AttemptCnt++;

                if (requestContext.MethodType == WebMethodType.GET)
                {
                    if (requestContext.SerializeType == SerializeType.Json)
                    {
                        result = await flurlRequest.GetJsonAsync<TOut>();
                    }
                    else
                    {
                        var rawData = await flurlRequest.GetBytesAsync();
                        result = MessagePackSerializer.Deserialize<TOut>(rawData);
                    }
                }
                else if (requestContext.MethodType == WebMethodType.POST)
                {
                    if (requestContext.SerializeType == SerializeType.Json)
                    {
                        result = await flurlRequest
                            .PostJsonAsync((string)requestContext)
                            .ReceiveJson<TOut>();
                    }
                    else
                    {
                        var rawData = await flurlRequest
                            .PostAsync(new ByteArrayContent(requestContext))
                            .ReceiveBytes();

                        result = MessagePackSerializer.Deserialize<TOut>(rawData);
                    }
                }
            }
            catch (FlurlHttpException flurlException)
            {
                if (requestContext.AttemptCnt < WebConfig.ReTryCount)
                    result = await RequestRetryPolicy<TOut>(flurlException, flurlRequest, requestContext);
                else
                {
                    if (_exceptionHandler != null)
                        await _exceptionHandler.Invoke(flurlException);
                }
            }
            catch (Exception ex)
            {
                if (_exceptionHandler != null)
                    await _exceptionHandler.Invoke(ex);
            }

            return result;
        }

        private static async Task<byte[]> SendAsync(IFlurlRequest flurlRequest, WebRequestContext requestContext)
        {
            byte[] rawData = null;

            try
            {
                requestContext.AttemptCnt++;

                if (requestContext.MethodType == WebMethodType.GET)
                {
                    rawData = await flurlRequest.GetBytesAsync();
                }
                else if (requestContext.MethodType == WebMethodType.POST)
                {
                    rawData = await flurlRequest
                        .PostAsync(new ByteArrayContent(requestContext))
                        .ReceiveBytes();
                }
            }
            catch (FlurlHttpException flurlException)
            {
                if (requestContext.AttemptCnt < WebConfig.ReTryCount)
                    rawData = await RequestRetryPolicy(flurlException, flurlRequest, requestContext);
                else
                {
                    if (_exceptionHandler != null)
                        await _exceptionHandler.Invoke(flurlException);
                }
            }
            catch (Exception ex)
            {
                if (_exceptionHandler != null)
                    await _exceptionHandler.Invoke(ex);
            }

            return rawData;
        }

        private static async Task<TOut> EncryptSendAsync<TOut>(IFlurlRequest flurlRequest, WebRequestContext requestContext)
        {
            TOut result = default;
            try
            {
                requestContext.AttemptCnt++;

                byte[] e_rawData = null;
                if (requestContext.MethodType == WebMethodType.GET)
                {
                    e_rawData = await flurlRequest.GetBytesAsync();
                }
                else if (requestContext.MethodType == WebMethodType.POST)
                {
                    e_rawData = await flurlRequest
                        .PostAsync(new ByteArrayContent(requestContext))
                        .ReceiveBytes();
                }

                var rawData = CryptoFacade.Instance.Decrypt_AES(e_rawData);
                result = MessagePackSerializer.Deserialize<TOut>(rawData);
            }
            catch (FlurlHttpException flurlException)
            {
                if (requestContext.AttemptCnt < WebConfig.ReTryCount)
                    result = await RequestRetryPolicy<TOut>(flurlException, flurlRequest, requestContext);
                else
                {
                    if (_exceptionHandler != null)
                        await _exceptionHandler.Invoke(flurlException);
                }
            }
            catch (CryptographicException cryptoEx)
            {
                if (requestContext.AttemptCnt < WebConfig.ReTryCount)
                    result = await EncryptSendAsync<TOut>(flurlRequest, requestContext);
                else
                {
                    if (_exceptionHandler != null)
                        await _exceptionHandler.Invoke(cryptoEx);
                }
            }
            catch (Exception ex)
            {
                if (_exceptionHandler != null)
                    await _exceptionHandler.Invoke(ex);
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

        private static async Task<TOut> RequestRetryPolicy<TOut>(FlurlHttpException flurlException, IFlurlRequest flurlRequest, WebRequestContext requestContext)
        {
            TOut result = default;

            if (flurlException.Call.Response != null
                && flurlException.Call.Response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                _exceptionHandler?.Invoke(flurlException);
            }
            else if (flurlException.Call.Response != null)// 재시도
            {
                if (requestContext.NeedEncrypt)
                    result = await EncryptSendAsync<TOut>(flurlRequest, requestContext);
                else
                    result = await SendAsync<TOut>(flurlRequest, requestContext);
            }

            return result;
        }

        private static async Task<byte[]> RequestRetryPolicy(FlurlHttpException flurlException, IFlurlRequest flurlRequest, WebRequestContext requestContext)
        {
            byte[] result = default;

            if (flurlException.Call.Response != null
                && flurlException.Call.Response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                _exceptionHandler?.Invoke(flurlException);
            }
            else if (flurlException.Call.Response != null)// 재시도
            {
                result = await SendAsync(flurlRequest, requestContext);
            }

            return result;
        }

        #endregion Utility Function
    }
}