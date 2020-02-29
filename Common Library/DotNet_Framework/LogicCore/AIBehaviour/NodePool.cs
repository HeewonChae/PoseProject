using LogicCore.Utility;
using LogicCore.Utility.Pool;

namespace LogicCore.AIBehaviour
{
	public class NodePool<NodeType> : Singleton.INode where NodeType : Node, new()
	{
		private readonly LightPool<NodeType> _nodePool = new LightPool<NodeType>(() => new NodeType());

		public NodeType Alloc()
		{
			return _nodePool.Alloc();
		}

		public void Free(NodeType node)
		{
			_nodePool.Free(node);
		}
	}
}