using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations;
using FX;
using GameResources;
using PhysicsUtils;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class StigmaLeg : Ability
{
	[Serializable]
	public sealed class Debuff : Ability
	{
		public sealed class Instance : AbilityInstance<Debuff>
		{
			private ParticleEffectInfo _hitParticle;

			private float _remainCooldownTime;

			public Instance(Character owner, Debuff ability)
				: base(owner, ability)
			{
				_hitParticle = CommonResource.instance.hitParticle;
			}

			protected override void OnAttach()
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				if ((Object)(object)ability.attacker != (Object)null)
				{
					PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attachSoundInfo, ((Component)owner).transform.position);
					ability.attacker.onStartAction += HandleOnStartAction;
				}
			}

			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				//IL_011a: Unknown result type (might be due to invalid IL or missing references)
				//IL_011f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0187: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
				if ((action.type != Characters.Actions.Action.Type.Skill && action.type != 0) || _remainCooldownTime > 0f)
				{
					return;
				}
				if ((Object)(object)owner == (Object)null)
				{
					ability.attacker.onStartAction -= HandleOnStartAction;
					return;
				}
				if ((Object)(object)ability._targetPoint != (Object)null)
				{
					Bounds bounds = ((Collider2D)owner.collider).bounds;
					Vector3 center = ((Bounds)(ref bounds)).center;
					bounds = ((Collider2D)owner.collider).bounds;
					Vector3 size = ((Bounds)(ref bounds)).size;
					size.x *= ability._positionInfo.pivotValue.x;
					size.y *= ability._positionInfo.pivotValue.y;
					Vector3 position = center + size;
					ability._targetPoint.position = position;
				}
				Character attacker = ability.attacker;
				Damage damage = attacker.stat.GetDamage(ability._additionalHitDamage.value, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), ability._hitInfo);
				_remainCooldownTime = ability._coodownTime;
				((MonoBehaviour)attacker).StartCoroutine(ability._operations.CRun(attacker));
				attacker.Attack(owner, ref damage);
				PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attackSoundInfo, ((Component)owner).transform.position);
				_hitParticle.Emit(Vector2.op_Implicit(((Component)owner).transform.position), ((Collider2D)owner.collider).bounds, Vector2.zero);
			}

			protected override void OnDetach()
			{
				if ((Object)(object)ability.attacker != (Object)null)
				{
					ability.attacker.onStartAction -= HandleOnStartAction;
				}
			}

			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				_remainCooldownTime -= deltaTime;
			}
		}

		[SerializeField]
		private float _coodownTime;

		[SerializeField]
		private CustomFloat _additionalHitDamage;

		[SerializeField]
		private HitInfo _hitInfo;

		[SerializeField]
		private SoundInfo _attachSoundInfo;

		[SerializeField]
		private SoundInfo _attackSoundInfo;

		[SerializeField]
		private PositionInfo _positionInfo;

		[SerializeField]
		private Transform _targetPoint;

		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;

		public Character attacker { get; set; }

		public override void Initialize()
		{
			base.Initialize();
			_operations.Initialize();
		}

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	public sealed class Instance : AbilityInstance<StigmaLeg>
	{
		private readonly NonAllocCaster _caster;

		private Character _stigmaTarget;

		public Instance(Character owner, StigmaLeg ability)
			: base(owner, ability)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			_caster = new NonAllocCaster(12);
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
				if ((Object)(object)_stigmaTarget != (Object)null && !_stigmaTarget.health.dead)
				{
					yield return null;
					continue;
				}
				if (animationChronometer.timeScale > float.Epsilon)
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
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
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
				if (!((Object)(object)component.character == (Object)null))
				{
					component.character.ability.Add(ability._debuff);
					_stigmaTarget = component.character;
				}
				break;
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
		_debuff.attacker = owner;
		return new Instance(owner, this);
	}
}
