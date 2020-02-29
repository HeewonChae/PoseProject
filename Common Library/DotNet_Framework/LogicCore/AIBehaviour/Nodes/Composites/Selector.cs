using LogicCore.Utility;
using System.Collections.Generic;

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