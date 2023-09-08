using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Operations.Gauge;

public class AddGaugeValue : Operation
{
	[SerializeField]
	private ValueGauge _gaugeWithValue;

	[SerializeField]
	private float _amount = 1f;

	public override void Run()
	{
		if (!((Object)(object)_gaugeWithValue == (Object)null))
		{
			_gaugeWithValue.Add(_amount);
		}
	}
}
