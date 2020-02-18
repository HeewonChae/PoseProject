using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.AIBehaviour.Nodes.Decorators
{
	public class RepeatUntilFail : Decorator
	{
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

				if (childStatus != Status.Failure)
				{
					yield return Status.Running;
					continue;
				}

				break;
			}

			yield return Status.Success;
			yield break;
		}

		public override void FreeAll()
		{
			base.FreeAll(); // Composite.FreeAll();

			Singleton.Get<NodePool<RepeatUntilFail>>().Free(this);
		}
	}
}
