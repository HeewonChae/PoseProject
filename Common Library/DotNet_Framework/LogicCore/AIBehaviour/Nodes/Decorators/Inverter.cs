using LogicCore.Utility;
using System.Collections.Generic;

namespace LogicCore.AIBehaviour.Nodes.Decorators
{
	public class Inverter : Decorator
	{
		public override IEnumerable<Status> Execute()
		{
			var child = _children[0];
			child.Start(this.CurrentTime);

			Status childStatus;
			while ((childStatus = child.OnUpdate(this.CurrentTime)) == Status.Running)
			{
				yield return Status.Running;
			}

			if (childStatus == Status.Failure)
			{
				yield return Status.Success;
				yield break;
			}

			if (childStatus == Status.Success)
			{
				yield return Status.Failure;
				yield break;
			}

			yield break;
		}

		public override void FreeAll()
		{
			base.FreeAll();
			Singleton.Get<NodePool<Inverter>>().Free(this);
		}
	}
}