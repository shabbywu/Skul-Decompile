using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class GiveStatusOnGaveDamage : Ability
{
	public class Instance : AbilityInstance<GiveStatusOnGaveDamage>
	{
		private float _remainCooldown;

		public override float iconFillAmount => _remainCooldown / ability._cooldown;

		public Instance(Character owner, GiveStatusOnGaveDamage ability)
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

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldown -= deltaTime;
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (!(_remainCooldown > 0f) && !((Object)(object)target.character == (Object)null) && !target.character.health.dead && ((EnumArray<Damage.MotionType, bool>)ability._motionTypeFilter)[gaveDamage.motionType] && ((EnumArray<Damage.AttackType, bool>)ability._damageTypeFilter)[gaveDamage.attackType])
			{
				_remainCooldown = ability._cooldown;
				owner.GiveStatus(target.character, ability._status);
			}
		}
	}

	[SerializeField]
	private float _cooldown;

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private AttackTypeBoolArray _damageTypeFilter;

	[SerializeField]
	private CharacterStatus.ApplyInfo _status;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
