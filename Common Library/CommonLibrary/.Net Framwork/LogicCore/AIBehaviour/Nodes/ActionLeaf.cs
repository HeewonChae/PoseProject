using LogicCore.Debug;
using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.AIBehaviour.Nodes
{
	public class ActionLeaf : Node
	{
		/// <summary>
		/// Function to invoke for the action.
		/// </summary>
		Func<long, Status> _behaviour;

		public void OnAlloc(string name, Func<long, Status> behaviour)
		{
			base.SetName(name);
			this._behaviour = behaviour;
		}

		public override IEnumerable<Status> Execute()
		{
			Status runStatus = Status.Success;
			while ((runStatus = _behaviour(Parent.CurrentTime)) == Status.Running)
			{
				yield return runStatus;
			}

			yield return runStatus;
			yield break;
		}

		public override void FreeAll()
		{
			Dev.DebugString($"ActionLeaf.FreeAll() { _name }");

			Singleton.Get<NodePool<ActionLeaf>>().Free(this);
		}
	}
}
