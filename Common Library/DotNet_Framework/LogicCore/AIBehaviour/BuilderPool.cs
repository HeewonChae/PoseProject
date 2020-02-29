using LogicCore.Utility;
using LogicCore.Utility.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
