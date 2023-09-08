using System;

namespace Characters.Abilities;

[Serializable]
public class IgnoreShieldOverDamage : Ability
{
	public class Instance : AbilityInstance<IgnoreShieldOverDamage>
	{
		public Instance(Character owner, IgnoreShieldOverDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTakeDamage.Add(int.MinValue, OnOwnerTakeDamage);
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove(OnOwnerTakeDamage);
		}

		private bool OnOwnerTakeDamage(ref Damage damage)
		{
			if (!owner.health.shield.hasAny)
			{
				return false;
			}
			if (owner.health.shield.amount > damage.amount)
			{
				return false;
			}
			owner.health.shield.Consume(damage.amount);
			return true;
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
