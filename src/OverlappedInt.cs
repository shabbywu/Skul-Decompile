using System.Collections.Generic;

public class OverlappedInt
{
	private readonly Dictionary<object, int> _values = new Dictionary<object, int>();

	public int value { get; private set; }

	public OverlappedInt(int defaultValue = 0)
	{
		Attach(this, defaultValue);
	}

	public void SetDefault(int defaultValue)
	{
		_values[this] = defaultValue;
	}

	public void Attach(object owner, int bonus)
	{
		_values.Add(owner, bonus);
		value += bonus;
	}

	public void Change(object owner, int bonus)
	{
		value -= _values[owner];
		_values[owner] = bonus;
	}

	public void ChangeOrAttach(object owner, int bonus)
	{
		if (_values.ContainsKey(owner))
		{
			Change(owner, bonus);
			return;
		}
		_values.Add(owner, bonus);
		value += bonus;
	}

	public void Remove(object owner)
	{
		if (_values.TryGetValue(owner, out var num))
		{
			value -= num;
			_values.Remove(owner);
		}
	}
}
