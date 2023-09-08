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
			if (!(_remainCooldown > 0f) && !((Object)(object)target.character == (Object)null) && !target.character.health.dead && target.character.playerComponents != null && (string.IsNullOrWhiteSpace(ability._attackKey) || gaveDamage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase)) && ability._motionTypeFilter[gaveDamage.motionType] && ability._damageTypeFilter[gaveDamage.attackType] && !target.character.invulnerable.value)
			{
				_remainCooldown = ability._cooldown;
				target.character.playerComponents.savableAbilityManager.Apply(SavableAbilityManager.Name.Curse);
			}
		}
	}

	[SerializeField]
	[Information("Add만 할 뿐 Remove를 하지 않으니 신중히 사용해야함", InformationAttribute.InformationType.Warning, true)]
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
