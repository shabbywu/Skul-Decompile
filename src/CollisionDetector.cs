using System;
using Characters;
using Characters.Utils;
using PhysicsUtils;
using UnityEngine;

[Serializable]
public class CollisionDetector
{
	public delegate void onTerrainHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

	public delegate void onTargetHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);

	private const int maxHits = 99;

	private GameObject _owner;

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(2048), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	private LayerMask _terrainLayer = Layers.groundMask;

	[SerializeField]
	private Collider2D _collider;

	[Range(1f, 99f)]
	[SerializeField]
	private int _maxHits = 15;

	[SerializeField]
	private int _maxHitsPerUnit = 1;

	[SerializeField]
	private float _hitIntervalPerUnit = 0.5f;

	private HitHistoryManager _hits = new HitHistoryManager(99);

	private int _propPenetratingHits;

	private ContactFilter2D _filter;

	private static readonly NonAllocCaster _caster;

	public LayerMask layerMask
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _filter.layerMask;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_filter.layerMask = value;
		}
	}

	public event onTerrainHitDelegate onTerrainHit;

	public event onTargetHitDelegate onHit;

	public event Action onStop;

	static CollisionDetector()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_caster = new NonAllocCaster(99);
	}

	internal void Initialize()
	{
		_hits.Clear();
		_propPenetratingHits = 0;
		if ((Object)(object)_collider != (Object)null)
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
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(_filter.layerMask);
		_caster.RayCast(origin, direction, distance);
		if (Object.op_Implicit((Object)(object)_collider))
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
			LayerMask terrainLayer = _terrainLayer;
			RaycastHit2D val = _caster.results[i];
			if (terrainLayer.Contains(((Component)((RaycastHit2D)(ref val)).collider).gameObject.layer))
			{
				this.onTerrainHit(_collider, origin, direction, distance, _caster.results[i]);
			}
			else
			{
				val = _caster.results[i];
				Target component = ((Component)((RaycastHit2D)(ref val)).collider).GetComponent<Target>();
				if (!_hits.CanAttack(component, _maxHits, _maxHitsPerUnit, _hitIntervalPerUnit))
				{
					continue;
				}
				if ((Object)(object)component.character != (Object)null)
				{
					if (!component.character.liveAndActive)
					{
						continue;
					}
					this.onHit(_collider, origin, direction, distance, _caster.results[i], component);
					_hits.AddOrUpdate(component);
				}
				else if ((Object)(object)component.damageable != (Object)null)
				{
					this.onHit(_collider, origin, direction, distance, _caster.results[i], component);
					if (!component.damageable.blockCast)
					{
						_propPenetratingHits++;
					}
					_hits.AddOrUpdate(component);
				}
			}
			if (_hits.Count - _propPenetratingHits >= _maxHits)
			{
				this.onStop?.Invoke();
			}
		}
	}
}
