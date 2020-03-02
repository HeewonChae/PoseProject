using LogicCore.Utility;
using SportsWebService.Utilities;
using System;
using System.IO;

namespace SportsWebService
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Mysql
            Repository.RepositoryStatic.Init_Mysql();

            // Register Singleton
            Singleton.Register<CryptoFacade>();

            // Load Table
            string tableRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            TableLoader.Init(tableRootPath);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}