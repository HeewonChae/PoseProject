using LogicCore.Utility;
using LogicCore.Utility.Pool;

namespace LogicCore.AIBehaviour
{
	public class BuilderPool : Singleton.INode
	{
		private readonly LightPool<Builder> _builderPool = new LightPool<Builder>(() => new Builder());

		public Builder Alloc()
		{
			return _builderPool.Alloc();
		}

		public void Free(Builder builder)
		{
			_builderPool.Free(builder);
		}
	}
}