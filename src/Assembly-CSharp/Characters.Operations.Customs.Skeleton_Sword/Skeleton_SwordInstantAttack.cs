using System;
using System.Collections;
using Characters.Abilities.Weapons.Skeleton_Sword;
using Characters.Operations.Attack;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Customs.Skeleton_Sword;

public sealed class Skeleton_SwordInstantAttack : CharacterOperation, IAttack
{
	private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(2048);

	private NonAllocOverlapper _overlapper;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(2048), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 99로 두는 게 좋음.")]
	private int _maxHits = 512;

	[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
	[SerializeField]
	private bool _optimizedCollider = true;

	[SerializeField]
	[Tooltip("공격자 자기 자신을 대상에서 제외할지 여부")]
	private bool _excludeItself = true;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(BoundsAttackInfoSequence))]
	private BoundsAttackInfoSequence.Subcomponents _attackAndEffect;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(BoundsAttackInfoSequence))]
	private BoundsAttackInfoSequence.Subcomponents _tetanusAttackAndEffect;

	private IAttackDamage _attackDamage;

	public event OnAttackHitDelegate onHit;

	private void Awake()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (_optimizedCollider && (Object)(object)_collider != (Object)null)
		{
			((Behaviour)_collider).enabled = false;
		}
		_maxHits = Math.Min(_maxHits, 2048);
		_overlapper = (NonAllocOverlapper)((_maxHits == _sharedOverlapper.capacity) ? ((object)_sharedOverlapper) : ((object)new NonAllocOverlapper(_maxHits)));
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_attackAndEffect.Initialize();
	}

	public override void Stop()
	{
		_attackAndEffect.StopAllOperationsToOwner();
	}

	public override void Run(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		((Behaviour)_collider).enabled = true;
		_overlapper.OverlapCollider(_collider);
		Bounds bounds = _collider.bounds;
		if (_optimizedCollider)
		{
			((Behaviour)_collider).enabled = false;
		}
		ReadonlyBoundedList<Collider2D> results = _overlapper.results;
		for (int i = 0; i < results.Count; i++)
		{
			Target component = ((Component)results[i]).GetComponent<Target>();
			if ((Object)(object)component == (Object)null)
			{
				Debug.LogError((object)"Target is null in InstantAttack2");
				break;
			}
			if (_attackAndEffect.noDelay)
			{
				BoundsAttackInfoSequence[] array = ((component.character.ability.GetInstance<Skeleton_SwordTatanusDamage>() != null) ? _tetanusAttackAndEffect.components : _attackAndEffect.components);
				BoundsAttackInfoSequence[] array2 = array;
				foreach (BoundsAttackInfoSequence boundsAttackInfoSequence in array2)
				{
					Attack(owner, bounds, component, boundsAttackInfoSequence.attackInfo);
				}
			}
			else
			{
				((MonoBehaviour)component).StartCoroutine(CAttack(owner, bounds, component));
			}
		}
	}

	private void Attack(Character owner, Bounds bounds, Target target, BoundsAttackInfo attackInfo)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)target == (Object)null)
		{
			Debug.LogError((object)"Target is null in InstantAttack2");
		}
		else
		{
			if (!((Behaviour)target).isActiveAndEnabled)
			{
				return;
			}
			Bounds val = bounds;
			Bounds bounds2 = target.collider.bounds;
			Bounds bounds3 = default(Bounds);
			((Bounds)(ref bounds3)).min = Vector2.op_Implicit(MMMaths.Max(Vector2.op_Implicit(((Bounds)(ref val)).min), Vector2.op_Implicit(((Bounds)(ref bounds2)).min)));
			((Bounds)(ref bounds3)).max = Vector2.op_Implicit(MMMaths.Min(Vector2.op_Implicit(((Bounds)(ref val)).max), Vector2.op_Implicit(((Bounds)(ref bounds2)).max)));
			Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds3);
			Vector2 force = Vector2.zero;
			if (attackInfo.pushInfo != null)
			{
				(Vector2, Vector2) tuple = attackInfo.pushInfo.EvaluateTimeIndependent(owner, target);
				force = tuple.Item1 + tuple.Item2;
			}
			if ((Object)(object)target.character != (Object)null)
			{
				if (target.character.liveAndActive && (!_excludeItself || !((Object)(object)target.character == (Object)(object)owner)) && !target.character.cinematic.value)
				{
					attackInfo.ApplyChrono(owner, target.character);
					if (attackInfo.operationsToOwner.components.Length != 0)
					{
						((MonoBehaviour)owner).StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
					}
					Damage damage = owner.stat.GetDamage(_attackDamage.amount, hitPoint, attackInfo.hitInfo);
					if (attackInfo.hitInfo.attackType != 0)
					{
						CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
					}
					if (target.character.evasion.value)
					{
						owner.TryAttackCharacter(target, ref damage);
						return;
					}
					((MonoBehaviour)owner).StartCoroutine(attackInfo.operationInfo.CRun(owner, target.character));
					this.onHit?.Invoke(target, ref damage);
					owner.TryAttackCharacter(target, ref damage);
					attackInfo.effect.Spawn(owner, bounds3, in damage, target);
				}
			}
			else if ((Object)(object)target.damageable != (Object)null)
			{
				attackInfo.ApplyChrono(owner);
				((MonoBehaviour)owner).StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
				Damage damage2 = owner.stat.GetDamage(_attackDamage.amount, hitPoint, attackInfo.hitInfo);
				if (target.damageable.spawnEffectOnHit && attackInfo.hitInfo.attackType != 0)
				{
					CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
					attackInfo.effect.Spawn(owner, bounds3, in damage2, target);
				}
				if (attackInfo.hitInfo.attackType != 0)
				{
					this.onHit?.Invoke(target, ref damage2);
					target.damageable.Hit(owner, ref damage2, force);
				}
			}
		}
	}

	private IEnumerator CAttack(Character owner, Bounds bounds, Target target)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		int index = 0;
		float time = 0f;
		BoundsAttackInfoSequence[] attackAndEfects = ((target.character.ability.GetInstance<Skeleton_SwordTatanusDamage>() != null) ? _tetanusAttackAndEffect.components : _attackAndEffect.components);
		BoundsAttackInfoSequence[] array = attackAndEfects;
		for (int i = 0; i < array.Length; i++)
		{
			_ = array[i];
			while ((Object)(object)this != (Object)null && index < attackAndEfects.Length)
			{
				for (; index < attackAndEfects.Length; index++)
				{
					BoundsAttackInfoSequence boundsAttackInfoSequence;
					if (!(time >= (boundsAttackInfoSequence = attackAndEfects[index]).timeToTrigger))
					{
						break;
					}
					Attack(owner, bounds, target, boundsAttackInfoSequence.attackInfo);
				}
				yield return null;
				time += owner.chronometer.animation.deltaTime;
			}
		}
	}
}
