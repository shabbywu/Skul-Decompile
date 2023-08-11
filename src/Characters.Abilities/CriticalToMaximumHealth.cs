using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class CriticalToMaximumHealth : Ability
{
	public class Instance : AbilityInstance<CriticalToMaximumHealth>
	{
		private float _remainCooldownTime;

		internal Instance(Character owner, CriticalToMaximumHealth ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(0, (GiveDamageDelegate)OnGiveDamage);
		}

		protected override void OnDetach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			CharacterHealth characterHealth = target.character?.health;
			if ((Object)(object)characterHealth == (Object)null || characterHealth.percent < 1.0)
			{
				return false;
			}
			damage.criticalChance = 1.0;
			_remainCooldownTime = ability._cooldownTime;
			return false;
		}
	}

	[SerializeField]
	private float _cooldownTime;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
