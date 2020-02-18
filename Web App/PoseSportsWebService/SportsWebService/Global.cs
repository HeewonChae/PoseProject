using LogicCore.Utility;
using SportsWebService.Utility;
using SportsWebService.WebBehavior.Server.ServiceInitialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService
{
	public class Global : IServiceInitialization
	{
		public void Initialize()
		{
			// Load Table
			string tableRootPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
			TableLoader.Init(tableRootPath);
		}
	}
}