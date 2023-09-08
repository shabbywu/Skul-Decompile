using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using Characters.Operations.Attack;
using FX.CastAttackVisualEffect;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt;

public class YggdrasillElderEntCollisionDetector : MonoBehaviour
{
	public delegate void onTerrainHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

	public delegate void onTargetHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);

	[SerializeField]
	protected HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _onCharacterHit;

	[SerializeField]
	private Character _owner;

	[SerializeField]
	private TargetLayer _layer;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	[Range(1f, 15f)]
	private int _maxHits = 1;

	private List<Target> _hits = new List<Target>(15);

	private ContactFilter2D _filter;

	[SerializeField]
	protected ChronoInfo _chronoToGlobe;

	[SerializeField]
	protected ChronoInfo _chronoToOwner;

	[SerializeField]
	protected ChronoInfo _chronoToTarget;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	internal OperationInfo.Subcomponents _operationToOwnerWhenHitInfo;

	[Subcomponent(typeof(CastAttackInfoSequence))]
	[SerializeField]
	private CastAttackInfoSequence.Subcomponents _attackAndEffect;

	[SerializeField]
	[CastAttackVisualEffect.Subcomponent]
	private CastAttackVisualEffect.Subcomponents _effect;

	private CoroutineReference _expireReference;

	private IAttackDamage _attackDamage;

	private int _propHits;

	private static readonly NonAllocCaster _caster;

	private bool _running;

	public event Action<RaycastHit2D> onTerrainHit;

	public event onTargetHitDelegate onHit;

	static YggdrasillElderEntCollisionDetector()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_caster = new NonAllocCaster(15);
	}

	private void Awake()
	{
		onHit += delegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)target.character == (Object)null) && !target.character.cinematic.value)
			{
				Damage damage = _owner.stat.GetDamage(_attackDamage.amount, ((RaycastHit2D)(ref raycastHit)).point, _hitInfo);
				if (_owner.TryAttackCharacter(target, ref damage))
				{
					_chronoToOwner.ApplyTo(_owner);
					_chronoToTarget.ApplyTo(target.character);
					((MonoBehaviour)this).StartCoroutine(_onCharacterHit.CRun(_owner, target.character));
					((MonoBehaviour)this).StartCoroutine(_operationToOwnerWhenHitInfo.CRun(_owner));
				}
			}
		};
	}

	internal void Initialize(GameObject owner, Collider2D collider)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		_filter.layerMask = _layer.Evaluate(owner);
		_propHits = 0;
		_hits.Clear();
		_collider = collider;
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_attackAndEffect.Initialize();
		_onCharacterHit.Initialize();
	}

	private void Detect(Vector2 origin, Vector2 distance)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		Detect(origin, ((Vector2)(ref distance)).normalized, ((Vector2)(ref distance)).magnitude);
	}

	private void Detect(Vector2 origin, Vector2 direction, float distance)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(_filter.layerMask);
		_caster.RayCast(origin, direction, distance);
		if (Object.op_Implicit((Object)(object)_collider))
		{
			_caster.ColliderCast(_collider, direction, distance);
		}
		else
		{
			_caster.RayCast(origin, direction, distance);
		}
		for (int i = 0; i < _caster.results.Count; i++)
		{
			RaycastHit2D val = _caster.results[i];
			Target component = ((Component)((RaycastHit2D)(ref val)).collider).GetComponent<Target>();
			if ((Object)(object)component == (Object)null)
			{
				break;
			}
			if (!_hits.Contains(component))
			{
				if ((Object)(object)component.character != (Object)null)
				{
					if (component.character.liveAndActive)
					{
						this.onHit(_collider, origin, direction, distance, _caster.results[i], component);
						_hits.Add(component);
					}
				}
				else if ((Object)(object)component.damageable != (Object)null)
				{
					Stat stat = _owner.stat;
					double baseDamage = _attackDamage.amount;
					val = _caster.results[i];
					Damage damage = stat.GetDamage(baseDamage, ((RaycastHit2D)(ref val)).point, _hitInfo);
					component.damageable.Hit(_owner, ref damage);
					this.onHit(_collider, origin, direction, distance, _caster.results[i], component);
					_hits.Add(component);
				}
			}
			if (_hits.Count >= _maxHits)
			{
				Stop();
			}
		}
	}

	public void Stop()
	{
		_running = false;
		_attackAndEffect.StopAllOperationsToOwner();
	}

	public IEnumerator CRun(Transform moveTarget)
	{
		Vector2 val = Vector2.op_Implicit(moveTarget.position);
		_running = true;
		while (_running)
		{
			Vector2 nextPosition = Vector2.op_Implicit(moveTarget.position);
			if (_owner.chronometer.animation.deltaTime > float.Epsilon)
			{
				Detect(val, nextPosition - val);
			}
			yield return null;
			val = nextPosition;
		}
	}
}
