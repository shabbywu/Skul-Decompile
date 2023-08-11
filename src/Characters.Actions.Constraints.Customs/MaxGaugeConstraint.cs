using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs;

public sealed class MaxGaugeConstraint : Constraint
{
	[SerializeField]
	private ValueGauge _gauge;

	public override bool Pass()
	{
		return _gauge.maxValue <= _gauge.currentValue;
	}
}
