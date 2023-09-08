using System;
using System.Collections;
using Characters.Utils;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public class SweepAttack2 : CharacterOperation, IAttack
{
	[Serializable]
	public class CollisionDetector
	{
		public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

		public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);

		private static readonly NonAllocCaster _caster = new NonAllocCaster(99);

		private SweepAttack2 _sweepAttack;

		[SerializeField]
		private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(2048), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

		[SerializeField]
		private LayerMask _terrainLayer = Layers.groundMask;

		[SerializeField]
		private Collider2D _collider;

		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		[SerializeField]
		private bool _optimizedCollider = true;

		[SerializeField]
		private int _maxHits = 512;

		[SerializeField]
		private int _maxHitsPerUnit = 1;

		[SerializeField]
		private float _hitIntervalPerUnit = 0.5f;

		private HitHistoryManager _hits = new HitHistoryManager(32);

		private int _propPenetratingHits;

		private ContactFilter2D _filter;

		public Collider2D collider
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

		public event onTerrainHitDelegate onTerrainHit;

		public event onTargetHitDelegate onHit;

		internal void Initialize(SweepAttack2 sweepAttack)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			_sweepAttack = sweepAttack;
			_filter.layerMask = _layer.Evaluate(((Component)sweepAttack.owner).gameObject);
			_hits.Clear();
			_propPenetratingHits = 0;
			if (_optimizedCollider && (Object)(object)_collider != (Object)null)
			{
				((Behaviour)_collider).enabled = false;
			}
		}

		internal void Detect(Vector2 origin, Vector2 distance)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			Detect(origin, ((Vector2)(ref distance)).normalized, ((Vector2)(ref distance)).magnitude);
		}

		internal void Detect(Vector2 origin, Vector2 direction, float distance)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(_filter.layerMask);
			if ((Object)(object)_collider != (Object)null)
			{
				if (_optimizedCollider)
				{
					((Behaviour)_collider).enabled = true;
					_caster.ColliderCast(_collider, direction, distance);
					((Behaviour)_collider).enabled = false;
				}
				else
				{
					_caster.ColliderCast(_collider, direction, distance);
				}
			}
			else
			{
				_caster.RayCast(origin, direction, distance);
			}
			Target target;
			for (int i = 0; i < _caster.results.Count; i++)
			{
				RaycastHit2D result = _caster.results[i];
				HandleResult();
				if (_hits.Count - _propPenetratingHits >= _maxHits)
				{
					_sweepAttack.Stop();
				}
				void HandleResult()
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					//IL_002f: Unknown result type (might be due to invalid IL or missing references)
					//IL_003b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0124: Unknown result type (might be due to invalid IL or missing references)
					//IL_012a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0136: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
					//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
					if (_terrainLayer.Contains(((Component)((RaycastHit2D)(ref result)).collider).gameObject.layer))
					{
						this.onTerrainHit(origin, direction, distance, result);
					}
					else
					{
						target = ((Component)((RaycastHit2D)(ref result)).collider).GetComponent<Target>();
						if (!((Object)(object)target == (Object)null) && _hits.CanAttack(target, _maxHits, _maxHitsPerUnit, _hitIntervalPerUnit))
						{
							if ((Object)(object)target.character != (Object)null)
							{
								if (target.character.liveAndActive && !target.character.cinematic.value)
								{
									this.onHit(origin, direction, distance, result, target);
									_hits.AddOrUpdate(target);
								}
							}
							else if ((Object)(object)target.damageable != (Object)null)
							{
								this.onHit(origin, direction, distance, result, target);
								if (!target.damageable.blockCast)
								{
									_propPenetratingHits++;
								}
								_hits.AddOrUpdate(target);
							}
						}
					}
				}
			}
		}
	}

	[SerializeField]
	private bool _adaptiveForce;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private bool _timeIndependent;

	[SerializeField]
	private bool _trackMovement = true;

	[SerializeField]
	private CollisionDetector _collisionDetector;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onTerrainHit;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(CastAttackInfoSequence))]
	protected CastAttackInfoSequence.Subcomponents _attackAndEffect;

	private IAttackDamage _attackDamage;

	private CoroutineReference _detectReference;

	internal Character owner { get; private set; }

	private CollisionDetector collisionDetector => _collisionDetector;

	public float duration
	{
		get
		{
			return _duration;
		}
		set
		{
			_duration = value;
		}
	}

	public Collider2D range
	{
		get
		{
			return collisionDetector.collider;
		}
		set
		{
			collisionDetector.collider = value;
		}
	}

	public event OnAttackHitDelegate onHit;

	private void Awake()
	{
		_collisionDetector.onTerrainHit += onTerrainHit;
		if (_attackAndEffect.noDelay)
		{
			_collisionDetector.onHit += onTargetHitWithoutDelay;
		}
		else
		{
			_collisionDetector.onHit += onTargetHit;
		}
		void onTargetHit(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((MonoBehaviour)target).StartCoroutine(CAttack(origin, direction, distance, raycastHit, target));
		}
		void onTargetHitWithoutDelay(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			CastAttackInfoSequence[] components = _attackAndEffect.components;
			foreach (CastAttackInfoSequence castAttackInfoSequence in components)
			{
				Attack(castAttackInfoSequence.attackInfo, origin, direction, distance, raycastHit, target);
			}
		}
		void onTerrainHit(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			((MonoBehaviour)this).StartCoroutine(_onTerrainHit.CRun(owner));
		}
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_attackAndEffect.Initialize();
	}

	public override void Run(Character owner)
	{
		this.owner = owner;
		_collisionDetector.Initialize(this);
		_detectReference.Stop();
		_detectReference = ((MonoBehaviour)(object)owner).StartCoroutineWithReference(CDetect());
	}

	public override void Stop()
	{
		base.Stop();
		_attackAndEffect.StopAllOperationsToOwner();
		_detectReference.Stop();
	}

	private IEnumerator CDetect()
	{
		float time = 0f;
		_ = owner.chronometer.master;
		Chronometer animationChronometer = owner.chronometer.animation;
		while (time < _duration)
		{
			if (_timeIndependent || animationChronometer.timeScale > float.Epsilon)
			{
				Vector2 distance = Vector2.zero;
				if (_trackMovement && (Object)(object)owner.movement != (Object)null)
				{
					distance = owner.movement.moved;
				}
				_collisionDetector.Detect(Vector2.op_Implicit(((Component)this).transform.position), distance);
			}
			yield return null;
			time = ((!_timeIndependent) ? (time + animationChronometer.deltaTime) : (time + Chronometer.global.deltaTime));
		}
	}

	protected void Attack(CastAttackInfo attackInfo, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		Vector2 force = Vector2.zero;
		if (attackInfo.pushInfo != null)
		{
			(Vector2, Vector2) tuple = attackInfo.pushInfo.EvaluateTimeIndependent(owner, target);
			force = tuple.Item1 + tuple.Item2;
		}
		if ((Object)(object)target.character != (Object)null)
		{
			if (target.character.liveAndActive && !((Object)(object)target.character == (Object)(object)owner) && !target.character.cinematic.value)
			{
				attackInfo.ApplyChrono(owner, target.character);
				if (attackInfo.operationsToOwner.components.Length != 0)
				{
					((MonoBehaviour)owner).StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
				}
				if (_adaptiveForce)
				{
					attackInfo.hitInfo.ChangeAdaptiveDamageAttribute(owner);
				}
				Damage damage = owner.stat.GetDamage(_attackDamage.amount, ((RaycastHit2D)(ref raycastHit)).point, attackInfo.hitInfo);
				if (attackInfo.hitInfo.attackType != 0)
				{
					CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
				}
				if (owner.TryAttackCharacter(target, ref damage))
				{
					((MonoBehaviour)owner).StartCoroutine(attackInfo.operationsToCharacter.CRun(owner, target.character));
					attackInfo.effect.Spawn(owner, _collisionDetector.collider, origin, direction, distance, raycastHit, damage, target);
				}
			}
		}
		else if ((Object)(object)target.damageable != (Object)null)
		{
			attackInfo.ApplyChrono(owner);
			((MonoBehaviour)owner).StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
			Damage damage2 = owner.stat.GetDamage(_attackDamage.amount, ((RaycastHit2D)(ref raycastHit)).point, attackInfo.hitInfo);
			if (target.damageable.spawnEffectOnHit && attackInfo.hitInfo.attackType != 0)
			{
				CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
				attackInfo.effect.Spawn(owner, _collisionDetector.collider, origin, direction, distance, raycastHit, damage2, target);
			}
			if (attackInfo.hitInfo.attackType != 0)
			{
				target.damageable.Hit(owner, ref damage2, force);
			}
		}
	}

	protected virtual IEnumerator CAttack(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		int index = 0;
		float time = 0f;
		Vector3 originOffset = MMMaths.Vector2ToVector3(origin) - ((Component)target).transform.position;
		Vector3 hitPointOffset = MMMaths.Vector2ToVector3(((RaycastHit2D)(ref raycastHit)).point) - ((Component)target).transform.position;
		while ((Object)(object)this != (Object)null && index < _attackAndEffect.components.Length)
		{
			for (; index < _attackAndEffect.components.Length; index++)
			{
				CastAttackInfoSequence castAttackInfoSequence;
				if (!(time >= (castAttackInfoSequence = _attackAndEffect.components[index]).timeToTrigger))
				{
					break;
				}
				((RaycastHit2D)(ref raycastHit)).point = Vector2.op_Implicit(((Component)target).transform.position + hitPointOffset);
				Attack(castAttackInfoSequence.attackInfo, Vector2.op_Implicit(((Component)target).transform.position + originOffset), direction, distance, raycastHit, target);
			}
			yield return null;
			time += owner.chronometer.animation.deltaTime;
		}
	}
}
