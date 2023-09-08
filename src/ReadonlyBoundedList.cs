using System;
using System.Collections;
using System.Collections.Generic;

public class ReadonlyBoundedList<T> : IEnumerable, IEnumerable<T>, IReadOnlyList<T>, IReadOnlyCollection<T>
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private readonly ReadonlyBoundedList<T> _list;

		private int _index;

		public T Current { get; private set; }

		object IEnumerator.Current => Current;

		internal Enumerator(ReadonlyBoundedList<T> list)
		{
			_list = list;
			_index = 0;
			Current = default(T);
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (_index == _list.Count)
			{
				return false;
			}
			Current = _list._itmes[_index++];
			return true;
		}

		public void Reset()
		{
			_index = 0;
			Current = default(T);
		}
	}

	protected readonly T[] _itmes;

	public int capacity => _itmes.Length;

	public int Count { get; protected set; }

	public T this[int index]
	{
		get
		{
			return _itmes[index];
		}
		protected set
		{
			if (index >= Count)
			{
				throw new IndexOutOfRangeException();
			}
			_itmes[index] = value;
		}
	}

	public ReadonlyBoundedList(int capacity)
	{
		_itmes = new T[capacity];
		Count = capacity;
	}

	public ReadonlyBoundedList(T[] sourceArray)
	{
		_itmes = (T[])sourceArray.Clone();
		Count = sourceArray.Length;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(this);
	}

	public int IndexOf(T item)
	{
		return Array.IndexOf(_itmes, item, 0, Count);
	}

	public bool Contains(T item)
	{
		return IndexOf(item) != -1;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		Array.Copy(_itmes, 0, array, arrayIndex, Count);
	}
}
