using LogicCore.Debug;
using System;
using System.Threading;

namespace LogicCore.Thread.Message
{
	public abstract class Producer : IComparable<Producer>
	{
		private static int Indexer = 0x1000;
		public int Index = 0;

		public Producer()
		{
			Index = Interlocked.Add(ref Producer.Indexer, 0x02);
		}

		public int CompareTo(Producer other)
		{
			if (this.Index < other.Index)
				return -1;
			else if (this.Index > other.Index)
				return 1;

			return 0;
		}

		private bool lastPostResult = false;

		public bool Produce(Consumer.Singular consumer, Worker worker)
		{
			lastPostResult = consumer.Dispatch(this, worker);
			Dev.Assert(lastPostResult);
			return lastPostResult;
		}
	}
}