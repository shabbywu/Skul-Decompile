using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations;
using FX;
using GameResources;
using PhysicsUtils;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class ManeOfBeastKing : Ability
{
	[Serializable]
	public sealed class Debuff : Ability
	{
		public sealed class Instance : AbilityInstance<Debuff>
		{
			private ParticleEffectInfo _hitParticle;

			private bool _canUse;

			public Instance(Character owner, Debuff ability)
				: base(owner, ability)
			{
				_hitParticle = CommonResource.instance.hitParticle;
			}

			protected override void OnAttach()
			{
				_canUse = true;
				owner.health.onTakeDamage.Add(int.MinValue, (TakeDamageDelegate)OnTakeDamage);
				owner.health.onTookDamage += OnTookDamage;
			}

			private bool OnTakeDamage(ref Damage damage)
			{
				if (!damage.canCritical)
				{
					return false;
				}
				if (!_canUse)
				{
					return false;
				}
				damage.criticalChance = 1.0;
				damage.Evaluate(immuneToCritical: false);
				_canUse = false;
				return false;
			}

			private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0114: Unknown result type (might be due to invalid IL or missing references)
				//IL_0130: Unknown result type (might be due to invalid IL or missing references)
				//IL_0135: Unknown result type (might be due to invalid IL or missing references)
				//IL_0145: Unknown result type (might be due to invalid IL or missing references)
				//IL_014a: Unknown result type (might be due to invalid IL or missing references)
				if (((EnumArray<Damage.AttackType, bool>)ability._attackType)[tookDamage.attackType] && ((EnumArray<Damage.MotionType, bool>)ability._motionType)[tookDamage.motionType] && ((EnumArray<Character.Type, bool>)ability._attackerType)[tookDamage.attacker.character.type])
				{
					owner.health.onTookDamage -= OnTookDamage;
					Character character = tookDamage.attacker.character;
					Damage damage = character.stat.GetDamage(ability._additionalHitDamage.value, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), ability._additionalHitInfo);
					character.Attack(owner, ref damage);
					ability._hitEffect.Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds)));
					PersistentSingleton<SoundManager>.Instance.PlaySound(ability._hitSound, ((Component)owner).transform.position);
					_hitParticle.Emit(Vector2.op_Implicit(((Component)owner).transform.position), ((Collider2D)owner.collider).bounds, Vector2.zero);
					owner.ability.Remove(this);
				}
			}

			protected override void OnDetach()
			{
				owner.health.onTakeDamage.Remove((TakeDamageDelegate)OnTakeDamage);
				owner.health.onTookDamage -= OnTookDamage;
			}
		}

		[SerializeField]
		private CustomFloat _additionalHitDamage;

		[SerializeField]
		private HitInfo _additionalHitInfo;

		[SerializeField]
		private MotionTypeBoolArray _motionType;

		[SerializeField]
		private AttackTypeBoolArray _attackType;

		[SerializeField]
		private CharacterTypeBoolArray _attackerType;

		[SerializeField]
		private EffectInfo _hitEffect;

		[SerializeField]
		private SoundInfo _hitSound;

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	public sealed class Instance : AbilityInstance<ManeOfBeastKing>
	{
		private readonly NonAllocCaster _caster;

		public Instance(Character owner, ManeOfBeastKing ability)
			: base(owner, ability)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			_caster = new NonAllocCaster(99);
		}

		protected override void OnAttach()
		{
			owner.onStartAction += OnStartAction;
		}

		private void OnStartAction(Characters.Actions.Action action)
		{
			if (action.type == Characters.Actions.Action.Type.Dash)
			{
				((MonoBehaviour)owner).StartCoroutine(CCheckTargetCollision(action));
			}
		}

		private IEnumerator CCheckTargetCollision(Characters.Actions.Action dash)
		{
			Chronometer animationChronometer = owner.chronometer.animation;
			while (dash.running)
			{
				if (((ChronometerBase)animationChronometer).timeScale > float.Epsilon)
				{
					Vector2 val = Vector2.zero;
					if ((Object)(object)owner.movement != (Object)null)
					{
						val = owner.movement.moved;
					}
					Detect(((Vector2)(ref val)).normalized, ((Vector2)(ref val)).magnitude);
				}
				yield return null;
			}
		}

		private void Detect(Vector2 direction, float distance)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
			_caster.ColliderCast((Collider2D)(object)owner.collider, direction, distance);
			for (int i = 0; i < _caster.results.Count; i++)
			{
				RaycastHit2D val = _caster.results[i];
				Target component = ((Component)((RaycastHit2D)(ref val)).collider).GetComponent<Target>();
				if ((Object)(object)component == (Object)null)
				{
					Debug.LogError((object)(((Object)((RaycastHit2D)(ref val)).collider).name + " : Character has no Target component"));
					continue;
				}
				if ((Object)(object)component.character == (Object)null)
				{
					break;
				}
				component.character.ability.Add(ability._debuff);
			}
		}

		protected override void OnDetach()
		{
			owner.onStartAction -= OnStartAction;
			((MonoBehaviour)owner).StopCoroutine("CCheckTargetCollision");
		}
	}

	[SerializeField]
	private Debuff _debuff;

	public override void Initialize()
	{
		base.Initialize();
		_debuff.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
