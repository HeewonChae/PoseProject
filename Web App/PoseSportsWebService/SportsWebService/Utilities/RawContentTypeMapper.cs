using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;

namespace SportsWebService.Utilities
{
    public class RawContentTypeMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
#if DEBUG
            LogicCore.Utility.ThirdPartyLog.Log4Net.WriteLog(contentType, LogicCore.Utility.ThirdPartyLog.Log4Net.Level.INFO);
#endif

            if (contentType.Contains("application/octet-stream"))
            {
                return WebContentFormat.Raw;
            }
            else if (contentType.Contains("application/json"))
            {
                return WebContentFormat.Json;
            }
            else
            {
                return WebContentFormat.Default;
            }
        }
    }
}