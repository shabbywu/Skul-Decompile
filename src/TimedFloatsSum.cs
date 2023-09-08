public class TimedFloatsSum : TimedFloats
{
	private float _sum;

	public override float value => _staticValue + _sum;

	public override void Add(float value, float time)
	{
		base.Add(value, time);
		_sum += value;
	}

	protected override void RemoveAt(int index)
	{
		_sum -= _values[index];
		base.RemoveAt(index);
	}

	public override void Clear()
	{
		_sum = 0f;
		base.Clear();
	}
}
