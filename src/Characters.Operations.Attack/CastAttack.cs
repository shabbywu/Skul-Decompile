using System;
using System.Collections;
using System.Collections.Generic;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public sealed class CastAttack : CharacterOperation, IAttack
{
	[Serializable]
	private class CollisionDetector
	{
		public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

		public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);

		private static readonly NonAllocCaster _caster = new NonAllocCaster(15);

		private CastAttack _castAttack;

		[SerializeField]
		private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(2048), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

		[SerializeField]
		private Collider2D _collider;

		[Range(1f, 15f)]
		[SerializeField]
		private int _maxHits = 15;

		private List<Target> _hits = new List<Target>(15);

		private int _propPenetratingHits;

		private ContactFilter2D _filter;

		public Collider2D collider => _collider;

		public event onTerrainHitDelegate onTerrainHit;

		public event onTargetHitDelegate onHit;

		internal void Initialize(CastAttack castAttack)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			_castAttack = castAttack;
			_filter.layerMask = _layer.Evaluate(((Component)castAttack.owner).gameObject);
			_hits.Clear();
			_propPenetratingHits = 0;
			if ((Object)(object)_collider != (Object)null)
			{
				((Behaviour)_collider).enabled = false;
			}
		}

		internal void Detect(Vector2 origin, Vector2 direction, float distance)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
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
			Target target;
			for (int i = 0; i < _caster.results.Count; i++)
			{
				RaycastHit2D result = _caster.results[i];
				HandleResult();
				if (_hits.Count - _propPenetratingHits >= _maxHits)
				{
					break;
				}
				void HandleResult()
				{
					//IL_001f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
					//IL_0093: Unknown result type (might be due to invalid IL or missing references)
					//IL_0099: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
					if (((Component)((RaycastHit2D)(ref result)).collider).gameObject.layer == 8)
					{
						this.onTerrainHit(origin, direction, distance, result);
					}
					else
					{
						target = ((Component)((RaycastHit2D)(ref result)).collider).GetComponent<Target>();
						if (!_hits.Contains(target))
						{
							if ((Object)(object)target.character != (Object)null)
							{
								if (target.character.liveAndActive)
								{
									this.onHit(origin, direction, distance, result, target);
									_hits.Add(target);
								}
							}
							else if ((Object)(object)target.damageable != (Object)null)
							{
								this.onHit(origin, direction, distance, result, target);
								if (!target.damageable.blockCast)
								{
									_propPenetratingHits++;
								}
								_hits.Add(target);
							}
						}
					}
				}
			}
		}
	}

	[SerializeField]
	private float _distance;

	[SerializeField]
	private CollisionDetector _collisionDetector;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onTerrainHit;

	[SerializeField]
	[Subcomponent(typeof(CastAttackInfoSequence))]
	private CastAttackInfoSequence.Subcomponents _attackAndEffect;

	private IAttackDamage _attackDamage;

	internal Character owner { get; private set; }

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
			CastAttackInfoSequence[] components = ((SubcomponentArray<CastAttackInfoSequence>)_attackAndEffect).components;
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

	public override void Stop()
	{
		_attackAndEffect.StopAllOperationsToOwner();
	}

	public override void Run(Character owner)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		_collisionDetector.Initialize(this);
		_collisionDetector.Detect(Vector2.op_Implicit(((Component)this).transform.position), (owner.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left, _distance);
	}

	private void Attack(CastAttackInfo attackInfo, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
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
				if (((SubcomponentArray<OperationInfo>)attackInfo.operationsToOwner).components.Length != 0)
				{
					((MonoBehaviour)owner).StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
				}
				Damage damage = owner.stat.GetDamage(_attackDamage.amount, ((RaycastHit2D)(ref raycastHit)).point, attackInfo.hitInfo);
				if (attackInfo.hitInfo.attackType != 0)
				{
					CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
				}
				attackInfo.effect.Spawn(owner, _collisionDetector.collider, origin, direction, distance, raycastHit, damage, target);
				if (owner.TryAttackCharacter(target, ref damage))
				{
					((MonoBehaviour)owner).StartCoroutine(attackInfo.operationsToCharacter.CRun(owner, target.character));
					this.onHit?.Invoke(target, ref damage);
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

	private IEnumerator CAttack(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		int index = 0;
		float time = 0f;
		while ((Object)(object)this != (Object)null && index < ((SubcomponentArray<CastAttackInfoSequence>)_attackAndEffect).components.Length)
		{
			for (; index < ((SubcomponentArray<CastAttackInfoSequence>)_attackAndEffect).components.Length; index++)
			{
				CastAttackInfoSequence castAttackInfoSequence;
				if (!(time >= (castAttackInfoSequence = ((SubcomponentArray<CastAttackInfoSequence>)_attackAndEffect).components[index]).timeToTrigger))
				{
					break;
				}
				Attack(castAttackInfoSequence.attackInfo, origin, direction, distance, raycastHit, target);
			}
			yield return null;
			time += ((ChronometerBase)owner.chronometer.animation).deltaTime;
		}
	}
}
