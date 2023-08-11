using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class AddMaxGaugeValue : Ability
{
	public class Instance : AbilityInstance<AddMaxGaugeValue>
	{
		internal Instance(Character owner, AddMaxGaugeValue ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			ability._gauge.maxValue += ability._amount;
		}

		protected override void OnDetach()
		{
			ability._gauge.maxValue -= ability._amount;
		}
	}

	[SerializeField]
	private ValueGauge _gauge;

	[SerializeField]
	private int _amount = 1;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
