using LogicCore.Thread;
using LogicCore.Utility;
using LogicCore.Utility.ThirdPartyLog;
using SportsAdminTool.Logic.Football;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SportsAdminTool
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Global Exception Handler
            AppDomain.CurrentDomain.UnhandledException += (s, eArgs) =>
            {
                Log4Net.WriteLog(eArgs.ExceptionObject.ToString(), Log4Net.Level.FATAL);
                Trace.Assert(false, eArgs.ExceptionObject.ToString());

                Singleton.Get<CheckValidation>().OutputErrorToJsonFile();
            };

            base.OnStartup(e);

            // Prepare logical initialize
            Logic.LogicStatic.Init();
            Model.ModelStatic.Init();
        }
    }
}