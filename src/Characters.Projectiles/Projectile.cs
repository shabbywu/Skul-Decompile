using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Characters.Operations;
using Characters.Projectiles.Movements;
using Characters.Projectiles.Operations;
using Characters.Utils;
using FX.ProjectileAttackVisualEffect;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles;

[RequireComponent(typeof(PoolObject))]
public class Projectile : MonoBehaviour, IProjectile, IMonoBehaviour
{
	[Serializable]
	private class CollisionDetector
	{
		public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

		public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);

		private const int maxHits = 99;

		private Projectile _projectile;

		[SerializeField]
		private TargetLayer _layer;

		[SerializeField]
		private LayerMask _terrainLayer = Layers.terrainMaskForProjectile;

		[SerializeField]
		private Collider2D _collider;

		[Range(1f, 99f)]
		[SerializeField]
		private int _maxHits = 1;

		[SerializeField]
		private int _maxHitsPerUnit = 1;

		[SerializeField]
		private float _hitIntervalPerUnit = 0.5f;

		internal HitHistoryManager hitHistoryManager;

		private HitHistoryManager _internalHitHistory = new HitHistoryManager(99);

		private int _propPenetratingHits;

		private static readonly NonAllocCaster _caster;

		public Collider2D collider => _collider;

		public event onTerrainHitDelegate onTerrainHit;

		public event onTargetHitDelegate onHit;

