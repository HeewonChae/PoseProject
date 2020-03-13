using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reservation = LogicCore.Utility.Alarm.Reservation;
using AlarmClass = LogicCore.Utility.Alarm;
using LogicCore.Utility;

namespace SportsAdminTool.Logic.Football.Alarm
{
	public class CheckCompletedFixtures : Reservation.IHandle, Singleton.INode
	{
		private Reservation _reservation;
		public Reservation Reservation { get => _reservation; set => _reservation = value; }

		public void CancelReservation()
		{
			if (_reservation != null)
				Singleton.Get<AlarmClass>().Cancel(ref _reservation);
		}

		public void SetAlarm(long deltaTime)
		{
			CancelReservation();

			Singleton.Get<AlarmClass>().Set(deltaTime, (logicTime, payload) => this.Execute(logicTime), handler: this);
		}

		public void Execute(long time)
		{
			var mainWindow = Singleton.Get<MainWindow>();

			mainWindow.Dispatcher.Invoke(() =>
			{
				mainWindow.Fv_item_check_completed_fixtures_Click(null, null);
			});
		}
	}
}
