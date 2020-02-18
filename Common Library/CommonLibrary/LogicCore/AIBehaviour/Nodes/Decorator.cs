using LogicCore.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.AIBehaviour.Nodes
{
	public abstract class Decorator : Composite
	{
		public override void AddChild(Node child)
		{
			Dev.Assert(this._children.Count == 0, "Decorator node must have 1 child.");

			base.AddChild(child);
		}
	}
}
