using LogicCore.AIBehaviour.Nodes;
using LogicCore.AIBehaviour.Nodes.Decorators;
using LogicCore.AIBehaviour.Nodes.Composites;
using LogicCore.Debug;
using LogicCore.Utility;
using LogicCore.Utility.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicCore.AIBehaviour
{
	public class Builder : IPoolable
	{
		private Node _curNode = null;
		private readonly Stack<Node> _parentNodes = new Stack<Node>();

		public void OnAlloc() { }
		public void OnFree() 
		{
			CleanUp();
		}

		/// <summary>
		/// leaf node (action)
		/// </summary>
		/// <param name="name"></param>
		/// <param name="behaviour"></param>
		/// <returns></returns>
		public Builder Do(string name, Func<long, Status> behaviour)
		{
			if (_parentNodes.Count <= 0)
			{
				throw new ApplicationException("it must be a leaf node...");
			}

			var actionNode = Singleton.Get<NodePool<ActionLeaf>>().Alloc();
			actionNode.OnAlloc(name, behaviour);

			(_parentNodes.Peek() as Composite).AddChild(actionNode);
			return this;
		}

		/// <summary>
		/// decoration(condition) for action
		/// </summary>
		/// <param name="name"></param>
		/// <param name="behaviour"></param>
		/// <returns></returns>
		public Builder Condition(string name, Func<long, bool> behaviour)
		{
			return Do(name, (time) => behaviour(time) ? Status.Success : Status.Failure);
		}

		/// <summary>
		/// decoration(repeater) for action
		/// </summary>
		/// <param name="name"></param>
		/// <param name="behaviour"></param>
		/// <returns></returns>
		public Builder Repeat(string name, int numRequired)
		{
			var repeater = Singleton.Get<NodePool<Repeater>>().Alloc();
			repeater.OnAlloc(name, numRequired);

			if (_parentNodes.Count > 0)
			{
				(_parentNodes.Peek() as Composite).AddChild(repeater);
			}

			_parentNodes.Push(repeater);

			return this;
		}

		/// <summary>
		/// decoration(repeater) for action
		/// </summary>
		/// <param name="name"></param>
		/// <param name="behaviour"></param>
		/// <returns></returns>
		public Builder RepeatUntilFail(string name)
		{
			var repeatUntilFailure = Singleton.Get<NodePool<RepeatUntilFail>>().Alloc();
			repeatUntilFailure.SetName(name);

			if (_parentNodes.Count > 0)
			{
				(_parentNodes.Peek() as Composite).AddChild(repeatUntilFailure);
			}

			_parentNodes.Push(repeatUntilFailure);
			return this;
		}
		/// <summary>
		/// decoration(repeater) for action
		/// </summary>
		/// <param name="name"></param>
		/// <param name="behaviour"></param>
		/// <returns></returns>
		public Builder Wait(string name, int waitTime)
		{
			var waiter = Singleton.Get<NodePool<Waiter>>().Alloc();
			waiter.OnAlloc(name, waitTime);

			if (_parentNodes.Count > 0)
			{
				(_parentNodes.Peek() as Composite).AddChild(waiter);
			}

			_parentNodes.Push(waiter);
			return this;
		}

		/// <summary>
		/// composite(sequence) for action
		/// </summary>
		/// <param name="name"></param>
		/// <param name="behaviour"></param>
		/// <returns></returns>
		public Builder Sequence(string name)
		{
			var squence = Singleton.Get<NodePool<Sequence>>().Alloc();
			squence.SetName(name);

			if (_parentNodes.Count > 0)
			{
				(_parentNodes.Peek() as Composite).AddChild(squence);
			}

			_parentNodes.Push(squence);
			return this;
		}


		/// <summary>
		/// composite(parallel) for action
		/// </summary>
		/// <param name="name"></param>
		/// <param name="behaviour"></param>
		/// <returns></returns>
		public Builder Parallel(string name, int numRequiredToFail, int numRequiredToSucceed)
		{
			var Parallel = Singleton.Get<NodePool<Parallel>>().Alloc();
			Parallel.OnAlloc(name, numRequiredToFail, numRequiredToSucceed);

			if (_parentNodes.Count > 0)
			{
				(_parentNodes.Peek() as Composite).AddChild(Parallel);
			}

			_parentNodes.Push(Parallel);
			return this;
		}

		/// <summary>
		/// composite(selector) for action
		/// </summary>
		/// <param name="name"></param>
		/// <param name="behaviour"></param>
		/// <returns></returns>
		public Builder Selector(string name)
		{
			var Selector = Singleton.Get<NodePool<Selector>>().Alloc();
			Selector.SetName(name);

			if (_parentNodes.Count > 0)
			{
				(_parentNodes.Peek() as Composite).AddChild(Selector);
			}

			_parentNodes.Push(Selector);
			//Trace.WriteLine($"Selector Push to node {name}");

			return this;
		}


		/// <summary>
		/// decoration(repeater) for action
		/// </summary>
		/// <param name="name"></param>
		/// <param name="behaviour"></param>
		/// <returns></returns>
		public Builder Inverter(string name)
		{
			var inverter = Singleton.Get<NodePool<Inverter>>().Alloc();
			inverter.SetName(name);

			if (_parentNodes.Count > 0)
			{
				(_parentNodes.Peek() as Composite).AddChild(inverter);
			}

			_parentNodes.Push(inverter);
			return this;
		}

		public Builder Splice(Node subTree)
		{
			Dev.Assert(subTree != null, "subTree is null");
			Dev.Assert(_parentNodes.Count != 0, "Can't splice an unnested sub-tree, there must be a parent-tree.");

			(_parentNodes.Peek() as Composite).AddChild(subTree);

			return this;
		}

		public Node Build()
		{
			Dev.Assert(_curNode != null, "Can't create a behaviour tree with zero nodes");

			return _curNode;
		}

		public bool IsEmpty()
		{
			return _parentNodes.Count == 0 ? true : false;
		}

		public Builder End()
		{
			_curNode = _parentNodes.Pop();

			return this;
		}

		public void CleanUp()
		{
			_curNode.FreeAll();

			_curNode = null;
		}
	}
}
