using System.Collections.Generic;

public abstract class Sum<T>
{
	protected readonly Dictionary<object, T> _sum = new Dictionary<object, T>();

	protected T _base;

	public T total { get; protected set; }

	public T @base
	{
		get
		{
			return _base;
		}
		set
		{
			_base = value;
			ComputeValue();
		}
	}

	public T this[object key]
	{
		get
		{
			return _sum[key];
		}
		set
		{
			AddOrUpdate(key, value);
		}
	}

	public abstract void ComputeValue();

	public abstract void AddOrUpdate(object key, T value);

	public Sum()
	{
		_base = default(T);
		total = default(T);
	}

	public Sum(T @base)
	{
		_base = @base;
		total = @base;
	}

	public bool Contains(object key)
	{
		return _sum.ContainsKey(key);
	}

	public bool Remove(object owner)
	{
		if (_sum.Remove(owner))
		{
			ComputeValue();
			return true;
		}
		return false;
	}

	public void Clear()
	{
		_sum.Clear();
		total = _base;
	}
}
