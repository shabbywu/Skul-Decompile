using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ApplyStatusOnGaveEmberDamage : Ability
{
	public class Instance : AbilityInstance<ApplyStatusOnGaveEmberDamage>
	{
		private float _elapsed;

		private bool _canUse = true;

		internal Instance(Character owner, ApplyStatusOnGaveEmberDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.status.onGaveEmberDamage += HandleOnGaveEmberDamage;
			_elapsed = 0f;
		}

		protected override void OnDetach()
		{
			owner.status.onGaveEmberDamage -= HandleOnGaveEmberDamage;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (ability._cooldownTime == 0f)
			{
				_canUse = true;
				return;
			}
			_elapsed += deltaTime;
			if (_elapsed >= ability._cooldownTime)
			{
				_canUse = true;
				_elapsed -= ability._cooldownTime;
			}
		}

		private void HandleOnGaveEmberDamage(Character attacker, Character target)
		{
			if (_canUse && !((Object)(object)target == (Object)null) && !((Object)(object)target == (Object)(object)owner) && MMMaths.PercentChance(ability._chance))
			{
				owner.GiveStatus(target, ability._status);
				_canUse = false;
			}
		}
	}

	[Tooltip("default는 0초")]
	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private CharacterStatus.ApplyInfo _status;

	[Range(1f, 100f)]
	[SerializeField]
	private int _chance = 100;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
