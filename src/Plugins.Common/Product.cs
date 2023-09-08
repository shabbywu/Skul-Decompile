using System.Collections.Generic;

public class Product<T>
{
	protected readonly Dictionary<object, T> _multiplication = new Dictionary<object, T>();

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
			return _multiplication[key];
		}
		set
		{
			AddOrUpdate(key, value);
		}
	}

	public Product(T @base)
	{
		_base = @base;
		total = _base;
		Add(this, @base);
	}

	public void Add(object key, T value)
	{
		_multiplication.Add(key, value);
		total *= (dynamic)value;
	}

	public void AddOrUpdate(object key, T value)
	{
		if (_multiplication.ContainsKey(key))
		{
			_multiplication[key] = value;
			ComputeValue();
		}
		else
		{
			_multiplication.Add(key, value);
			total *= (dynamic)value;
		}
	}

	public bool Contains(object key)
	{
		return _multiplication.ContainsKey(key);
	}

	public bool Remove(object owner)
	{
		if (_multiplication.Remove(owner))
		{
			ComputeValue();
			return true;
		}
		return false;
	}

	public void Clear()
	{
		_multiplication.Clear();
		total = _base;
	}

	private void ComputeValue()
	{
		total = @base;
		foreach (T value in _multiplication.Values)
		{
			total *= (dynamic)value;
		}
	}
}
