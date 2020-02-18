using LogicCore.Debug;
using LogicCore.Utility.Collection;
using LogicCore.Utility.Pool;
using System.Threading;

namespace LogicCore.Utility
{
	public class Alarm : Singleton.INode
	{
		public delegate void OnTime(long time, IAlarmPayload payload);
		public delegate void OnCancel();

		public interface IAlarmPayload
		{
			void Clear();
		}

		public class Reservation : IPoolable
		{
			public interface IHandle
			{
				Reservation Reservation { get; set; }
				void CancelReservation();
			}

			internal long Time { get; set; }
			internal OnTime OnTime { get; set; }
			internal OnCancel OnCancel { get;  set; }
			internal IAlarmPayload Payload { get; set; }
			internal IHandle Handler { get; set; }

			public bool IsCalled { get; internal set; }
			public bool IsCanceled { get; internal set; }

			public void OnAlloc() { }

			public void OnFree()
			{
				this.IsCalled = false;
				this.IsCanceled = false;
				this.OnTime = null;
				this.OnCancel = null;
				this.Handler = null;

				Payload?.Clear();
			}
		}

		private readonly MinHeap<long, Reservation> _reservations = new MinHeap<long, Reservation>();
		private readonly LightPool<Reservation> _reservationPool = new LightPool<Reservation>(() => new Reservation());
		private readonly object _blocker = new object();

		public Reservation Set(long time, OnTime onTime, OnCancel onCancel = null,
			Reservation.IHandle handler = null, IAlarmPayload payload = null)
		{
			Dev.Assert(onTime != null, "Cannot set timer with null onTime delegate");

			Monitor.Enter(_blocker);

			Reservation reservation = _reservationPool.Alloc();
			reservation.Time = LogicTime.TIME() + time;
			reservation.OnTime = onTime;
			reservation.OnCancel = onCancel;
			reservation.Handler = handler;
			reservation.Payload = payload;

			if (handler != null)
			{
				Dev.Assert(handler.Reservation != null, "Alarm reservation already used");
				reservation.Handler.Reservation = reservation;
			}

			_reservations.Add(reservation.Time, reservation);

			Monitor.Exit(_blocker);

			return reservation;
		}

		public void Cancel(ref Reservation reservation)
		{
			if (reservation == null || reservation.IsCalled)
				return;

			Dev.Assert(reservation.IsCanceled == false, "Alarm reservation already used !!!");

			Monitor.Enter(_blocker);

			if(!reservation.IsCalled)
			{
				reservation.IsCanceled = true;
				reservation.OnCancel?.Invoke();
				reservation.Handler = null;
				reservation = null;
			}

			Monitor.Exit(_blocker);
		}

		public void Update(long time)
		{
			if (_reservations.Count <= 0)
				return;

			Monitor.Enter(_blocker);

			var pair = _reservations.PeekFirst();
			while (pair.Key <= time)
			{
				_reservations.Pop();

				if (!pair.Value.IsCanceled)
				{
					pair.Value.IsCalled = true;

					if(pair.Value.Handler != null)
						pair.Value.Handler.Reservation = null;

					pair.Value.OnTime?.Invoke(pair.Key, pair.Value.Payload);
				}

				_reservationPool.Free(pair.Value);

				if (_reservations.Count <= 0)
					break;

				pair = _reservations.PeekFirst();
			}

			Monitor.Exit(_blocker);
		}
	}
}
