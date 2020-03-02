using PosePacket.WebError;
using SportsWebService.App_Config;
using System;
using System.Net;
using System.ServiceModel.Web;

namespace SportsWebService.Utilities
{
    public static class ErrorHandler
    {
        public static void OccurException(int errorCode)
        {
            string message = string.Empty;
            message = ErrorCodeDescription.GetErrorDesc(errorCode);

            throw new WebFaultException<ErrorDetail>(new ErrorDetail()
            {
                Message = message,
                ErrorCode = errorCode,
            }
            , HttpStatusCode.ServiceUnavailable);
        }

        public static void OccurException(HttpStatusCode httpStatus)
        {
            throw new WebFaultException(httpStatus);
        }

        public static void WriteErrorLog(Exception exception)
        {
            // TODO: 예외 로그 작성
        }
    }
}