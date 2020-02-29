using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.AIBehaviour.Nodes.Decorators
{
	public class Repeater : Decorator
	{
		public int repeatCount = 0;
		public int currentCount = 0;

		public void OnAlloc(string name, int count = 0)
		{
			base.SetName(name);
			this.repeatCount = count;
			this.currentCount = 0;
		}

		public override IEnumerable<Status> Execute()
		{
			Status childStatus;
			while (true)
			{
				var child = _children[0];

				child.Start(this.CurrentTime);

				while ((childStatus = child.OnUpdate(this.CurrentTime)) == Status.Running)
				{
					yield return Status.Running;
				}

				if (this.repeatCount == 0)
				{
					yield return Status.Running;
					continue;
				}

				if (++currentCount < this.repeatCount)
				{
					yield return Status.Running;
					continue;
				}

				break;
			}

			yield return childStatus;
			yield break;
		}

		public override void FreeAll()
		{
			base.FreeAll();

			Singleton.Get<NodePool<Repeater>>().Free(this);
		}
	}
}
