public class SumDouble : Sum<double>
{
	public SumDouble(double @base)
		: base(@base)
	{
	}

	public void Add(object key, double value)
	{
		_sum.Add(key, value);
		base.total += value;
	}

	public override void AddOrUpdate(object key, double value)
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
		foreach (double value in _sum.Values)
		{
			base.total += value;
		}
	}
}
