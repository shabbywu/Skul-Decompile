using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public sealed class StatDebuffOnStatus : Ability
{
	public class Instance : AbilityInstance<StatDebuffOnStatus>
	{
		public Instance(Character owner, StatDebuffOnStatus ability)
			: base(owner, ability)
		{
			ability._statBonus.Initialize();
		}

		protected override void OnAttach()
		{
			switch (ability._kind)
			{
			case CharacterStatus.Kind.Wound:
				if (!ability._onRelease)
				{
					owner.status.onApplyWound += AttachStatDebuff;
					owner.status.onApplyBleed += DetachStatDebuff;
				}
				else
				{
					owner.status.onApplyBleed += DetachStatDebuff;
				}
				break;
			case CharacterStatus.Kind.Burn:
				if (!ability._onRelease)
				{
					owner.status.onApplyBurn += AttachStatDebuff;
					owner.status.onReleaseBurn += DetachStatDebuff;
				}
				else
				{
					owner.status.onReleaseBurn += DetachStatDebuff;
				}
				break;
			case CharacterStatus.Kind.Freeze:
				if (!ability._onRelease)
				{
					owner.status.onApplyFreeze += AttachStatDebuff;
					owner.status.onReleaseFreeze += DetachStatDebuff;
				}
				else
				{
					owner.status.onReleaseFreeze += AttachStatDebuff;
				}
				break;
			case CharacterStatus.Kind.Poison:
				if (!ability._onRelease)
				{
					owner.status.onApplyPoison += AttachStatDebuff;
					owner.status.onReleasePoison += DetachStatDebuff;
				}
				else
				{
					owner.status.onReleasePoison += AttachStatDebuff;
				}
				break;
			case CharacterStatus.Kind.Stun:
				if (!ability._onRelease)
				{
					owner.status.onApplyStun += AttachStatDebuff;
					owner.status.onReleaseStun += DetachStatDebuff;
				}
				else
				{
					owner.status.onReleaseStun += AttachStatDebuff;
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
					owner.status.onApplyWound -= AttachStatDebuff;
					owner.status.onApplyBleed -= DetachStatDebuff;
				}
				else
				{
					owner.status.onApplyBleed -= AttachStatDebuff;
				}
				break;
			case CharacterStatus.Kind.Burn:
				if (!ability._onRelease)
				{
					owner.status.onApplyBurn -= AttachStatDebuff;
					owner.status.onReleaseBurn -= DetachStatDebuff;
				}
				else
				{
					owner.status.onReleaseBurn -= AttachStatDebuff;
				}
				break;
			case CharacterStatus.Kind.Freeze:
				if (!ability._onRelease)
				{
					owner.status.onApplyFreeze -= AttachStatDebuff;
					owner.status.onReleaseFreeze -= DetachStatDebuff;
				}
				else
				{
					owner.status.onReleaseFreeze -= AttachStatDebuff;
				}
				break;
			case CharacterStatus.Kind.Poison:
				if (!ability._onRelease)
				{
					owner.status.onApplyPoison -= AttachStatDebuff;
					owner.status.onReleasePoison -= DetachStatDebuff;
				}
				else
				{
					owner.status.onReleasePoison -= AttachStatDebuff;
				}
				break;
			case CharacterStatus.Kind.Stun:
				if (!ability._onRelease)
				{
					owner.status.onApplyStun -= AttachStatDebuff;
					owner.status.onReleaseStun -= DetachStatDebuff;
				}
				else
				{
					owner.status.onReleasePoison -= AttachStatDebuff;
				}
				break;
			}
		}

		private void AttachStatDebuff(Character owner, Character target)
		{
			target.ability.Add(ability._statBonus);
		}

		private void DetachStatDebuff(Character owner, Character target)
		{
			if (!target.health.dead)
			{
				target.ability.Remove(ability._statBonus);
			}
		}
	}

	[SerializeField]
	private bool _onRelease;

	[SerializeField]
	internal CharacterStatus.Kind _kind;

	[SerializeField]
	private StatBonus _statBonus;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
