using LogicCore.Debug;
using System;
using System.Collections.Generic;

namespace LogicCore.Utility.Pool
{
	public class LightPool<T> where T : IPoolable
	{
		private readonly Stack<T> _items;
		private readonly Func<T> _generator;

		private int _capacity;

		public LightPool(Func<T> generator, int initialCapacity = 128)
		{
			Dev.Assert(generator != null, "generator is null");

			this._generator = generator;
			this._capacity = initialCapacity;
			this._items = new Stack<T>(initialCapacity);

			for (int count = 0; count < initialCapacity; count++)
			{
				this._items.Push(this._generator());
			}
		}

		public void Resize(int newCapacity)
		{
			if (this._capacity >= newCapacity)
			{
				return;
			}
			for (int count = this._capacity; count < newCapacity; count++)
			{
				this._items.Push(this._generator());
			}

			this._capacity = newCapacity;
		}

		public T Alloc()
		{
			if (this._items.Count > 0)
			{
				T item = this._items.Pop();
				item.OnAlloc();
				return item;
			}
			else
			{
				T item = this._generator();
				item.OnAlloc();
				return item;
			}
		}

		public void Free(T item)
		{
			item.OnFree();
			this._items.Push(item);
		}
	}
}