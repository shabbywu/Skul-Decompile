using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Darks;

[Serializable]
public sealed class GhostStories : Ability
{
	public sealed class Instance : AbilityInstance<GhostStories>
	{
		private float _remainCooldownTime;

		public Instance(Character owner, GhostStories ability)
			: base(owner, ability)
		{
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
		}

		protected override void OnAttach()
		{
			owner.health.onTookDamage += HandleOnTookDamage;
		}

		private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (!(_remainCooldownTime > 0f))
			{
				Character character = tookDamage.attacker.character;
				if (!((Object)(object)character == (Object)null) && ability._attackerFilter[character.type])
				{
					_remainCooldownTime = ability._cooldownTime;
					((MonoBehaviour)owner).StartCoroutine(CTeleport());
				}
			}
		}

		protected override void OnDetach()
		{
			owner.health.onTookDamage -= HandleOnTookDamage;
		}

		private IEnumerator CTeleport()
		{
			yield return Chronometer.global.WaitForSeconds(1f);
			ability._onTeleport.Run(owner);
		}
	}

	[SerializeField]
	private CharacterTypeBoolArray _attackerFilter;

	[CharacterOperation.Subcomponent]
	[SerializeField]
	private CharacterOperation.Subcomponents _onTeleport;

	[SerializeField]
	private float _cooldownTime;

	public override void Initialize()
	{
		base.Initialize();
		_onTeleport.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
