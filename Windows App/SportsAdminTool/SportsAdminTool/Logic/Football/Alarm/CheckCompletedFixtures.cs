using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogicCore.Utility;

namespace SportsAdminTool.Logic.Football.Alarm
{
    public class CheckCompletedFixtures : LogicAlarmBase
    {
        public override void Execute(long time)
        {
            var mainWindow = Singleton.Get<MainWindow>();

            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.Fv_item_check_completed_fixtures_Click(null, null);
            });
        }
    }
}