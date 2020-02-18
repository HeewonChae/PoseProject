using PosePacket.WebError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SportsWebService.WebBehavior.Server.ErrorHandler
{
    public class ServiceErrorHandler : IErrorHandler
    {
        #region 에러 처리하기 - HandleError(exception)

        /// <summary>
        /// 에러 처리하기
        /// </summary>
        /// <param name="exception">예외</param>
        /// <returns>처리 여부</returns>
        public bool HandleError(Exception exception)
        {
            Utility.ErrorHandler.WriteErrorLog(exception);
            return false;
        }
        #endregion

        #region 폴트 제공하기 - ProvideFault(exception, messageVersion, faultMessage)

        /// <summary>
        /// 폴트 제공하기
        /// </summary>
        /// <param name="exception">예외</param>
        /// <param name="messageVersion">메시지 버전</param>
        /// <param name="faultMessage">폴트 메시지</param>

        public void ProvideFault(Exception exception, MessageVersion messageVersion, ref Message faultMessage)
        {
            //if (exception is WebFaultException<ErrorDetail>
            //    || exception is WebFaultException)
            //    return;

            //var webFaultException = new WebFaultException<ErrorDetail>(new ErrorDetail()
            //{
            //    ErrorCode = 0,
            //    Message = "Invalid Access",
            //}
            //, HttpStatusCode.ServiceUnavailable);

            //var fault = webFaultException.CreateMessageFault();
            //faultMessage = Message.CreateMessage(messageVersion, fault, null);
        }

        #endregion
    }
}
