using System;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class ThornsArmor : Ability
{
	public sealed class Instance : AbilityInstance<ThornsArmor>
	{
		private float _remainCooldownTime;

		public Instance(Character owner, ThornsArmor ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_remainCooldownTime = ability._cooldownTime;
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attachAudioClipInfo, ((Component)owner).transform.position);
			owner.health.onTookDamage += HandleOnTookDamage;
		}

		private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			Character character = tookDamage.attacker.character;
			if (!(_remainCooldownTime > 0f) && !((Object)(object)character == (Object)null) && ability._characterType[character.type] && ability._motionType[tookDamage.motionType] && ability._attackType[tookDamage.attackType] && !(damageDealt < (double)ability._minDamage))
			{
				_remainCooldownTime = ability._cooldownTime;
				float value = ability._damage.value;
				Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(value, MMMaths.RandomPointWithinBounds(((Collider2D)character.collider).bounds));
				character.health.TakeHealth(value);
				((MonoBehaviour)character).StartCoroutine(ability._onHit.CRun(character));
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
		}

		protected override void OnDetach()
		{
			owner.health.onTookDamage -= HandleOnTookDamage;
		}
	}

	[SerializeField]
	private SoundInfo _attachAudioClipInfo;

	[SerializeField]
	private CharacterTypeBoolArray _characterType;

	[SerializeField]
	private MotionTypeBoolArray _motionType;

	[SerializeField]
	private AttackTypeBoolArray _attackType;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onHit;

	[SerializeField]
	private float _minDamage;

	[SerializeField]
	private float _cooldownTime = 1f;

	[SerializeField]
	private CustomFloat _damage;

	public override void Initialize()
	{
		base.Initialize();
		_onHit.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
