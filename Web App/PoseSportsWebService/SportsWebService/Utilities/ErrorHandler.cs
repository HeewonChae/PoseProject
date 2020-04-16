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
            throw new WebFaultException<ErrorDetail>(new ErrorDetail()
            {
                Message = ErrorCodeDescription.GetErrorDesc(errorCode),
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
            var message = "";
            switch (exception)
            {
                case WebFaultException<ErrorDetail> webFaultException:
                    message = $"{webFaultException.Detail.ErrorCode}, {webFaultException.Detail.Message}";
                    LogicCore.Utility.ThirdPartyLog.Log4Net.WriteLog(message, LogicCore.Utility.ThirdPartyLog.Log4Net.Level.ERROR);
                    break;

                default:
                    message = $"{exception.Message}";
                    LogicCore.Utility.ThirdPartyLog.Log4Net.WriteLog(message, LogicCore.Utility.ThirdPartyLog.Log4Net.Level.FATAL);
                    break;
            }
        }
    }
}