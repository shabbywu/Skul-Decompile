using System;
using UnityEngine;

namespace Characters.Abilities.Weapons.Wizard;

[Serializable]
public sealed class WizardManaChargingSpeedBonus : Ability
{
	public class Instance : AbilityInstance<WizardManaChargingSpeedBonus>
	{
		public Instance(Character owner, WizardManaChargingSpeedBonus ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			ability._passive.manaChargingSpeedMultiplier += ability._multiplier;
		}

		protected override void OnDetach()
		{
			ability._passive.manaChargingSpeedMultiplier -= ability._multiplier;
		}
	}

	[SerializeField]
	private WizardPassiveComponent _passive;

	[SerializeField]
	private float _multiplier;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
