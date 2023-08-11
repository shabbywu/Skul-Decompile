using System;
using Characters.Abilities.CharacterStat;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class HotTag : Ability
{
	public class Instance : AbilityInstance<HotTag>
	{
		private int _stack;

		private bool _attached;

		public override int iconStacks => _stack;

		public Instance(Character owner, HotTag ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
			owner.health.onTookDamage += OnTookDamage;
			owner.playerComponents.inventory.weapon.onSwap += OnSwap;
			_attached = false;
		}

		private void OnSwap()
		{
			owner.ability.Add(ability._stackablStatBonus);
			ability._stackablStatBonus.stack = _stack;
			_attached = true;
			_stack = 0;
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
			owner.health.onTookDamage -= OnTookDamage;
			owner.playerComponents.inventory.weapon.onSwap -= OnSwap;
			owner.ability.Remove(ability._stackablStatBonus);
			_stack = 0;
			_attached = false;
		}

		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (((EnumArray<Damage.AttackType, bool>)ability._attackType)[tookDamage.attackType])
			{
				AddStack();
			}
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (((EnumArray<Damage.AttackType, bool>)ability._attackType)[gaveDamage.attackType])
			{
				AddStack();
			}
		}

		private void AddStack()
		{
			_stack = ((_stack + 1 >= ability._maxStack) ? ability._maxStack : (_stack + 1));
		}
	}

	[SerializeField]
	private StackableStatBonus _stackablStatBonus;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private AttackTypeBoolArray _attackType;

	[SerializeField]
	private AttackTypeBoolArray _hitType;

	public override void Initialize()
	{
		base.Initialize();
		_stackablStatBonus.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
