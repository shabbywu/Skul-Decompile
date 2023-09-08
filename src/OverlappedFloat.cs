using System.Collections.Generic;

public class OverlappedFloat
{
	private readonly Dictionary<object, float> _values = new Dictionary<object, float>();

	public float value { get; private set; }

	public OverlappedFloat(float defaultValue = 0f)
	{
		Attach(this, defaultValue);
	}

	public void SetDefault(float defaultValue)
	{
		_values[this] = defaultValue;
	}

	public void Attach(object owner, float bonus)
	{
		_values.Add(owner, bonus);
		value += bonus;
	}

	public void Change(object owner, float bonus)
	{
		value -= _values[owner];
		_values[owner] = bonus;
	}

	public void ChangeOrAttach(object owner, float bonus)
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
