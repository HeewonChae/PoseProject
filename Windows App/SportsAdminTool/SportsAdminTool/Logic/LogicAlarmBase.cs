using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageCore = LogicCore.Thread.Message;
using Reservation = LogicCore.Utility.Alarm.Reservation;
using AlarmClass = LogicCore.Utility.Alarm;

namespace SportsAdminTool.Logic
{
    public abstract class LogicAlarmBase : MessageCore.Producer, Reservation.IHandle, Singleton.INode
    {
        public class SetAlarmWorker : MessageCore.Worker
        {
            public LogicAlarmBase AlarmHandler { get; set; }
            public long Time { get; set; }

            public override void Execute(MessageCore.Producer producer)
            {
                Singleton.Get<AlarmClass>().Set(Time, (logicTime, payload) => AlarmHandler.Execute(logicTime), handler: AlarmHandler);
            }
        }

        public class CancelAlarmWorker : MessageCore.Worker
        {
            public LogicAlarmBase AlarmHandler { get; set; }

            public override void Execute(MessageCore.Producer producer)
            {
                Singleton.Get<AlarmClass>().Cancel(ref AlarmHandler._reservation);
            }
        }

        #region Reservation.IHandle

        private Reservation _reservation;
        public Reservation Reservation { get => _reservation; set => _reservation = value; }

        public void CancelReservation()
        {
            base.Produce(_consumer, _cancelAlarmWorker);
        }

        #endregion Reservation.IHandle

        #region Field

        private readonly SetAlarmWorker _setAlarmWorker;
        private readonly CancelAlarmWorker _cancelAlarmWorker;
        private readonly MessageCore.Consumer.Singular _consumer;

        #endregion Field

        #region Constructors

        public LogicAlarmBase()
        {
            _setAlarmWorker = new SetAlarmWorker
            {
                AlarmHandler = this,
            };

            _cancelAlarmWorker = new CancelAlarmWorker
            {
                AlarmHandler = this,
            };

            _consumer = Singleton.Get<MessageCore.Consumer.Singular>();
        }

        #endregion Constructors

        #region Methods

        public void SetAlarm(long deltaTime)
        {
            _setAlarmWorker.Time = deltaTime;
            base.Produce(_consumer, _setAlarmWorker);
        }

        public abstract void Execute(long time);

        #endregion Methods
    }
}