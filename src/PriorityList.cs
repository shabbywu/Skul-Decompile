using System;
using System.Collections;
using System.Collections.Generic;

public class PriorityList<T> : IEnumerable<T>, IEnumerable
{
	public struct PriorityWrapper
	{
		public int priority;

		public T value;

		internal PriorityWrapper(int priority, T value)
		{
			this.priority = priority;
			this.value = value;
		}
	}

	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private readonly List<PriorityWrapper> _list;

		private int _index;

		public T Current { get; private set; }

		object IEnumerator.Current => Current;

		internal Enumerator(List<PriorityWrapper> list)
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
			Current = _list[_index++].value;
			return true;
		}

		public void Reset()
		{
			_index = 0;
			Current = default(T);
		}
	}

	protected readonly List<PriorityWrapper> _items = new List<PriorityWrapper>();

	public int Count => _items.Count;

	public T this[int index] => _items[index].value;

	public void Add(int priority, T item)
	{
		PriorityWrapper item2 = new PriorityWrapper(priority, item);
		for (int i = 0; i < _items.Count; i++)
		{
			if (priority > _items[i].priority)
			{
				_items.Insert(i, item2);
				return;
			}
		}
		_items.Add(item2);
	}

	public void Clear()
	{
		_items.Clear();
	}

	public bool Contains(T item)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if (_items[i].value.Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	public int IndexOf(T item)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if (_items[i].value.Equals(item))
			{
				return i;
			}
		}
		return -1;
	}

	public bool Remove(T item)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if (_items[i].value.Equals(item))
			{
				_items.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	public void RemoveAt(int index)
	{
		_items.RemoveAt(index);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return new Enumerator(_items);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(_items);
	}
}
