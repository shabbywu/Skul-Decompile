using System;
using System.Collections;
using Characters.Marks;
using Characters.Operations.Attack;
using GameResources;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Customs;

public sealed class Samurai2IlseomInstantAttack : CharacterOperation, IAttack
{
	private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(2048);

	private NonAllocOverlapper _overlapper;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(2048), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 512로 두는 게 좋음.")]
	[SerializeField]
	private int _maxHits = 512;

	[SerializeField]
	[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
	private bool _optimizedCollider = true;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _operationInfo;

	[SerializeField]
	[Space]
	private MarkInfo _mark;

	[SerializeField]
	[Tooltip("표식이 없을 때인 0개부터 시작")]
	private double[] _damagePercents;

	[SerializeField]
	[FrameTime]
	private float _attack1Time;

	[UnityEditor.Subcomponent(typeof(BoundsAttackInfo))]
	[SerializeField]
	private BoundsAttackInfo _attack1;

	[FrameTime]
	[SerializeField]
	private float _attack2Time;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(BoundsAttackInfo))]
	private BoundsAttackInfo _attack2;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(OperationInfo))]
	internal OperationInfo.Subcomponents _operationOnMaxStackHit;

	private IAttackDamage _attackDamage;

	public Collider2D range
	{
		get
		{
			return _collider;
		}
		set
		{
			_collider = value;
		}
	}

	public event OnAttackHitDelegate onHit;

	private void Awake()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		Array.Sort(_operationInfo.components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
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
		_attack1.Initialize();
		_attack2.Initialize();
	}

	public override void Stop()
	{
		_operationOnMaxStackHit.StopAll();
	}

	public override void Run(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		((Behaviour)_collider).enabled = true;
		_overlapper.OverlapCollider(_collider);
		Bounds bounds = _collider.bounds;
		if (_optimizedCollider)
		{
			((Behaviour)_collider).enabled = false;
		}
		ReadonlyBoundedList<Collider2D> results = _overlapper.results;
		bool flag = false;
		for (int i = 0; i < results.Count; i++)
		{
			Target component = ((Component)results[i]).GetComponent<Target>();
			if ((Object)(object)component == (Object)null)
			{
				Debug.LogError((object)"Target is null in InstantAttack2");
				return;
			}
			float num = 0f;
			if ((Object)(object)component.character != (Object)null)
			{
				num = component.character.mark.TakeAllStack(_mark);
				if (num == (float)_mark.maxStack)
				{
					flag = true;
				}
			}
			((MonoBehaviour)component).StartCoroutine(CAttack(owner, bounds, component, num));
		}
		if (flag && _operationOnMaxStackHit.components.Length != 0)
		{
			((MonoBehaviour)this).StartCoroutine(_operationOnMaxStackHit.CRun(owner));
		}
	}

	private void Attack(Character owner, Bounds bounds, Target target, BoundsAttackInfo attackInfo, double multiplier = 1.0)
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
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
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
				if (target.character.liveAndActive)
				{
					attackInfo.ApplyChrono(owner, target.character);
					if (attackInfo.operationsToOwner.components.Length != 0)
					{
						((MonoBehaviour)owner).StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
					}
					Damage damage = owner.stat.GetDamage((double)_attackDamage.amount * multiplier, hitPoint, attackInfo.hitInfo);
					if (attackInfo.hitInfo.attackType != 0)
					{
						CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
					}
					if (target.character.cinematic.value)
					{
						attackInfo.effect.Spawn(owner, bounds3, in damage, target);
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

	private IEnumerator CAttack(Character owner, Bounds bounds, Target target, float stacks)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float time = 0f;
		while ((Object)(object)this != (Object)null && time < _attack1Time)
		{
			yield return null;
			time += owner.chronometer.animation.deltaTime;
		}
		Attack(owner, bounds, target, _attack1);
		while ((Object)(object)this != (Object)null && time < _attack2Time)
		{
			yield return null;
			time += owner.chronometer.animation.deltaTime;
		}
		float damageMultiplier = _attack2.hitInfo.damageMultiplier;
		int num = (int)math.min(stacks, (float)_mark.maxStack);
		Attack(owner, bounds, target, _attack2, _damagePercents[num]);
		_attack2.hitInfo.damageMultiplier = damageMultiplier;
	}
}
