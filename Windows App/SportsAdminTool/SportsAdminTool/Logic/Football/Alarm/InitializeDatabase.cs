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
	public class InitializeDatabase : Reservation.IHandle, Singleton.INode
	{
		private Reservation _reservation;
		public Reservation Reservation { get => _reservation; set => _reservation = value; }

		public void CancelReservation()
		{
			if(_reservation != null)
				Singleton.Get<AlarmClass>().Cancel(ref _reservation);
		}

		public void SetAlarm(long deltaTime)
		{
			CancelReservation();

			_reservation = Singleton.Get<AlarmClass>().Set(deltaTime, (logicTime, payload) => this.Execute(logicTime));
		}

		public void Execute(long time)
		{
			var mainWindow = Singleton.Get<MainWindow>();

			mainWindow.Dispatcher.Invoke(() =>
			{
				mainWindow.Fv_item_initialize_footballdb_Click(null, null);
			});

			// 정상작동 안함..
			//mainWindow.Dispatcher.InvokeAsync(() =>
			//{
			//	mainWindow.Fv_item_initialize_footballdb_Click(null, null);
			//}).Completed += (sender, event_arg)=> { Dev.DebugString("Complete"); };
		}
	}
}
