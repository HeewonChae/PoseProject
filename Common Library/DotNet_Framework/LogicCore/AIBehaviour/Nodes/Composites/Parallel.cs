using LogicCore.Utility;
using System.Collections.Generic;

namespace LogicCore.AIBehaviour.Nodes.Composites
{
	public class Parallel : Composite
	{
		private int _numRequiredToFail;
		private int _numRequiredToSucceed;

		public void OnAlloc(string name, int numRequiredToFail, int numRequiredToSucceed)
		{
			base.SetName(name);
			this._numRequiredToFail = numRequiredToFail;
			this._numRequiredToSucceed = numRequiredToSucceed;
		}

		public override IEnumerable<Status> Execute()
		{
			var numChildrenSuceeded = 0;
			var numChildrenFailed = 0;

			foreach (var child in _children)
			{
				child.Start(this.CurrentTime);

				Status childStatus;
				while ((childStatus = child.OnUpdate(this.CurrentTime)) == Status.Running)
				{
					yield return Status.Running;
				}

				switch (childStatus)
				{
					case Status.Success:
						++numChildrenSuceeded;
						break;

					case Status.Failure:
						++numChildrenFailed;
						break;
				}
			}

			if (_numRequiredToSucceed > 0 &&
				numChildrenSuceeded >= _numRequiredToSucceed)
			{
				yield return Status.Success;
				yield break;
			}

			if (_numRequiredToFail > 0 &&
				numChildrenFailed >= _numRequiredToFail)
			{
				yield return Status.Failure;
				yield break;
			}

			yield return Status.Running;
		}

		public override void FreeAll()
		{
			base.FreeAll();

			Singleton.Get<NodePool<Parallel>>().Free(this);
		}
	}
}