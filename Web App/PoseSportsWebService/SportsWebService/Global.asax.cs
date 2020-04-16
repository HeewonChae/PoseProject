﻿using LogicCore.Utility;
using SportsWebService.Logics;
using SportsWebService.Logics.Converters;
using SportsWebService.Utilities;
using System;
using System.IO;

namespace SportsWebService
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Log
            LogicCore.Utility.ThirdPartyLog.Log4Net.Initialize();

            // Mysql
            Repository.RepositoryStatic.Init_Mysql();

            // Register Singleton
            Singleton.Register<CryptoFacade>();
            Singleton.Register<FootballFixtureDetailConverter>();
            Singleton.Register<FootballStandingsDetailConverter>();

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