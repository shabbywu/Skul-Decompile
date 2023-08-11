using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class StatusRegrant : Ability
{
	public sealed class Instance : AbilityInstance<StatusRegrant>
	{
		private CharacterStatus.ApplyInfo _burn;

		private CharacterStatus.ApplyInfo _freeze;

		private CharacterStatus.ApplyInfo _poison;

		private CharacterStatus.ApplyInfo _stun;

		private CharacterStatus.ApplyInfo _wound;

		public Instance(Character owner, StatusRegrant ability)
			: base(owner, ability)
		{
			_burn = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Burn);
			_freeze = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Freeze);
			_poison = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Poison);
			_stun = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Stun);
			_wound = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Wound);
		}

		protected override void OnAttach()
		{
			owner.status.onReleaseBurn += HandleOnReleaseBurn;
			owner.status.onReleaseFreeze += HandleOnReleaseFreeze;
			owner.status.onReleasePoison += HandleOnReleasePoison;
			owner.status.onReleaseStun += HandleOnReleaseStun;
			owner.status.onApplyWound += HandleOnApplyWound;
		}

		private void HandleOnApplyWound(Character attacker, Character target)
		{
			if (MMMaths.PercentChance(ability._chance))
			{
				owner.GiveStatus(target, _wound);
			}
		}

		private void HandleOnReleaseStun(Character attacker, Character target)
		{
			if (MMMaths.PercentChance(ability._chance))
			{
				owner.GiveStatus(target, _stun);
			}
		}

		private void HandleOnReleasePoison(Character attacker, Character target)
		{
			if (MMMaths.PercentChance(ability._chance))
			{
				owner.GiveStatus(target, _poison);
			}
		}

		private void HandleOnReleaseFreeze(Character attacker, Character target)
		{
			if (MMMaths.PercentChance(ability._chance))
			{
				owner.GiveStatus(target, _freeze);
			}
		}

		private void HandleOnReleaseBurn(Character attacker, Character target)
		{
			if (MMMaths.PercentChance(ability._chance))
			{
				owner.GiveStatus(target, _burn);
			}
		}

		protected override void OnDetach()
		{
			owner.status.onReleaseBurn -= HandleOnReleaseBurn;
			owner.status.onReleaseFreeze -= HandleOnReleaseFreeze;
			owner.status.onReleasePoison -= HandleOnReleasePoison;
			owner.status.onReleaseStun -= HandleOnReleaseStun;
			owner.status.onApplyWound -= HandleOnApplyWound;
		}
	}

	[SerializeField]
	[Range(0f, 100f)]
	private int _chance;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
