using System;
using UnityEngine;

namespace Characters.Abilities;

public sealed class FirstAttackAbilityAttacher : AbilityAttacher
{
	public sealed class FirstAttackMark : Ability
	{
		public sealed class Instance : AbilityInstance<FirstAttackMark>
		{
			public Instance(Character owner, FirstAttackMark ability)
				: base(owner, ability)
			{
			}

			protected override void OnAttach()
			{
			}

			protected override void OnDetach()
			{
			}
		}

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	[SerializeField]
	private CharacterTypeBoolArray _targetTypeFilter;

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private AttackTypeBoolArray _attackTypeFilter;

	[SerializeField]
	private DamageAttributeBoolArray _attributeFilter;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _abilityComponent;

	private FirstAttackMark _mark;

	public override void OnIntialize()
	{
		_abilityComponent.Initialize();
		_mark = new FirstAttackMark
		{
			duration = 2.1474836E+09f
		};
		_mark.Initialize();
	}

	public override void StartAttach()
	{
		Character character = base.owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
	}

	private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		Character character = target.character;
		if (!((Object)(object)character == (Object)null) && _targetTypeFilter[character.type] && _motionTypeFilter[gaveDamage.motionType] && _attackTypeFilter[gaveDamage.attackType] && _attributeFilter[gaveDamage.attribute] && !character.ability.Contains(_mark))
		{
			character.ability.Add(_mark);
			base.owner.ability.Add(_abilityComponent.ability);
		}
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			Character character = base.owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			base.owner.ability.Remove(_abilityComponent.ability);
		}
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
