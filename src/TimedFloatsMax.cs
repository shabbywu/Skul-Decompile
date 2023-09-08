using UnityEngine;

public class TimedFloatsMax : TimedFloats
{
	private float _max;

	public override float value => Mathf.Max(_staticValue, _max);

	public override void Add(float value, float time)
	{
		base.Add(value, time);
		if (_max < value)
		{
			_max = value;
		}
	}

	protected override void RemoveAt(int index)
	{
		base.RemoveAt(index);
		_max = float.MinValue;
		for (int i = 0; i < _values.Count; i++)
		{
			float num = _values[i];
			if (num > _max)
			{
				_max = num;
			}
		}
	}

	public override void Clear()
	{
		base.Clear();
		_max = float.MinValue;
	}
}
