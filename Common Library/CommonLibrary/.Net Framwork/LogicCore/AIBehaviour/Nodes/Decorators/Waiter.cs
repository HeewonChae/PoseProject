using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.AIBehaviour.Nodes.Decorators
{
	public class Waiter : Decorator
	{
		public long _endTime = 0;
		public int _elpaseTime = 0;

		public void OnAlloc(string name, int waitTime = 1000)
		{
			base.SetName(name);
			_elpaseTime = waitTime;
		}

		public override void Start(long time)
		{
			_endTime = time + _elpaseTime;
			base.Start(time);
		}

		public override void Stop(long time)
		{
			_endTime = 0;
			base.Stop(time);
		}

		public override IEnumerable<Status> Execute()
		{
			while (this.CurrentTime < _endTime)
			{
				yield return Status.Running;
			}

			var child = _children[0];

			child.Start(this.CurrentTime);

			Status childStatus;
			while ((childStatus = child.OnUpdate(this.CurrentTime)) == Status.Running)
			{
				yield return Status.Running;
			}

			yield return childStatus;
			yield break;
		}

		public override void FreeAll()
		{
			base.FreeAll(); // Composite.FreeAll();

			// children 을 먼저 Free 후 현재 Composite 를 처리
			Singleton.Get<NodePool<Waiter>>().Free(this);
		}
	}
}
