using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootballLogic = SportsAdminTool.Logic.Football;
using ApiLogic = SportsAdminTool.Logic.WebAPI;

using LogicThread = LogicCore.Thread;
using FootballAlarm = SportsAdminTool.Logic.Football.Alarm;

namespace SportsAdminTool.Logic
{
    public static class LogicStatic
    {
        public static void Init()
        {
            // DB init
            Repository.RepositoryStatic.Init_Mysql();

            // Log4Net Init
            LogicCore.Utility.ThirdPartyLog.Log4Net.Initialize();

            // Register singleton
            Singleton.Register(new ApiLogic.FootballWebAPI());
            Singleton.Register(new ApiLogic.FootballPredictorAPI());
            Singleton.Register(new FootballLogic.CheckValidation());
            Singleton.Register(new LogicThread.Message.Consumer.Singular());
            Singleton.Register(new LogicThread.Timeout());
            Singleton.Register(new Alarm());
            Singleton.Register(new Ticker());
            Singleton.Register(new FootballAlarm.InitializeDatabase());
            Singleton.Register(new FootballAlarm.CollectDatasAndPredict());
            Singleton.Register(new FootballAlarm.CheckCompletedFixtures());

            // Load Table
            string tableRootPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            TableLoader.Init(tableRootPath);
        }

        /// <summary>
        /// Called when main window is launched
        /// </summary>
        public static void StartWindow()
        {
            // Start singular logic thread
            var singular = Singleton.Get<LogicThread.Message.Consumer.Singular>();
            singular.Start();

            // Start timeout thread
            var heartBeat = new LogicThread.Timeout.Heartbeat();
            heartBeat.TimeHandler += (time) =>
            {
                // running at logic thread
                Singleton.Get<Alarm>().Update(time);
                Singleton.Get<Ticker>().Update(time);
            };
            Singleton.Get<LogicThread.Timeout>().Start(singular, heartBeat);

            //// Register logic alarm
            //TimeSpan ts = DateTime.Now.AddHours(24) - DateTime.Now; // 24시간 후
            //Singleton.Get<FootballAlarm.InitializeDatabase>().SetAlarm((long)ts.TotalMilliseconds);

            //ts = DateTime.Now.AddHours(6) - DateTime.Now; // 6시간 후
            //Singleton.Get<FootballAlarm.CollectDatasAndPredict>().SetAlarm((long)ts.TotalMilliseconds);

            //ts = DateTime.Now.AddHours(1) - DateTime.Now; // 1시간 후
            //Singleton.Get<FootballAlarm.CheckCompletedFixtures>().SetAlarm((long)ts.TotalMilliseconds);
        }
    }
}