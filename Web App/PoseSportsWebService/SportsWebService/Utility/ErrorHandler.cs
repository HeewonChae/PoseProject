using GameKernel;
using LogicCore.Utility;
using PosePacket.WebError;
using SportsWebService.App_Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Web;

namespace SportsWebService.Utility
{
	public static class ErrorHandler
	{
		public static void OccurException(int errorCode)
		{
			string message = string.Empty;
			if (GameDatabase.FindRecord("ErrorCodeDescriptions", errorCode) is ErrorCodeDescription errorDesc)
				message = errorDesc.Description;

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