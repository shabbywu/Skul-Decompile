using System;
using System.Collections;
using Characters.Projectiles.Movements;
using Characters.Projectiles.Operations;
using Characters.Utils;
using FX.ProjectileAttackVisualEffect;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Projectiles.Customs;

public class TerrainCollisionDetector : MonoBehaviour
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

		public event onTerrainHitDelegate onTerrainHit;

		static CollisionDetector()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			_caster = new NonAllocCaster(15);
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

		internal void SetHitHistoryManager(HitHistoryManager hitHistory)
		{
			hitHistoryManager = hitHistory;
		}

		internal void Detect(Vector2 origin, Vector2 direction, float distance)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(_terrainLayer);
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
					continue;
				}
				Target component = ((Component)((RaycastHit2D)(ref raycastHit)).collider).GetComponent<Target>();
				if ((Object)(object)component == (Object)null)
				{
					Debug.LogError((object)("Need a target component to: " + ((Object)((RaycastHit2D)(ref raycastHit)).collider).name + "!"));
				}
				else if (hitHistoryManager.CanAttack(component, _maxHits + _propPenetratingHits, _maxHitsPerUnit, _hitIntervalPerUnit) && hitHistoryManager.Count - _propPenetratingHits >= _maxHits)
				{
					_projectile.Despawn();
					break;
				}
			}
		}
	}

	[SerializeField]
	private Projectile _projectile;

	[SerializeField]
	private float _maxLifeTime;

	[SerializeField]
	private float _collisionTime;

	[SerializeField]
	private CollisionDetector _collisionDetector;

	[Operation.Subcomponent]
	[SerializeField]
	private Operation.Subcomponents _onDespawn;

	[HitOperation.Subcomponent]
	[SerializeField]
	private HitOperation.Subcomponents _onTerrainHit;

	[SerializeField]
	[ProjectileAttackVisualEffect.Subcomponent]
	private ProjectileAttackVisualEffect.Subcomponents _effect;

	[SerializeField]
	private Movement _movement;

	private float _direction;

	private float _time;

	public Vector2 direction { get; private set; }

	public float speed { get; private set; }

	private void Awake()
	{
		_collisionDetector.onTerrainHit += onTerrainHit;
		void onTerrainHit(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < ((SubcomponentArray<HitOperation>)_onTerrainHit).components.Length; i++)
			{
				((SubcomponentArray<HitOperation>)_onTerrainHit).components[i].Run(_projectile, raycastHit);
			}
			_effect.Spawn(_projectile, origin, direction, distance, raycastHit);
		}
	}

	public void Run()
	{
		((MonoBehaviour)this).StartCoroutine(CUpdate());
	}

	private IEnumerator CUpdate()
	{
		_time = 0f;
		while (_time <= _maxLifeTime)
		{
			yield return null;
			float deltaTime = ((ChronometerBase)Chronometer.global).deltaTime;
			_time += deltaTime;
			(Vector2, float) tuple = _movement.GetSpeed(_time, deltaTime);
			(direction, speed) = tuple;
			speed *= deltaTime;
			if (_time >= _collisionTime)
			{
				_collisionDetector.Detect(Vector2.op_Implicit(((Component)this).transform.position), direction, speed);
			}
		}
		_effect.SpawnExpire(_projectile);
		Despawn();
	}

	internal void Despawn()
	{
		for (int i = 0; i < ((SubcomponentArray<Operation>)_onDespawn).components.Length; i++)
		{
			((SubcomponentArray<Operation>)_onDespawn).components[i].Run(_projectile);
		}
		_effect.SpawnDespawn(_projectile);
	}
}
