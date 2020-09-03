using GameKernel;
using LogicCore.File;
using System.Collections.Generic;

namespace SportsWebService.App_Config
{
    public sealed class ErrorCodeDescription : IRecord
    {
        public static Dictionary<int, string> Errors = new Dictionary<int, string>();

        public static string GetErrorDesc(int errorCode)
        {
            if (Errors.ContainsKey(errorCode))
                return Errors[errorCode];

            return "Unknown Error";
        }
    }
}