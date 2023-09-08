using System.Collections.Generic;

public class MultiMap<TKey, TValue> : Dictionary<TKey, List<TValue>>
{
	public int CountAll
	{
		get
		{
			int num = 0;
			foreach (List<TValue> value in base.Values)
			{
				num += value.Count;
			}
			return num;
		}
	}

	public MultiMap()
	{
	}

	public MultiMap(int capacity)
		: base(capacity)
	{
	}

	public void Add(TKey key, TValue value)
	{
		if (TryGetValue(key, out var value2))
		{
			value2.Add(value);
			return;
		}
		value2 = new List<TValue>();
		value2.Add(value);
		Add(key, value2);
	}

	public bool Remove(TKey key, TValue value)
	{
		if (TryGetValue(key, out var value2) && value2.Remove(value))
		{
			if (value2.Count == 0)
			{
				Remove(key);
			}
			return true;
		}
		return false;
	}

	public int RemoveAll(TKey key, TValue value)
	{
		int num = 0;
		if (TryGetValue(key, out var value2))
		{
			while (value2.Remove(value))
			{
				num++;
			}
			if (value2.Count == 0)
			{
				Remove(key);
			}
		}
		return num;
	}

	public bool Contains(TKey key, TValue value)
	{
		if (TryGetValue(key, out var value2))
		{
			return value2.Contains(value);
		}
		return false;
	}

	public bool Contains(TValue value)
	{
		foreach (List<TValue> value2 in base.Values)
		{
			if (value2.Contains(value))
			{
				return true;
			}
		}
		return false;
	}
}
