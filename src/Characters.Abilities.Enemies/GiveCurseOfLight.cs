using System;
using UnityEngine;

namespace Characters.Abilities.Enemies;

[Serializable]
public class GiveCurseOfLight : Ability
{
	public class Instance : AbilityInstance<GiveCurseOfLight>
	{
		private float _remainCooldown;

		public Instance(Character owner, GiveCurseOfLight ability)
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
			if (!(_remainCooldown > 0f) && !((Object)(object)target.character == (Object)null) && !target.character.health.dead && target.character.playerComponents != null && (string.IsNullOrWhiteSpace(ability._attackKey) || gaveDamage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase)) && ((EnumArray<Damage.MotionType, bool>)ability._motionTypeFilter)[gaveDamage.motionType] && ((EnumArray<Damage.AttackType, bool>)ability._damageTypeFilter)[gaveDamage.attackType] && !target.character.invulnerable.value)
			{
				_remainCooldown = ability._cooldown;
				target.character.playerComponents.savableAbilityManager.Apply(SavableAbilityManager.Name.Curse);
			}
		}
	}

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private float _cooldown;

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private AttackTypeBoolArray _damageTypeFilter;

	[SerializeField]
	private string _attackKey;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
