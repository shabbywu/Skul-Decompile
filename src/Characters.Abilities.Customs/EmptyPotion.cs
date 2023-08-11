using System;
using Characters.Gear.Items;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public sealed class EmptyPotion : Ability
{
	public class Instance : AbilityInstance<EmptyPotion>
	{
		public int stack;

		public override int iconStacks => stack;

		public Instance(Character owner, EmptyPotion ability)
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

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (!((Object)(object)target.character == (Object)null) && target.character.status.wounded && ((EnumArray<Damage.AttackType, bool>)ability._attackType)[gaveDamage.attackType])
			{
				stack++;
				if (stack >= ability._maxStack)
				{
					Heal();
					RemoveItem();
				}
			}
		}

		private void Heal()
		{
			owner.health.Heal(ability._healAmount.value);
		}

		private void RemoveItem()
		{
			ability._emptyPotion.RemoveOnInventory();
			owner.ability.Remove(this);
		}
	}

	[SerializeField]
	private AttackTypeBoolArray _attackType;

	[SerializeField]
	private Item _emptyPotion;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private CustomFloat _healAmount;

	private Instance _instance;

	public int stack
	{
		get
		{
			return _instance.stack;
		}
		set
		{
			_instance.stack = value;
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return _instance = new Instance(owner, this);
	}
}
