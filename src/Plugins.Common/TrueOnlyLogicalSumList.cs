using System.Collections.Generic;

public sealed class TrueOnlyLogicalSumList
{
	private readonly List<object> _list = new List<object>();

	public bool value => _list.Count > 0;

	public TrueOnlyLogicalSumList(bool defaultValue = false)
	{
		if (defaultValue)
		{
			Attach(this);
		}
	}

	public void Attach(object key)
	{
		if (!_list.Contains(key))
		{
			_list.Add(key);
		}
	}

	public bool Detach(object key)
	{
		return _list.Remove(key);
	}
}
