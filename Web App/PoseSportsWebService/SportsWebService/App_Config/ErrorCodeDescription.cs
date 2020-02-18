using GameKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.App_Config
{
	public class ErrorCodeDescription : IRecord
	{
		public readonly int ErrorCode = 0;
		public readonly string Description = string.Empty;
	}
}