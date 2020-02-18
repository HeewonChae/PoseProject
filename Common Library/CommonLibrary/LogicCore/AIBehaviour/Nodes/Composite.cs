﻿using LogicCore.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.AIBehaviour.Nodes
{
	public abstract class Composite : Node
	{
		protected readonly List<Node> _children = new List<Node>();

		public virtual void AddChild(Node child)
		{
			child.Parent = this;
			_children.Add(child);
		}

		public override void FreeAll()
		{
			foreach (var node in _children)
			{
				node.FreeAll();
			}

			Dev.DebugString($"Composite.FreeAll() - { this._name }");

			_children.Clear();
		}
	}
}
