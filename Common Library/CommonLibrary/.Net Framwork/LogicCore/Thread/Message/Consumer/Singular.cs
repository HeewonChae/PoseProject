using LogicCore.Debug;
using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LogicCore.Thread.Message.Consumer
{
	/// <summary>
	/// 단일 로직 스레드 생성
	/// </summary>
	public class Singular : Singleton.INode
	{
		public delegate bool Handler(Exception ex);

		public event Handler ExceptionHandler
		{
			add { _handler += value; }
			remove { _handler -= value; }
		}

		private Handler _handler;

		internal BufferBlock<Worker> Container = new BufferBlock<Worker>(
				new DataflowBlockOptions()
				{
					BoundedCapacity = -1,
					MaxMessagesPerTask = -1,
				}
			);

		public static Action<Singular> Process =
			 async (consumer) =>
			 {
				 Dev.DebugString("Singular Thread Start");

				 Worker worker;
				 while (null != (worker = await consumer.Container.ReceiveAsync().ConfigureAwait(false)) ||
					consumer._cts.IsCancellationRequested == false)
				 {
					 var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
					 Producer producer = worker.Pop();
					 if (producer != null)
					 {
						 worker.Execute(producer);
					 }
				 }

				 Dev.DebugString("Singular Thread stop");
			 };

		public bool Dispatch(Producer producer, Worker worker)
		{
			worker.Bind(producer);

			return this.Container.Post(worker);
		}

		private CancellationTokenSource _cts;
		public bool Start()
		{
			Dev.Assert(_cts == null);
			_cts = new CancellationTokenSource();

			Task.Factory.StartNew((@singular) => Singular.Process(@singular as Singular)
				  , this
				  , TaskCreationOptions.LongRunning 
				  | TaskCreationOptions.DenyChildAttach)
					.ContinueWith(
						(prevTasks) =>
						{
							if (prevTasks.Exception is AggregateException)
							{
								prevTasks.Exception.Handle((ex) => _handler(ex));
							}

						}, TaskContinuationOptions.OnlyOnFaulted 
							| TaskContinuationOptions.ExecuteSynchronously 
							| TaskContinuationOptions.DenyChildAttach);

			return true;
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
	}
}