		static CollisionDetector()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			_caster = new NonAllocCaster(64);
		}

		internal void Initialize(Projectile projectile)
		{
			Initialize(projectile, _internalHitHistory);
		}

		internal void Initialize(Projectile projectile, HitHistoryManager hitHistory)
		{
			_projectile = projectile;
			hitHistoryManager = hitHistory;
			hitHistoryManager.Clear();
			_propPenetratingHits = 0;
			if ((Object)(object)_collider != (Object)null)
			{
				((Behaviour)_collider).enabled = false;
			}
		}

		internal void Dispose()
		{
			_projectile = null;
			_collider = null;
		}

		internal void SetHitHistoryManager(HitHistoryManager hitHistory)
		{
			hitHistoryManager = hitHistory;
		}

		internal void Detect(Vector2 origin, Vector2 direction, float distance)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)_projectile.owner == (Object)null)
			{
				return;
			}
			((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(LayerMask.op_Implicit(LayerMask.op_Implicit(_layer.Evaluate(((Component)_projectile.owner).gameObject)) | LayerMask.op_Implicit(_terrainLayer)));
			if ((Object)(object)_collider != (Object)null)
			{
				((Behaviour)_collider).enabled = true;
				_caster.ColliderCast(_collider, direction, distance);
				((Behaviour)_collider).enabled = false;
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
					this.onTerrainHit(origin, direction, distance, raycastHit);
					break;
				}
				Target component = ((Component)((RaycastHit2D)(ref raycastHit)).collider).GetComponent<Target>();
				if ((Object)(object)component == (Object)null)
				{
					Debug.LogError((object)("Need a target component to: " + ((Object)((RaycastHit2D)(ref raycastHit)).collider).name + "!"));
				}
				else
				{
					if (!hitHistoryManager.CanAttack(component, _maxHits + _propPenetratingHits, _maxHitsPerUnit, _hitIntervalPerUnit))
					{
						continue;
					}
					if ((Object)(object)component.character != (Object)null)
					{
						if ((Object)(object)component.character == (Object)(object)_projectile.owner || !component.character.liveAndActive)
						{
							continue;
						}
						this.onHit(origin, direction, distance, raycastHit, component);
						hitHistoryManager.AddOrUpdate(component);
					}
					else if ((Object)(object)component.damageable != (Object)null)
					{
						this.onHit(origin, direction, distance, raycastHit, component);
						if (!component.damageable.blockCast)
						{
							_propPenetratingHits++;
						}
						hitHistoryManager.AddOrUpdate(component);
					}
					if (hitHistoryManager.Count - _propPenetratingHits >= _maxHits)
					{
						_projectile.Despawn();
						break;
					}
				}
			}
		}
	}

	[GetComponent]
	[SerializeField]
	private PoolObject _reusable;

	[SerializeField]
	private float _maxLifeTime;

	[SerializeField]
	private float _collisionTime;

	[SerializeField]
	private Transform _rotatable;

	[SerializeField]
	private bool _disableCollisionDetect;

	[SerializeField]
	private CollisionDetector _collisionDetector;

	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Projectile);

	[Subcomponent(typeof(Characters.Projectiles.Operations.OperationInfo))]
	[Space]
	[SerializeField]
	private Characters.Projectiles.Operations.OperationInfo.Subcomponents _operations;

	[Characters.Projectiles.Operations.Operation.Subcomponent]
	[SerializeField]
	[Space]
	private Characters.Projectiles.Operations.Operation.Subcomponents _onSpawned;

	[SerializeField]
	[Characters.Projectiles.Operations.Operation.Subcomponent]
	private Characters.Projectiles.Operations.Operation.Subcomponents _onDespawn;

	[SerializeField]
	private bool _despawnOnTerrainHit = true;

	[HitOperation.Subcomponent]
	[SerializeField]
	private HitOperation.Subcomponents _onTerrainHit;

	[SerializeField]
	[Characters.Projectiles.Operations.CharacterHitOperation.Subcomponent]
	private Characters.Projectiles.Operations.CharacterHitOperation.Subcomponents _onCharacterHit;

	[SerializeField]
	[ProjectileAttackVisualEffect.Subcomponent]
	private ProjectileAttackVisualEffect.Subcomponents _effect;

	[SerializeField]
	[Movement.Subcomponent]
	private Movement _movement;

	private float _direction;

	private float _time;

	public Character owner { get; private set; }

	public PoolObject reusable => _reusable;

	public float maxLifeTime
	{
		get
		{
			return _maxLifeTime;
		}
		set
		{
			_maxLifeTime = value;
		}
	}

	public Movement movement => _movement;

	public Collider2D collider => _collisionDetector.collider;

	public float baseDamage { get; private set; }

	public float speedMultiplier { get; private set; }

	public Vector2 firedDirection { get; private set; }

	public Vector2 direction
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _movement.directionVector;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_movement.directionVector = value;
		}
	}

	public float speed { get; private set; }

	private void Awake()
	{
		if (_operations == null)
		{
			_operations = new Characters.Projectiles.Operations.OperationInfo.Subcomponents();
		}
		else
		{
			_operations.Sort();
		}
		_collisionDetector.onTerrainHit += onTerrainHit;
		_collisionDetector.onHit += onTargetHit;
		void onTargetHit(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			Damage damage = owner.stat.GetProjectileDamage(this, baseDamage, ((RaycastHit2D)(ref raycastHit)).point, _hitInfo);
			Vector2 force = direction * speed * 10f;
			if ((Object)(object)target.character != (Object)null)
			{
				if (target.character.liveAndActive && !target.character.cinematic.value && owner.TryAttackCharacter(target, ref damage))
				{
					for (int i = 0; i < ((SubcomponentArray<Characters.Projectiles.Operations.CharacterHitOperation>)_onCharacterHit).components.Length; i++)
					{
						((SubcomponentArray<Characters.Projectiles.Operations.CharacterHitOperation>)_onCharacterHit).components[i].Run(this, raycastHit, target.character);
					}
					if (_hitInfo.attackType != 0)
					{
						CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
					}
					_effect.Spawn(this, origin, direction, distance, raycastHit, damage, target);
				}
			}
			else if ((Object)(object)target.damageable != (Object)null)
			{
				target.damageable.Hit(owner, ref damage, force);
				if (target.damageable.spawnEffectOnHit && _hitInfo.attackType != 0)
				{
					CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
					_effect.Spawn(this, origin, direction, distance, raycastHit, damage, target);
				}
			}
		}
		void onTerrainHit(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			for (int j = 0; j < ((SubcomponentArray<HitOperation>)_onTerrainHit).components.Length; j++)
			{
				((SubcomponentArray<HitOperation>)_onTerrainHit).components[j].Run(this, raycastHit);
			}
			_effect.Spawn(this, origin, direction, distance, raycastHit);
			if (_despawnOnTerrainHit)
			{
				Despawn();
			}
		}
	}

	private void OnDestroy()
	{
		owner = null;
		_reusable = null;
		_movement = null;
		_collisionDetector.Dispose();
		_collisionDetector = null;
		_operations = null;
		_onSpawned = null;
		_onDespawn = null;
		_onTerrainHit = null;
		_onCharacterHit = null;
		_effect = null;
	}

	private IEnumerator CUpdate(float delay)
	{
		while (delay > 0f)
		{
			delay -= (((Object)(object)owner != (Object)null) ? ((ChronometerBase)owner.chronometer.projectile).deltaTime : ((ChronometerBase)Chronometer.global).deltaTime);
			yield return null;
		}
		_time = 0f;
		while (_time <= _maxLifeTime)
		{
			float num = (((Object)(object)owner != (Object)null) ? ((ChronometerBase)owner.chronometer.projectile).deltaTime : ((ChronometerBase)Chronometer.global).deltaTime);
			_time += num;
			(Vector2, float) tuple = _movement.GetSpeed(_time, num);
			(firedDirection, speed) = tuple;
			speed *= num;
			if ((Object)(object)_rotatable != (Object)null)
			{
				((Component)_rotatable).transform.rotation = Quaternion.FromToRotation(Vector3.right, Vector2.op_Implicit(firedDirection));
			}
			if (_time >= _collisionTime && !_disableCollisionDetect)
			{
				_collisionDetector.Detect(Vector2.op_Implicit(((Component)this).transform.position), firedDirection, speed);
			}
			((Component)this).transform.Translate(Vector2.op_Implicit(firedDirection * speed), (Space)0);
			yield return null;
		}
		_effect.SpawnExpire(this);
		Despawn();
	}

	public void Despawn()
	{
		for (int i = 0; i < ((SubcomponentArray<Characters.Projectiles.Operations.Operation>)_onDespawn).components.Length; i++)
		{
			((SubcomponentArray<Characters.Projectiles.Operations.Operation>)_onDespawn).components[i].Run(this);
		}
		_effect.SpawnDespawn(this);
		_reusable.Despawn();
	}

	public void Fire(Character owner, float attackDamage, float direction, bool flipX = false, bool flipY = false, float speedMultiplier = 1f, HitHistoryManager hitHistoryManager = null, float delay = 0f)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		_direction = direction;
		Vector3 localScale = ((Component)this).transform.localScale;
		if (flipX)
		{
			localScale.x *= -1f;
		}
		if (flipY)
		{
			localScale.y *= -1f;
		}
		((Component)this).transform.localScale = localScale;
		baseDamage = attackDamage;
		this.speedMultiplier = speedMultiplier;
		((Component)this).gameObject.layer = (TargetLayer.IsPlayer(((Component)owner).gameObject.layer) ? 15 : 16);
		if ((Object)(object)_rotatable != (Object)null)
		{
			((Component)_rotatable).transform.rotation = Quaternion.Euler(0f, 0f, _direction);
		}
		_movement.Initialize(this, _direction);
		_collisionDetector.Initialize(this);
		if (hitHistoryManager != null)
		{
			SetHitHistroyManager(hitHistoryManager);
		}
		for (int i = 0; i < ((SubcomponentArray<Characters.Projectiles.Operations.Operation>)_onSpawned).components.Length; i++)
		{
			((SubcomponentArray<Characters.Projectiles.Operations.Operation>)_onSpawned).components[i].Run(this);
		}
		((MonoBehaviour)this).StartCoroutine(_operations.CRun(this));
		((MonoBehaviour)this).StartCoroutine(CUpdate(delay));
	}

	public void ClearHitHistroy()
	{
		_collisionDetector.hitHistoryManager.Clear();
	}

	public void SetHitHistroyManager(HitHistoryManager hitHistoryManager)
	{
		_collisionDetector.hitHistoryManager = hitHistoryManager;
	}

	public void DetectCollision(Vector2 origin, Vector2 direction, float speed)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		_collisionDetector.Detect(Vector2.op_Implicit(((Component)this).transform.position), direction, speed);
	}

	public override string ToString()
	{
		return ((Object)this).name;
	}

	[SpecialName]
	string IMonoBehaviour.get_name()
	{
		return ((Object)this).name;
	}

	[SpecialName]
	GameObject IMonoBehaviour.get_gameObject()
	{
		return ((Component)this).gameObject;
	}

	[SpecialName]
	Transform IMonoBehaviour.get_transform()
	{
		return ((Component)this).transform;
	}

	T IMonoBehaviour.GetComponent<T>()
	{
		return ((Component)this).GetComponent<T>();
	}

	T IMonoBehaviour.GetComponentInChildren<T>(bool includeInactive)
	{
		return ((Component)this).GetComponentInChildren<T>(includeInactive);
	}

	T IMonoBehaviour.GetComponentInChildren<T>()
	{
		return ((Component)this).GetComponentInChildren<T>();
	}

	T IMonoBehaviour.GetComponentInParent<T>()
	{
		return ((Component)this).GetComponentInParent<T>();
	}

	T[] IMonoBehaviour.GetComponents<T>()
	{
		return ((Component)this).GetComponents<T>();
	}

	void IMonoBehaviour.GetComponents<T>(List<T> results)
	{
		((Component)this).GetComponents<T>(results);
	}

	void IMonoBehaviour.GetComponentsInChildren<T>(List<T> results)
	{
		((Component)this).GetComponentsInChildren<T>(results);
	}

	T[] IMonoBehaviour.GetComponentsInChildren<T>(bool includeInactive)
	{
		return ((Component)this).GetComponentsInChildren<T>(includeInactive);
	}

	void IMonoBehaviour.GetComponentsInChildren<T>(bool includeInactive, List<T> result)
	{
		((Component)this).GetComponentsInChildren<T>(includeInactive, result);
	}

	T[] IMonoBehaviour.GetComponentsInChildren<T>()
	{
		return ((Component)this).GetComponentsInChildren<T>();
	}

	T[] IMonoBehaviour.GetComponentsInParent<T>()
	{
		return ((Component)this).GetComponentsInParent<T>();
	}

	T[] IMonoBehaviour.GetComponentsInParent<T>(bool includeInactive)
	{
		return ((Component)this).GetComponentsInParent<T>(includeInactive);
	}

	void IMonoBehaviour.GetComponentsInParent<T>(bool includeInactive, List<T> results)
	{
		((Component)this).GetComponentsInParent<T>(includeInactive, results);
	}

	[SpecialName]
	bool IMonoBehaviour.get_enabled()
	{
		return ((Behaviour)this).enabled;
	}

	[SpecialName]
	void IMonoBehaviour.set_enabled(bool value)
	{
		((Behaviour)this).enabled = value;
	}

	[SpecialName]
	bool IMonoBehaviour.get_isActiveAndEnabled()
	{
		return ((Behaviour)this).isActiveAndEnabled;
	}

	Coroutine IMonoBehaviour.StartCoroutine(IEnumerator routine)
	{
		return ((MonoBehaviour)this).StartCoroutine(routine);
	}

	void IMonoBehaviour.StopAllCoroutines()
	{
		((MonoBehaviour)this).StopAllCoroutines();
	}

	void IMonoBehaviour.StopCoroutine(IEnumerator routine)
	{
		((MonoBehaviour)this).StopCoroutine(routine);
	}

	void IMonoBehaviour.StopCoroutine(Coroutine routine)
	{
		((MonoBehaviour)this).StopCoroutine(routine);
	}
}
