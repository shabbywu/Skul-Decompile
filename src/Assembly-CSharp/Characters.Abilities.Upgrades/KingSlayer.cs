using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class KingSlayer : Ability
{
	public sealed class Instance : AbilityInstance<KingSlayer>
	{
		[Serializable]
		public sealed class KingMark : Ability
		{
			public sealed class Instance : AbilityInstance<KingMark>
			{
				private ITarget _target;

				private EffectPoolInstance _markEffect;

				private bool _killing;

				public Instance(Character owner, KingMark ability)
					: base(owner, ability)
				{
					_target = ((Component)owner).GetComponentInChildren<ITarget>();
					owner.health.onTookDamage += HandleOnTookDamage;
				}

				private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
				{
					if (!((Object)(object)ability._attacker == (Object)null) && !_killing && owner.health.percent * 100.0 <= (double)ability._kinSlayer.triggerPercent)
					{
						owner.health.onTookDamage -= HandleOnTookDamage;
						_killing = true;
						Invoke();
					}
				}

				private void Invoke()
				{
					if (!((Object)(object)ability._attacker == (Object)null))
					{
						((MonoBehaviour)ability._attacker).StartCoroutine(CDelayedKill());
					}
				}

				private IEnumerator CDelayedKill()
				{
					Bounds bounds = ((Collider2D)owner.collider).bounds;
					Vector3 center = ((Bounds)(ref bounds)).center;
					_markEffect = ability._markEffect.Spawn(center, owner);
					owner.chronometer.animation.AttachTimeScale(this, ability._timeScaleDuringKilling);
					EffectInfo killEffect = ability._killEffect;
					bounds = ((Collider2D)owner.collider).bounds;
					killEffect.Spawn(((Bounds)(ref bounds)).center, owner);
					PersistentSingleton<SoundManager>.Instance.PlaySound(ability._markSound, ((Component)owner).gameObject.transform.position);
					yield return owner.chronometer.master.WaitForSeconds(1f);
					if ((Object)(object)_markEffect != (Object)null)
					{
						_markEffect.Stop();
					}
					_markEffect = null;
					if ((Object)(object)owner == (Object)null || !((Component)owner).gameObject.activeSelf)
					{
						yield break;
					}
					if (owner.health.dead)
					{
						owner.chronometer.animation.DetachTimeScale(this);
						yield break;
					}
					owner.chronometer.animation.DetachTimeScale(this);
					Attacker attacker = ability._attacker;
					double currentHealth = owner.health.currentHealth;
					bounds = ((Collider2D)owner.collider).bounds;
					Damage damage = new Damage(attacker, currentHealth, Vector2.op_Implicit(((Bounds)(ref bounds)).center), Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.DarkAbility, 1.0, 0f, 0.0, 1.0, 1.0, canCritical: true, @null: false, 0.0, 0.0, 0);
					((MonoBehaviour)ability._attacker).StartCoroutine(ability._onKilled.CRun(ability._attacker));
					ability._attacker.TryKillTarget(_target, ref damage);
					if (!((Object)(object)owner == (Object)null) && !owner.health.dead && ((Component)owner).gameObject.activeSelf)
					{
						_killing = false;
						owner.ability.Remove(this);
					}
				}

				protected override void OnAttach()
				{
				}

				protected override void OnDetach()
				{
					owner.health.onTookDamage -= HandleOnTookDamage;
					if ((Object)(object)_markEffect != (Object)null)
					{
						_markEffect.Stop();
					}
					_markEffect = null;
				}
			}

			[SerializeField]
			private KingSlayerComponent _kinSlayer;

			[Header("처형 연출")]
			[SerializeField]
			private EffectInfo _markEffect = new EffectInfo
			{
				subordinated = true
			};

			[SerializeField]
			private SoundInfo _markSound;

			[SerializeField]
			private EffectInfo _killEffect;

			[SerializeField]
			[Subcomponent(typeof(OperationInfo))]
			private OperationInfo.Subcomponents _onKilled;

			[SerializeField]
			private float _timeScaleDuringKilling = 0.3f;

			private Character _attacker;

			public void SetAttacker(Character attacker)
			{
				_attacker = attacker;
			}

			public override void Initialize()
			{
				base.Initialize();
				_onKilled.Initialize();
			}

			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new Instance(owner, this);
			}
		}

		private HashSet<Character> _targets;

		public Instance(Character owner, KingSlayer ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_targets = new HashSet<Character>();
			Singleton<Service>.Instance.levelManager.onMapLoaded += HandleOnMapLoaded;
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		private void HandleOnMapLoaded()
		{
			_targets.Clear();
		}

		protected override void OnDetach()
		{
			if (!Service.quitting)
			{
				Singleton<Service>.Instance.levelManager.onMapLoaded -= HandleOnMapLoaded;
				Character character = owner;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
			}
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (!((Object)(object)target.character == (Object)null) && !target.character.health.dead && (string.IsNullOrWhiteSpace(ability._attackKey) || gaveDamage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase)) && ability._targetFilter[target.character.type] && ability._motionTypeFilter[gaveDamage.motionType] && ability._attackTypeFilter[gaveDamage.attackType] && !_targets.Contains(target.character))
			{
				(target.character.ability.Add(ability._kingMark) as KingMark.Instance).ability.SetAttacker(owner);
				_targets.Add(target.character);
			}
		}
	}

	[SerializeField]
	private CharacterTypeBoolArray _targetFilter = new CharacterTypeBoolArray(true, true, true, true, true);

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private AttackTypeBoolArray _attackTypeFilter;

	[SerializeField]
	private string _attackKey;

	[SerializeField]
	private Instance.KingMark _kingMark;

	public override void Initialize()
	{
		base.Initialize();
		_kingMark.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
