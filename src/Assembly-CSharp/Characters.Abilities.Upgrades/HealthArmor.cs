using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class HealthArmor : Ability
{
	public sealed class Instance : AbilityInstance<HealthArmor>
	{
		public Instance(Character owner, HealthArmor ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			float plus = (float)owner.health.percent * ability._reduceHealthPercent * ability._shieldRatio;
			owner.playerComponents.savableAbilityManager.IncreaseStack(SavableAbilityManager.Name.PurchasedMaxHealth, 0f - ability._reduceHealthPercent);
			owner.playerComponents.savableAbilityManager.IncreaseStack(SavableAbilityManager.Name.PurchasedShield, plus);
		}

		protected override void OnDetach()
		{
		}
	}

	[SerializeField]
	private float _reduceHealthPercent;

	[SerializeField]
	private float _shieldRatio;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
