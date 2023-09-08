using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class AttachAbilityToStatusTarget : Ability
{
	public class Instance : AbilityInstance<AttachAbilityToStatusTarget>
	{
		public Instance(Character owner, AttachAbilityToStatusTarget ability)
			: base(owner, ability)
		{
			ability._abilityComponent.Initialize();
		}

		protected override void OnAttach()
		{
			switch (ability._kind)
			{
			case CharacterStatus.Kind.Wound:
				if (!ability._onRelease)
				{
					owner.status.onApplyWound += AttachAbility;
					owner.status.onApplyBleed += DetachAbility;
				}
				else
				{
					owner.status.onApplyBleed += DetachAbility;
				}
				break;
			case CharacterStatus.Kind.Burn:
				if (!ability._onRelease)
				{
					owner.status.onApplyBurn += AttachAbility;
					owner.status.onReleaseBurn += DetachAbility;
				}
				else
				{
					owner.status.onReleaseBurn += DetachAbility;
				}
				break;
			case CharacterStatus.Kind.Freeze:
				if (!ability._onRelease)
				{
					owner.status.onApplyFreeze += AttachAbility;
					owner.status.onReleaseFreeze += DetachAbility;
				}
				else
				{
					owner.status.onReleaseFreeze += AttachAbility;
				}
				break;
			case CharacterStatus.Kind.Poison:
				if (!ability._onRelease)
				{
					owner.status.onApplyPoison += AttachAbility;
					owner.status.onReleasePoison += DetachAbility;
				}
				else
				{
					owner.status.onReleasePoison += AttachAbility;
				}
				break;
			case CharacterStatus.Kind.Stun:
				if (!ability._onRelease)
				{
					owner.status.onApplyStun += AttachAbility;
					owner.status.onReleaseStun += DetachAbility;
				}
				else
				{
					owner.status.onReleaseStun += AttachAbility;
				}
				break;
			}
		}

		protected override void OnDetach()
		{
			switch (ability._kind)
			{
			case CharacterStatus.Kind.Wound:
				if (!ability._onRelease)
				{
					owner.status.onApplyWound -= AttachAbility;
					owner.status.onApplyBleed -= DetachAbility;
				}
				else
				{
					owner.status.onApplyBleed -= AttachAbility;
				}
				break;
			case CharacterStatus.Kind.Burn:
				if (!ability._onRelease)
				{
					owner.status.onApplyBurn -= AttachAbility;
					owner.status.onReleaseBurn -= DetachAbility;
				}
				else
				{
					owner.status.onReleaseBurn -= AttachAbility;
				}
				break;
			case CharacterStatus.Kind.Freeze:
				if (!ability._onRelease)
				{
					owner.status.onApplyFreeze -= AttachAbility;
					owner.status.onReleaseFreeze -= DetachAbility;
				}
				else
				{
					owner.status.onReleaseFreeze -= AttachAbility;
				}
				break;
			case CharacterStatus.Kind.Poison:
				if (!ability._onRelease)
				{
					owner.status.onApplyPoison -= AttachAbility;
					owner.status.onReleasePoison -= DetachAbility;
				}
				else
				{
					owner.status.onReleasePoison -= AttachAbility;
				}
				break;
			case CharacterStatus.Kind.Stun:
				if (!ability._onRelease)
				{
					owner.status.onApplyStun -= AttachAbility;
					owner.status.onReleaseStun -= DetachAbility;
				}
				else
				{
					owner.status.onReleasePoison -= AttachAbility;
				}
				break;
			}
		}

		private void AttachAbility(Character owner, Character target)
		{
			target.ability.Add(ability._abilityComponent.ability);
		}

		private void DetachAbility(Character owner, Character target)
		{
			if (!target.health.dead)
			{
				target.ability.Remove(ability._abilityComponent.ability);
			}
		}
	}

	[SerializeField]
	[Tooltip("제거는 따로 안시켜줌")]
	private bool _onRelease;

	[SerializeField]
	internal CharacterStatus.Kind _kind;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _abilityComponent;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
