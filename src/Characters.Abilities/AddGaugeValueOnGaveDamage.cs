using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class AddGaugeValueOnGaveDamage : Ability
{
	public class Instance : AbilityInstance<AddGaugeValueOnGaveDamage>
	{
		internal Instance(Character owner, AddGaugeValueOnGaveDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (!((Object)(object)target.character == (Object)null) && ((EnumArray<Damage.MotionType, bool>)ability._attackTypes)[tookDamage.motionType] && ((EnumArray<Damage.AttackType, bool>)ability._types)[tookDamage.attackType] && MMMaths.PercentChance(ability._chance) && (string.IsNullOrWhiteSpace(ability._attackKey) || tookDamage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase)))
			{
				float num = (tookDamage.critical ? ability._amountOnCritical : ability._amount);
				if (ability._multiplierByDamageDealt)
				{
					num *= (float)damageDealt;
				}
				ability._gauge.Add(num);
			}
		}
	}

	[Serializable]
	private class AttackTypeBoolArray : EnumArray<Damage.MotionType, bool>
	{
	}

	[Serializable]
	private class DamageTypeBoolArray : EnumArray<Damage.AttackType, bool>
	{
	}

	[SerializeField]
	private ValueGauge _gauge;

	[Range(1f, 100f)]
	[SerializeField]
	private int _chance = 100;

	[SerializeField]
	private int _amount = 1;

	[SerializeField]
	private int _amountOnCritical = 1;

	[SerializeField]
	private bool _multiplierByDamageDealt;

	[SerializeField]
	private string _attackKey;

	[SerializeField]
	private AttackTypeBoolArray _attackTypes;

	[SerializeField]
	private DamageTypeBoolArray _types;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
