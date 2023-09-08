using System.Collections.Generic;

public abstract class TimedFloats
{
	protected readonly List<float> _values = new List<float>();

	protected readonly List<float> _times = new List<float>();

	protected float _staticValue;

	public abstract float value { get; }

	public TimedFloats()
	{
	}

	public TimedFloats(float defaultValue)
	{
		_staticValue = defaultValue;
	}

	public void SetDefault(float defaultValue)
	{
		_staticValue = defaultValue;
	}

	public virtual void Add(float value, float time)
	{
		_values.Add(value);
		_times.Add(time);
	}

	public virtual void Clear()
	{
		_values.Clear();
		_times.Clear();
	}

	public void TakeTime(float time)
	{
		for (int num = _times.Count - 1; num >= 0; num--)
		{
			if ((_times[num] -= time) <= 0f)
			{
				RemoveAt(num);
			}
		}
	}

	protected virtual void RemoveAt(int index)
	{
		_values.RemoveAt(index);
		_times.RemoveAt(index);
	}
}
