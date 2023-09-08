using System;
using System.Collections;
using Characters.Movements;
using Characters.Operations.Movement;
using Characters.Utils;
using FX.BoundsAttackVisualEffect;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public sealed class RingAttack : CharacterOperation, IAttack
{
	private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(2048);

	private NonAllocOverlapper _overlapper;

	[SerializeField]
	private bool _adaptiveForce;

	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

	[SerializeField]
	private ChronoInfo _chronoToGlobe;

	[SerializeField]
	private ChronoInfo _chronoToOwner;

	[SerializeField]
	private ChronoInfo _chronoToTarget;

	[UnityEditor.Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	internal OperationInfo.Subcomponents _operationToOwnerWhenHitInfo;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private CircleCollider2D _outCollider;

	[SerializeField]
	private CircleCollider2D _inCollider;

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(2048), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 512로 두는 게 좋음.")]
	[SerializeField]
	private int _maxHits = 512;

	[SerializeField]
	private int _maxHitsPerUnit = 1;

	[SerializeField]
	private float _hitIntervalPerUnit = 0.5f;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _operationInfo;

	[BoundsAttackVisualEffect.Subcomponent]
	[SerializeField]
	private BoundsAttackVisualEffect.Subcomponents _effect;

	private PushInfo _pushInfo;

	private IAttackDamage _attackDamage;

	private CoroutineReference coroutineReference;

	private bool _timeIndependent;

	private HitHistoryManager _hits = new HitHistoryManager(32);

	public event OnAttackHitDelegate onHit;

	private void Awake()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		Array.Sort(_operationInfo.components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
		_maxHits = Math.Min(_maxHits, 2048);
		_overlapper = (NonAllocOverlapper)((_maxHits == _sharedOverlapper.capacity) ? ((object)_sharedOverlapper) : ((object)new NonAllocOverlapper(_maxHits)));
	}

	public override void Initialize()
	{
		base.Initialize();
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_operationInfo.Initialize();
		_hits.Clear();
		TargetedOperationInfo[] components = _operationInfo.components;
		foreach (TargetedOperationInfo targetedOperationInfo in components)
		{
			if (targetedOperationInfo.operation is Knockback knockback)
			{
				_pushInfo = knockback.pushInfo;
				break;
			}
			if (targetedOperationInfo.operation is Smash smash)
			{
				_pushInfo = smash.pushInfo;
			}
		}
	}

	public override void Run(Character owner)
	{
		_hits.Clear();
		coroutineReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CDetect(owner));
	}

	private IEnumerator CDetect(Character owner)
	{
		float elapsed = 0f;
		Chronometer animationChronometer = owner.chronometer.animation;
		while (elapsed < _duration)
		{
			if (_timeIndependent || animationChronometer.timeScale > float.Epsilon)
			{
				Detect(owner);
			}
			yield return null;
			elapsed = ((!_timeIndependent) ? (elapsed + animationChronometer.deltaTime) : (elapsed + Chronometer.global.deltaTime));
		}
	}

	private void Detect(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		Bounds bounds = ((Collider2D)_outCollider).bounds;
		_overlapper.OverlapCollider((Collider2D)(object)_outCollider);
		bool flag = false;
		for (int i = 0; i < _overlapper.results.Count; i++)
		{
			Target component = ((Component)_overlapper.results[i]).GetComponent<Target>();
			if ((Object)(object)component == (Object)null || !_hits.CanAttack(component, _maxHits, _maxHitsPerUnit, _hitIntervalPerUnit))
			{
				continue;
			}
			Bounds bounds2 = ((Collider2D)_inCollider).bounds;
			float num = Vector2.Distance(Vector2.op_Implicit(((Bounds)(ref bounds2)).center), component.collider.bounds.GetMostRightTop());
			bounds2 = ((Collider2D)_inCollider).bounds;
			float num2 = Vector2.Distance(Vector2.op_Implicit(((Bounds)(ref bounds2)).center), component.collider.bounds.GetMostRightBottom());
			bounds2 = ((Collider2D)_inCollider).bounds;
			float num3 = Vector2.Distance(Vector2.op_Implicit(((Bounds)(ref bounds2)).center), component.collider.bounds.GetMostLeftBottom());
			bounds2 = ((Collider2D)_inCollider).bounds;
			float num4 = Vector2.Distance(Vector2.op_Implicit(((Bounds)(ref bounds2)).center), component.collider.bounds.GetMostLeftTop());
			if (num < _inCollider.radius * ((Component)_inCollider).transform.lossyScale.x && num2 < _inCollider.radius * ((Component)_inCollider).transform.lossyScale.x && num3 < _inCollider.radius * ((Component)_inCollider).transform.lossyScale.x && num4 < _inCollider.radius * ((Component)_inCollider).transform.lossyScale.x)
			{
				continue;
			}
			Bounds bounds3 = component.collider.bounds;
			Bounds bounds4 = default(Bounds);
			((Bounds)(ref bounds4)).min = Vector2.op_Implicit(MMMaths.Max(Vector2.op_Implicit(((Bounds)(ref bounds)).min), Vector2.op_Implicit(((Bounds)(ref bounds3)).min)));
			((Bounds)(ref bounds4)).max = Vector2.op_Implicit(MMMaths.Min(Vector2.op_Implicit(((Bounds)(ref bounds)).max), Vector2.op_Implicit(((Bounds)(ref bounds3)).max)));
			Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds4);
			Vector2 force = Vector2.zero;
			if (_pushInfo != null)
			{
				(Vector2, Vector2) tuple = _pushInfo.EvaluateTimeIndependent(owner, component);
				force = tuple.Item1 + tuple.Item2;
			}
			if (_adaptiveForce)
			{
				_hitInfo.ChangeAdaptiveDamageAttribute(owner);
			}
			if ((Object)(object)component.character != (Object)null)
			{
				if (component.character.liveAndActive && !((Object)(object)component.character == (Object)(object)owner) && !component.character.cinematic.value)
				{
					_chronoToTarget.ApplyTo(component.character);
					Damage damage = owner.stat.GetDamage(_attackDamage.amount, hitPoint, _hitInfo);
					if (_hitInfo.attackType != 0)
					{
						CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)component).transform.position), bounds4, force);
					}
					flag = owner.TryAttackCharacter(component, ref damage);
					_hits.AddOrUpdate(component);
					if (flag)
					{
						((MonoBehaviour)this).StartCoroutine(_operationInfo.CRun(owner, component.character));
						this.onHit?.Invoke(component, ref damage);
						_effect.Spawn(owner, bounds4, in damage, component);
					}
				}
			}
			else if ((Object)(object)component.damageable != (Object)null)
			{
				Damage damage2 = owner.stat.GetDamage(_attackDamage.amount, hitPoint, _hitInfo);
				if (component.damageable.spawnEffectOnHit && _hitInfo.attackType != 0)
				{
					CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)component).transform.position), bounds4, force);
					_effect.Spawn(owner, bounds4, in damage2, component);
				}
				if (_hitInfo.attackType == Damage.AttackType.None)
				{
					return;
				}
				if (component.damageable.blockCast)
				{
					flag = true;
					this.onHit?.Invoke(component, ref damage2);
				}
				component.damageable.Hit(owner, ref damage2, force);
			}
		}
		if (flag)
		{
			_chronoToGlobe.ApplyGlobe();
			_chronoToOwner.ApplyTo(owner);
			if (_operationToOwnerWhenHitInfo.components.Length != 0)
			{
				((MonoBehaviour)this).StartCoroutine(_operationToOwnerWhenHitInfo.CRun(owner));
			}
		}
	}

	public override void Stop()
	{
		_operationToOwnerWhenHitInfo.StopAll();
		coroutineReference.Stop();
	}
}
