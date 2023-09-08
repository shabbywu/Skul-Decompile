using System.Collections.Generic;

public class ProductFloat
{
	protected readonly Dictionary<object, float> _multiplication = new Dictionary<object, float>();

	protected float _base;

	public float total { get; protected set; }

	public float @base
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

	public float this[object key]
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

	public ProductFloat(float @base)
	{
		_base = @base;
		total = _base;
		Add(this, @base);
	}

	public void Add(object key, float value)
	{
		_multiplication.Add(key, value);
		total *= value;
	}

	public void AddOrUpdate(object key, float value)
	{
		if (_multiplication.ContainsKey(key))
		{
			_multiplication[key] = value;
			ComputeValue();
		}
		else
		{
			_multiplication.Add(key, value);
			total *= value;
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
		foreach (float value in _multiplication.Values)
		{
			total *= value;
		}
	}
}
