using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.Utility.Collection
{
	public class MinHeap<T> where T : IComparable<T>
	{
		private readonly List<T> _elements;

		public int Count { get { return this._elements.Count; } }

		public MinHeap() : this(16) { }

		public MinHeap(int capacity)
		{
			this._elements = new List<T>(capacity);
		}

		public void Clear()
		{
			this._elements.Clear();
		}

		public void Add(T item)
		{
			this._elements.Add(item);
			this.BubbleUp();
		}

		public T Pop()
		{
			int count = this._elements.Count;
			if (count <= 0)
			{
				throw new Exception("no element in MinHeap");
			}

			if (count == 1)
			{
				T value = this._elements[0];
				this._elements.Clear();
				return value;
			}

			T minValue = this._elements[0];
			int lastIndex = this._elements.Count - 1;
			this._elements[0] = this._elements[lastIndex];
			this._elements.RemoveAt(lastIndex);

			this.SinkDown();
			return minValue;
		}

		public T PeekFirst()
		{
			if (this._elements.Count <= 0)
			{
				throw new Exception("no element in MinHeap");
			}

			return this._elements[0];
		}

		public void RemoveFirst()
		{
			int count = this._elements.Count;
			if (count <= 0)
			{
				throw new Exception("no element in MinHeap");
			}

			if (count == 1)
			{
				this._elements.Clear();
				return;
			}

			int lastIndex = this._elements.Count - 1;
			this._elements[0] = this._elements[lastIndex];
			this._elements.RemoveAt(lastIndex);

			this.SinkDown();
		}

		private void BubbleUp()
		{
			int index = this._elements.Count - 1;
			while (index > 0)
			{
				int parentIndex = (index - 1) / 2;
				int compared = this._elements[index].CompareTo(this._elements[parentIndex]);
				if (compared < 0)
				{
					this.Swap(index, parentIndex);
					index = parentIndex;
					continue;
				}

				return;
			}
		}

		private void SinkDown()
		{
			int index = 0;
			int lastIndex = this._elements.Count;

			while (index < lastIndex)
			{
				int childL = index * 2 + 1;
				if (childL >= lastIndex)
				{
					return;
				}

				int childR = childL + 1;
				if (childR >= lastIndex)
				{
					int comparedL_only = this._elements[index].CompareTo(this._elements[childL]);
					if (comparedL_only > 0)
					{
						this.Swap(index, childL);
						index = childL;
						continue;
					}
					return;
				}

				int comparedL = this._elements[index].CompareTo(this._elements[childL]);
				if (comparedL > 0)
				{
					int comparedChild = this._elements[childL].CompareTo(this._elements[childR]);
					if (comparedChild < 0)
					{
						this.Swap(index, childL);
						index = childL;
						continue;
					}
					else
					{
						this.Swap(index, childR);
						index = childR;
						continue;
					}
				}

				int comparedR = this._elements[index].CompareTo(this._elements[childR]);
				if (comparedR > 0)
				{
					this.Swap(index, childR);
					index = childR;
					continue;
				}
				return;
			}
		}

		void Swap(int index1, int index2)
		{
			T temp = this._elements[index1];
			this._elements[index1] = this._elements[index2];
			this._elements[index2] = temp;
		}
	}

	public class MinHeap<TKey, TValue> where TKey : IComparable<TKey>
	{
		private readonly List<KeyValuePair<TKey, TValue>> _elements;

		public int Count { get { return this._elements.Count; } }

		public MinHeap() : this(16) { }

		public MinHeap(int capacity)
		{
			this._elements = new List<KeyValuePair<TKey, TValue>>(capacity);
		}

		public void Clear()
		{
			this._elements.Clear();
		}

		public void Add(KeyValuePair<TKey, TValue> pair)
		{
			this._elements.Add(pair);
			this.BubbleUp();
		}
		public void Add(TKey key, TValue value)
		{
			this._elements.Add(new KeyValuePair<TKey, TValue>(key, value));
			this.BubbleUp();
		}

		public KeyValuePair<TKey, TValue> Pop()
		{
			int count = this._elements.Count;
			if (count <= 0)
			{
				throw new Exception("No element in MinHeap");
			}

			if (count == 1)
			{
				KeyValuePair<TKey, TValue> pair = this._elements[0];
				this._elements.Clear();
				return pair;
			}

			KeyValuePair<TKey, TValue> minPair = this._elements[0];
			int lastIndex = this._elements.Count - 1;
			this._elements[0] = this._elements[lastIndex];
			this._elements.RemoveAt(lastIndex);

			this.SinkDown();
			return minPair;
		}

		public KeyValuePair<TKey, TValue> PeekFirst()
		{
			if (this._elements.Count <= 0)
			{
				throw new Exception("no element in MinHeap");
			}

			return this._elements[0];
		}

		public void RemoveFirst()
		{
			int count = this._elements.Count;
			if (count <= 0)
			{
				throw new Exception("no element in MinHeap");
			}

			if (count == 1)
			{
				this._elements.Clear();
				return;
			}

			int lastIndex = this._elements.Count - 1;
			this._elements[0] = this._elements[lastIndex];
			this._elements.RemoveAt(lastIndex);

			this.SinkDown();
		}

		private void BubbleUp()
		{
			int index = this._elements.Count - 1;
			while (index > 0)
			{
				int parentIndex = (index - 1) / 2;
				int compared = this._elements[index].Key.CompareTo(this._elements[parentIndex].Key);
				if (compared < 0)
				{
					this.Swap(index, parentIndex);
					index = parentIndex;
					continue;
				}

				return;
			}
		}

		private void SinkDown()
		{
			int index = 0;
			int lastIndex = this._elements.Count;

			while (index < lastIndex)
			{
				int childL = index * 2 + 1;
				if (childL >= lastIndex)
				{
					return;
				}

				int childR = childL + 1;
				if (childR >= lastIndex)
				{
					int comparedL_only = this._elements[index].Key.CompareTo(this._elements[childL].Key);
					if (comparedL_only > 0)
					{
						this.Swap(index, childL);
						index = childL;
						continue;
					}
					return;
				}

				int comparedL = this._elements[index].Key.CompareTo(this._elements[childL].Key);
				if (comparedL > 0)
				{
					int comparedChild = this._elements[childL].Key.CompareTo(this._elements[childR].Key);
					if (comparedChild < 0)
					{
						this.Swap(index, childL);
						index = childL;
						continue;
					}
					else
					{
						this.Swap(index, childR);
						index = childR;
						continue;
					}
				}

				int comparedR = this._elements[index].Key.CompareTo(this._elements[childR].Key);
				if (comparedR > 0)
				{
					this.Swap(index, childR);
					index = childR;
					continue;
				}
				return;
			}
		}

		void Swap(int index1, int index2)
		{
			KeyValuePair<TKey, TValue> temp = this._elements[index1];
			this._elements[index1] = this._elements[index2];
			this._elements[index2] = temp;
		}
	}
}
