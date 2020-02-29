using LogicCore.Debug;
using LogicCore.Utility;
using LogicCore.Utility.Collection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicCore.Thread
{
	/// <summary>
	/// App Logic Time Update, Pump Heatbeat worker
	/// </summary>
	public class Timeout : Message.Producer, Singleton.INode
	{
		public interface IListener
		{
			Boolean OnTimeout();
		}

		public delegate void Handler(long clock);

		public class Heartbeat : Message.Worker
		{
			public long Time { get; set; }

			public override void Execute(Message.Producer producer)
			{
				_handler?.Invoke(Time);
			}

			public event Timeout.Handler TimeHandler
			{
				add { _handler += value; }
				remove { _handler -= value; }
			}

			private Timeout.Handler _handler;
		}

		public static Int32 Interval = 1000; // 100ms = 0.1s

		private Message.Consumer.Singular _consumer;
		private Message.Worker _worker;
		private readonly MinHeap<long, IListener> _listeners = new MinHeap<long, IListener>();
		private readonly object _blocker = new object();
		private CancellationTokenSource _cts;

		public void Start(Message.Consumer.Singular consumer, Heartbeat worker)
		{
			Dev.Assert(_cts == null);
			_cts = new CancellationTokenSource();

			_consumer = consumer;
			_worker = worker;

			var startTime = Utility.LogicTime.TimeTicks;

			Task.Factory.StartNew((@object) =>
			{
				Timeout timeout = (@object as Timeout);
				while (this._cts.IsCancellationRequested == false)
				{
					timeout.OnTimeout();
				}
			}, this
			, TaskCreationOptions.LongRunning
			| TaskCreationOptions.DenyChildAttach);
		}

		public bool Stop()
		{
			if (_cts?.Token.CanBeCanceled ?? false)
			{
				_cts.Cancel();
				_cts = null;
				return true;
			}

			return false;
		}

		public void RegistLisener(IListener listener, long deltaTime)
		{
			Monitor.Enter(_blocker);

			_listeners.Add(deltaTime, listener);

			Monitor.Exit(_blocker);
		}

		public void OnListenerTimeout(long time)
		{
			if (_listeners.Count <= 0)
				return;

			var pair = _listeners.PeekFirst();
			while (pair.Key <= time)
			{
				_listeners.Pop();

				pair.Value.OnTimeout();

				if (_listeners.Count <= 0)
					break;

				pair = _listeners.PeekFirst();
			}
		}

		private void OnTimeout()
		{
			long curTime = Utility.LogicTime.TIME();

			var heartBeat = _worker as Heartbeat;
			heartBeat.Time = curTime;

			// dispatch
			base.Produce(this._consumer, this._worker);

			Monitor.Enter(_blocker);

			OnListenerTimeout(curTime);

			Monitor.Exit(_blocker);

			long delay = Utility.LogicTime.TIME() - curTime;
			if (delay < Timeout.Interval) // delay
				System.Threading.Thread.Sleep(Timeout.Interval - (int)delay);
		}
	}
}