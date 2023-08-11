using System;
using Data;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class WhaleboneAmulet : Ability
{
	public sealed class Instance : AbilityInstance<WhaleboneAmulet>
	{
		public Instance(Character owner, WhaleboneAmulet ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTakeDamage.Add(-100, (TakeDamageDelegate)HandleOnTakeDamage);
		}

		private bool HandleOnTakeDamage(ref Damage damage)
		{
			if (owner.invulnerable.value || owner.evasion.value)
			{
				return false;
			}
			if (damage.@null)
			{
				return false;
			}
			if (damage.amount < 1.0)
			{
				return false;
			}
			int num = (int)(damage.amount / (double)ability._reduceDamagePerBone);
			if (num > 0 && GameData.Currency.bone.Has(num))
			{
				GameData.Currency.bone.Consume(num);
				damage.@null = true;
				return false;
			}
			if (GameData.Currency.bone.Has(1))
			{
				if (num == 0)
				{
					GameData.Currency.bone.Consume(1);
					damage.@null = true;
					return false;
				}
				GameData.Currency.bone.Consume(GameData.Currency.bone.balance);
				int num2 = ability._reduceDamagePerBone * GameData.Currency.bone.balance;
				damage.extraFixedDamage = -num2;
			}
			return false;
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove((TakeDamageDelegate)HandleOnTakeDamage);
		}
	}

	[SerializeField]
	private int _reduceDamagePerBone;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
