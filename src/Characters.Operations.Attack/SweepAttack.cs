using System;
using System.Collections;
using Characters.Movements;
using Characters.Operations.Movement;
using Characters.Utils;
using FX.CastAttackVisualEffect;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public class SweepAttack : CharacterOperation, IAttack
{
	[Serializable]
	public class CollisionDetector
	{
		public delegate void onTerrainHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

		public delegate void onTargetHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);

		public onTerrainHitDelegate onTerrainHit;

		public onTargetHitDelegate onHit;

		private SweepAttack _sweepAttack;

		[SerializeField]
		private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(2048), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

		[SerializeField]
		private bool _selfTarget;

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

		private static readonly NonAllocCaster _caster;

		[NonSerialized]
		public bool group;

		public HitHistoryManager hits
		{
			get
			{
				return _hits;
			}
			set
			{
				_hits = value;
			}
		}

		static CollisionDetector()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			_caster = new NonAllocCaster(99);
		}

		internal void Initialize(SweepAttack sweepAttack)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			_sweepAttack = sweepAttack;
			_filter.layerMask = _layer.Evaluate(((Component)sweepAttack.owner).gameObject);
			if (!group)
			{
				_hits.Clear();
			}
			_propPenetratingHits = 0;
			if (_optimizedCollider && (Object)(object)_collider != (Object)null)
			{
				((Behaviour)_collider).enabled = false;
			}
			if (_maxHitsPerUnit == 0)
			{
				_maxHitsPerUnit = int.MaxValue;
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
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(_filter.layerMask);
			if (Object.op_Implicit((Object)(object)_collider))
			{
				if (_optimizedCollider)
				{
					((Behaviour)_collider).enabled = true;
				}
				_caster.ColliderCast(_collider, direction, distance);
				if (_optimizedCollider)
				{
					((Behaviour)_collider).enabled = false;
				}
			}
			else
			{
				_caster.RayCast(origin, direction, distance);
			}
			for (int i = 0; i < _caster.results.Count; i++)
			{
				RaycastHit2D raycastHit = _caster.results[i];
				if (ExtensionMethods.Contains(_terrainLayer, ((Component)((RaycastHit2D)(ref raycastHit)).collider).gameObject.layer))
				{
					onTerrainHit(_collider, origin, direction, distance, raycastHit);
				}
				else
				{
					Target component = ((Component)((RaycastHit2D)(ref raycastHit)).collider).GetComponent<Target>();
					if ((Object)(object)component == (Object)null || !_hits.CanAttack(component, _maxHits, _maxHitsPerUnit, _hitIntervalPerUnit))
					{
						continue;
					}
					if ((Object)(object)component.character != (Object)null)
					{
						if ((!_selfTarget && (Object)(object)component.character == (Object)(object)_sweepAttack.owner) || !component.character.liveAndActive)
						{
							continue;
						}
						onHit(_collider, origin, direction, distance, raycastHit, component);
						_hits.AddOrUpdate(component);
					}
					else if ((Object)(object)component.damageable != (Object)null)
					{
						onHit(_collider, origin, direction, distance, raycastHit, component);
						if (!component.damageable.blockCast)
						{
							_propPenetratingHits++;
						}
						_hits.AddOrUpdate(component);
					}
				}
				if (_hits.Count - _propPenetratingHits >= _maxHits)
				{
					_sweepAttack.Stop();
				}
			}
		}
	}

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
	private float _duration;

	[SerializeField]
	private bool _timeIndependent;

	[SerializeField]
	private bool _trackMovement = true;

	[SerializeField]
	private CollisionDetector _collisionDetector;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operationToOwnerWhenHitInfo;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onTerrainHit;

	[Subcomponent(typeof(TargetedOperationInfo))]
	[SerializeField]
	private TargetedOperationInfo.Subcomponents _onCharacterHit;

	[CastAttackVisualEffect.Subcomponent]
	[SerializeField]
	private CastAttackVisualEffect.Subcomponents _effect;

	private CoroutineReference _detectReference;

	private PushInfo _pushInfo;

	private IAttackDamage _attackDamage;

	internal Character owner { get; private set; }

	public CollisionDetector collisionDetector => _collisionDetector;

	public HitInfo hitInfo => _hitInfo;

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

	public event OnAttackHitDelegate onHit;

	private void Awake()
	{
		if (_duration == 0f)
		{
			_duration = float.PositiveInfinity;
		}
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_onTerrainHit.Initialize();
		_onCharacterHit.Initialize();
		TargetedOperationInfo[] components = ((SubcomponentArray<TargetedOperationInfo>)_onCharacterHit).components;
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
		_collisionDetector.onTerrainHit = onTerrainHit;
		_collisionDetector.onHit = onTargetHit;
		void onTargetHit(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)owner == (Object)null)
			{
				Debug.LogError((object)"owner is null");
			}
			if (_adaptiveForce)
			{
				_hitInfo.ChangeAdaptiveDamageAttribute(owner);
			}
			Damage damage = owner.stat.GetDamage(_attackDamage.amount, ((RaycastHit2D)(ref raycastHit)).point, _hitInfo);
			Vector2 force = Vector2.zero;
			if (_pushInfo != null)
			{
				(Vector2, Vector2) tuple = _pushInfo.EvaluateTimeIndependent(owner, target);
				force = tuple.Item1 + tuple.Item2;
			}
			if ((Object)(object)target.character != (Object)null)
			{
				if (!target.character.cinematic.value)
				{
					_chronoToGlobe.ApplyGlobe();
					_chronoToOwner.ApplyTo(owner);
					_chronoToTarget.ApplyTo(target.character);
					if (((SubcomponentArray<OperationInfo>)_operationToOwnerWhenHitInfo).components.Length != 0)
					{
						((MonoBehaviour)this).StartCoroutine(_operationToOwnerWhenHitInfo.CRun(owner));
					}
					if (_hitInfo.attackType != 0)
					{
						CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
					}
					if (owner.TryAttackCharacter(target, ref damage))
					{
						((MonoBehaviour)this).StartCoroutine(_onCharacterHit.CRun(owner, target.character));
						this.onHit?.Invoke(target, ref damage);
						_effect.Spawn(owner, collider, origin, direction, distance, raycastHit, damage, target);
					}
				}
			}
			else if ((Object)(object)target.damageable != (Object)null)
			{
				if (target.damageable.spawnEffectOnHit && _hitInfo.attackType != 0)
				{
					CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
					_effect.Spawn(owner, collider, origin, direction, distance, raycastHit, damage, target);
				}
				if (_hitInfo.attackType != 0)
				{
					target.damageable.Hit(owner, ref damage, force);
				}
			}
		}
		void onTerrainHit(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			((MonoBehaviour)this).StartCoroutine(_onTerrainHit.CRun(owner));
			_effect.Spawn(owner, collider, origin, direction, distance, raycastHit);
		}
	}

	public override void Run(Character owner)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		_collisionDetector.Initialize(this);
		((CoroutineReference)(ref _detectReference)).Stop();
		_detectReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)owner, CDetect());
	}

	public override void Stop()
	{
		base.Stop();
		_operationToOwnerWhenHitInfo.StopAll();
		((CoroutineReference)(ref _detectReference)).Stop();
	}

	private IEnumerator CDetect()
	{
		float time = 0f;
		_ = owner.chronometer.master;
		Chronometer animationChronometer = owner.chronometer.animation;
		while (time < _duration)
		{
			if (_timeIndependent || ((ChronometerBase)animationChronometer).timeScale > float.Epsilon)
			{
				Vector2 distance = Vector2.zero;
				if (_trackMovement && (Object)(object)owner.movement != (Object)null)
				{
					distance = owner.movement.moved;
				}
				_collisionDetector.Detect(Vector2.op_Implicit(((Component)this).transform.position), distance);
			}
			yield return null;
			time = ((!_timeIndependent) ? (time + ((ChronometerBase)animationChronometer).deltaTime) : (time + ((ChronometerBase)Chronometer.global).deltaTime));
		}
	}
}
