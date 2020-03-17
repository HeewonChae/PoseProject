using LogicCore.Debug;
using LogicCore.Utility.Pool;
using System.Collections.Generic;
using System.Threading;

namespace LogicCore.Utility
{
    public class Ticker : Singleton.INode
    {
        public delegate void OnTime(long time);

        public class Reservation : IPoolable
        {
            private static int Indexer = 0x1000;

            public int Index = 0;
            public OnTime OnTime { get; internal set; }

            public Reservation()
            {
                Index = Interlocked.Add(ref Reservation.Indexer, 0x02);
            }

            public void OnAlloc()
            {
            }

            public void OnFree()
            {
                this.OnTime = null;
            }
        }

        private readonly LightPool<Reservation> _reservationPool = new LightPool<Reservation>(() => new Reservation());

        private readonly Queue<Reservation> _willremove = new Queue<Reservation>();
        private readonly Queue<Reservation> _willadd = new Queue<Reservation>();
        private readonly Dictionary<int, Reservation> _updates = new Dictionary<int, Reservation>();

        public Reservation Set(OnTime onTime)
        {
            Dev.Assert(onTime != null, "Cannot set timer with null onTime delegate");

            Reservation reservation = this._reservationPool.Alloc();

            Dev.Assert(!_updates.ContainsKey(reservation.Index), "Already exist reservation");

            reservation.OnTime = onTime;
            _willadd.Enqueue(reservation);

            return reservation;
        }

        public void Remove(ref Reservation reservation)
        {
            Dev.Assert(_updates.ContainsKey(reservation.Index), "Not exist reservation");

            _willremove.Enqueue(reservation);
            reservation = null;
        }

        public void Update(long clock)
        {
            while (_willremove.Count != 0)
            {
                var willRemove = _willremove.Dequeue();
                _updates.Remove(willRemove.Index);
                _reservationPool.Free(willRemove);
            }

            while (_willadd.Count != 0)
            {
                Reservation willAdd = _willadd.Dequeue();
                _updates.Add(willAdd.Index, willAdd);
            }

            foreach (var reservation in _updates.Values)
            {
                reservation.OnTime.Invoke(clock);
            }
        }
    }
}