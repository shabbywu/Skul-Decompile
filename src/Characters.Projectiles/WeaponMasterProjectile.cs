using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Characters.Operations;
using Characters.Projectiles.Movements;
using Characters.Projectiles.Operations;
using Characters.Utils;
using FX.ProjectileAttackVisualEffect;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Projectiles;

[RequireComponent(typeof(PoolObject))]
public class WeaponMasterProjectile : MonoBehaviour, IProjectile, IMonoBehaviour
{
	[Serializable]
	private class CollisionDetector
	{
		public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

		public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);

		private const int maxHits = 99;

		private WeaponMasterProjectile _projectile;

		[SerializeField]
		private TargetLayer _layer;

		[SerializeField]
		private Collider2D _collider;

		[SerializeField]
		[Range(1f, 99f)]
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

		public event onTargetHitDelegate onHit;

		static CollisionDetector()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			_caster = new NonAllocCaster(64);
		}

		internal void Initialize(WeaponMasterProjectile projectile)
		{
			Initialize(projectile, _internalHitHistory);
		}

		internal void Initialize(WeaponMasterProjectile projectile, HitHistoryManager hitHistory)
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

		internal void SetHitHistoryManager(HitHistoryManager hitHistory)
		{
			hitHistoryManager = hitHistory;
		}

		internal void Detect(Vector2 origin, Vector2 direction, float distance)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)_projectile.owner == (Object)null)
			{
				return;
			}
			((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)_projectile.owner).gameObject));
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

	[SerializeField]
	[GetComponent]
	private PoolObject _reusable;

	[SerializeField]
	private float _maxLifeTime;

	[SerializeField]
	private float _collisionTime;

	[SerializeField]
	private CollisionDetector _collisionDetector;

	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Projectile);

	[Characters.Projectiles.Operations.CharacterHitOperation.Subcomponent]
	[SerializeField]
	[Space]
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
		_collisionDetector.onHit += onTargetHit;
		void onTargetHit(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			Damage damage = owner.stat.GetDamage(baseDamage, ((RaycastHit2D)(ref raycastHit)).point, _hitInfo);
			_ = direction * speed * 10f;
			if (!((Object)(object)target.character == (Object)null) && target.character.liveAndActive && !target.character.invulnerable.value)
			{
				owner.Attack(target, ref damage);
				for (int i = 0; i < ((SubcomponentArray<Characters.Projectiles.Operations.CharacterHitOperation>)_onCharacterHit).components.Length; i++)
				{
					((SubcomponentArray<Characters.Projectiles.Operations.CharacterHitOperation>)_onCharacterHit).components[i].Run(this, raycastHit, target.character);
				}
				_effect.Spawn(this, origin, direction, distance, raycastHit, damage, target);
			}
		}
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
			if (_time >= _collisionTime)
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
		_effect.SpawnDespawn(this);
		_reusable.Despawn();
	}

	public void Fire(Character owner, float attackDamage, float direction, bool flipX = false, bool flipY = false, float speedMultiplier = 1f, HitHistoryManager hitHistoryManager = null, float delay = 0f)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
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
		_movement.Initialize(this, _direction);
		_collisionDetector.Initialize(this);
		if (hitHistoryManager != null)
		{
			SetHitHistroyManager(hitHistoryManager);
		}
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
