public class SumFloat : Sum<float>
{
	public SumFloat(float @base)
		: base(@base)
	{
	}

	public void Add(object key, float value)
	{
		_sum.Add(key, value);
		base.total += value;
	}

	public override void AddOrUpdate(object key, float value)
	{
		if (_sum.ContainsKey(key))
		{
			_sum[key] = value;
			ComputeValue();
		}
		else
		{
			_sum.Add(key, value);
			base.total += value;
		}
	}

	public override void ComputeValue()
	{
		base.total = base.@base;
		foreach (float value in _sum.Values)
		{
			base.total += value;
		}
	}
}
