using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.Utility.Pool
{
	public interface IPoolable
	{
		void OnAlloc();

		void OnFree();
	}
}
