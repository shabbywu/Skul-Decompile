using System;
using System.Collections;
using System.Collections.Generic;

public class BoundedList<T> : ReadonlyBoundedList<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	public T[] array => _itmes;

	public new int Count
	{
		get
		{
			return base.Count;
		}
		set
		{
			if (value < 0 || value > _itmes.Length)
			{
				throw new IndexOutOfRangeException();
			}
			base.Count = value;
		}
	}

	public bool IsReadOnly => false;

	public new T this[int index]
	{
		get
		{
			return base[index];
		}
		set
		{
			if (index >= Count)
			{
				throw new IndexOutOfRangeException();
			}
			base[index] = value;
		}
	}

	public BoundedList(int capacity)
		: base(capacity)
	{
	}

	public BoundedList(T[] sourceArray)
		: base(sourceArray)
	{
	}

	public ReadonlyBoundedList<T> AsReadonly()
	{
		return this;
	}

	public void Insert(int index, T item)
	{
		_itmes[Count++] = item;
	}

	public void RemoveAt(int index)
	{
		if (index >= Count)
		{
			throw new IndexOutOfRangeException();
		}
		for (int i = index; i < Count - 1; i++)
		{
			_itmes[i] = _itmes[i + 1];
		}
		Count--;
	}

	public void Add(T item)
	{
		_itmes[Count++] = item;
	}

	public void Clear()
	{
		Count = 0;
	}

	public bool Remove(T item)
	{
		int num = IndexOf(item);
		if (num == -1)
		{
			return false;
		}
		RemoveAt(num);
		return true;
	}
}
