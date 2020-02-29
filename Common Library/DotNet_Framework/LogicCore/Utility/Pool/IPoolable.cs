namespace LogicCore.Utility.Pool
{
	public interface IPoolable
	{
		void OnAlloc();

		void OnFree();
	}
}