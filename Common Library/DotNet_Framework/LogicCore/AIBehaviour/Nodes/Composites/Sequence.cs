using LogicCore.Utility;
using System.Collections.Generic;

namespace LogicCore.AIBehaviour.Nodes.Composites
{
	public class Sequence : Composite
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

				if (childStatus != Status.Success)
				{
					yield return childStatus;
					yield break;
				}
			}

			yield return Status.Success;
			yield break;
		}

		public override void FreeAll()
		{
			base.FreeAll(); // Composite.FreeAll();

			Singleton.Get<NodePool<Sequence>>().Free(this);
		}
	}
}