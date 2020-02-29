using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.AIBehaviour.Nodes.Composites
{
	public class Selector : Composite
	{
		public override IEnumerable<Status> Execute()
		{
			foreach (var child in _children)
			{
				child.Start(this.CurrentTime);

				Status childStatus;
				while ((childStatus = child.OnUpdate(this.CurrentTime)) == Status.Running)
				{
					yield return Status.Running;
				}

				if (childStatus != Status.Failure)
				{
					yield return childStatus;
					yield break;
				}
			}

			yield return Status.Failure;
		}

		public override void FreeAll()
		{
			base.FreeAll();

			Singleton.Get<NodePool<Selector>>().Free(this);
		}
	}
}
