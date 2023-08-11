using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public sealed class StatBonusByTargetStatus : Ability
{
	public class Instance : AbilityInstance<StatBonusByTargetStatus>
	{
		public Instance(Character owner, StatBonusByTargetStatus ability)
			: base(owner, ability)
		{
			ability._statBonus.Initialize();
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
			owner.ability.Remove(ability._statBonus);
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if ((Object)(object)target.character == (Object)null)
			{
				return;
			}
			switch (ability._kind)
			{
			case CharacterStatus.Kind.Wound:
				if (target.character.status.wounded)
				{
					owner.ability.Add(ability._statBonus);
				}
				break;
			case CharacterStatus.Kind.Burn:
				if (target.character.status.burning)
				{
					owner.ability.Add(ability._statBonus);
				}
				break;
			case CharacterStatus.Kind.Freeze:
				if (target.character.status.freezed)
				{
					owner.ability.Add(ability._statBonus);
				}
				break;
			case CharacterStatus.Kind.Poison:
				if (target.character.status.poisoned)
				{
					owner.ability.Add(ability._statBonus);
				}
				break;
			case CharacterStatus.Kind.Stun:
				if (target.character.status.stuned)
				{
					owner.ability.Add(ability._statBonus);
				}
				break;
			case CharacterStatus.Kind.Unmoving:
				if (target.character.status.unmovable)
				{
					owner.ability.Add(ability._statBonus);
				}
				break;
			}
		}
	}

	[SerializeField]
	internal CharacterStatus.Kind _kind;

	[SerializeField]
	private StatBonus _statBonus;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
