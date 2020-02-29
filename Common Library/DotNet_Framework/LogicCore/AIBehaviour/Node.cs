using LogicCore.Debug;
using LogicCore.Utility.Pool;
using System.Collections.Generic;

namespace LogicCore.AIBehaviour
{
	public enum Status
	{
		Success,
		Failure,
		Running
	}

	public abstract class Node : IPoolable
	{
		protected IEnumerator<Status> _current;
		protected Status _lastStatus;
		protected string _name = string.Empty;
		protected Node _parent;

		public string Name => _name;
		public Status LastStatus => _lastStatus;
		public long CurrentTime { get; set; }

		public Node Parent
		{
			get => _parent;
			internal set => _parent = value;
		}

		public virtual void OnAlloc()
		{
		}

		public virtual void OnFree()
		{
			_current = null;
			_name = string.Empty;
			_parent = null;
			_lastStatus = Status.Success;
		}

		public void SetName(string name)
		{
			_name = name;
		}

		public abstract IEnumerable<Status> Execute();

		public abstract void FreeAll();

		public Status OnUpdate(long time)
		{
			Dev.Assert(_current != null, $"Call {Name}.Start() first");

			this.CurrentTime = time;

			if (_current.MoveNext())
			{
				_lastStatus = _current.Current;
			}
			else
			{
				_current = null;
				return LastStatus;
			}

			if (LastStatus != Status.Running)
			{
				Stop(this.CurrentTime);
			}

			return LastStatus;
		}

		public virtual void Start(long time)
		{
			this.CurrentTime = time;

			_current = this.Execute().GetEnumerator();
		}

		public virtual void Stop(long time)
		{
			this.CurrentTime = time;

			if (LastStatus == Status.Running)
				_lastStatus = Status.Failure;
		}
	}
}