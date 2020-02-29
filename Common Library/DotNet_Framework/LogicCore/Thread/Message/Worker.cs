using LogicCore.Debug;

namespace LogicCore.Thread.Message
{
	public abstract class Worker
	{
		protected Producer _producer;

		public virtual void OnAlloc()
		{
			Dev.Assert(_producer == null);
		}

		public virtual void OnFree()
		{
			this.Clear();
		}

		public abstract void Execute(Producer producer);

		protected void Clear()
		{
			_producer = null;
		}

		public void Bind(Producer producer)
		{
			_producer = producer;
		}

		public Producer Pop()
		{
			return _producer;
		}
	}
}