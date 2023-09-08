using System;
using Characters.Movements;
using Characters.Operations.Movement;
using FX.BoundsAttackVisualEffect;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public sealed class InstantAttack : CharacterOperation, IAttack
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

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(OperationInfo))]
	internal OperationInfo.Subcomponents _operationToOwnerWhenHitInfo;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(2048), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 512로 두는 게 좋음.")]
	[SerializeField]
	private int _maxHits = 512;

	[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
	[SerializeField]
	private bool _optimizedCollider = true;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _operationInfo;

	[SerializeField]
	[BoundsAttackVisualEffect.Subcomponent]
	private BoundsAttackVisualEffect.Subcomponents _effect;

	private PushInfo _pushInfo;

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
		base.Initialize();
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_operationInfo.Initialize();
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		((Behaviour)_collider).enabled = true;
		Bounds bounds = _collider.bounds;
		_overlapper.OverlapCollider(_collider);
		if (_optimizedCollider)
		{
			((Behaviour)_collider).enabled = false;
		}
		bool flag = false;
		for (int i = 0; i < _overlapper.results.Count; i++)
		{
			Target component = ((Component)_overlapper.results[i]).GetComponent<Target>();
			if ((Object)(object)component == (Object)null)
			{
				continue;
			}
			Bounds bounds2 = component.collider.bounds;
			Bounds bounds3 = default(Bounds);
			((Bounds)(ref bounds3)).min = Vector2.op_Implicit(MMMaths.Max(Vector2.op_Implicit(((Bounds)(ref bounds)).min), Vector2.op_Implicit(((Bounds)(ref bounds2)).min)));
			((Bounds)(ref bounds3)).max = Vector2.op_Implicit(MMMaths.Min(Vector2.op_Implicit(((Bounds)(ref bounds)).max), Vector2.op_Implicit(((Bounds)(ref bounds2)).max)));
			Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds3);
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
					flag = true;
					_chronoToTarget.ApplyTo(component.character);
					Damage damage = owner.stat.GetDamage(_attackDamage.amount, hitPoint, _hitInfo);
					if (_hitInfo.attackType != 0)
					{
						CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)component).transform.position), bounds3, force);
					}
					flag = owner.TryAttackCharacter(component, ref damage);
					if (flag)
					{
						((MonoBehaviour)this).StartCoroutine(_operationInfo.CRun(owner, component.character));
						this.onHit?.Invoke(component, ref damage);
						_effect.Spawn(owner, bounds3, in damage, component);
					}
				}
			}
			else if ((Object)(object)component.damageable != (Object)null)
			{
				Damage damage2 = owner.stat.GetDamage(_attackDamage.amount, hitPoint, _hitInfo);
				if (component.damageable.spawnEffectOnHit && _hitInfo.attackType != 0)
				{
					CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)component).transform.position), bounds3, force);
					_effect.Spawn(owner, bounds3, in damage2, component);
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
	}
}
