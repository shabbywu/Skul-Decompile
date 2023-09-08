public class SumInt : Sum<int>
{
	public SumInt(int @base)
		: base(@base)
	{
	}

	public void Add(object key, int value)
	{
		_sum.Add(key, value);
		base.total += value;
	}

	public override void AddOrUpdate(object key, int value)
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
		foreach (int value in _sum.Values)
		{
			base.total += value;
		}
	}
}
