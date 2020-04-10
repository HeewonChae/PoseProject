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
            switch (exception)
            {
                case WebFaultException<ErrorDetail> webFaultException:
                    var message = $"{webFaultException.Detail.ErrorCode}, {webFaultException.Detail.Message} \n {webFaultException.StackTrace}";
                    LogicCore.Utility.ThirdPartyLog.Log4Net.WriteLog(message, LogicCore.Utility.ThirdPartyLog.Log4Net.Level.ERROR);
                    break;

                default:
                    LogicCore.Utility.ThirdPartyLog.Log4Net.WriteLog(exception.StackTrace, LogicCore.Utility.ThirdPartyLog.Log4Net.Level.FATAL);
                    break;
            }
        }
    }
}