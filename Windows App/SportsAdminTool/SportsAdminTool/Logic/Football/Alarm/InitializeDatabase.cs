using LogicCore.AIBehaviour;
using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reservation = LogicCore.Utility.Alarm.Reservation;
using AlarmClass = LogicCore.Utility.Alarm;
using LogicCore.Debug;

namespace SportsAdminTool.Logic.Football.Alarm
{
    public class InitializeDatabase : LogicAlarmBase
    {
        public override void Execute(long time)
        {
            var mainWindow = Singleton.Get<MainWindow>();

            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.Fv_item_initialize_footballdb_Click(null, null);
            });
        }
    }
}