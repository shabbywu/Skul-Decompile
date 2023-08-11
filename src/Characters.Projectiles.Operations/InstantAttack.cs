using System;
using Characters.Movements;
using Characters.Operations;
using Characters.Operations.Movement;
using FX.BoundsAttackVisualEffect;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class InstantAttack : Operation
{
	private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(2048);

	[SerializeField]
	private int _limit = 15;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(2048), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[Subcomponent(typeof(TargetedOperationInfo))]
	[SerializeField]
	private TargetedOperationInfo.Subcomponents _operationInfo;

	[BoundsAttackVisualEffect.Subcomponent]
	[SerializeField]
	private BoundsAttackVisualEffect.Subcomponents _effect;

	[SerializeField]
	protected HitInfo _hitInfo = new HitInfo(Damage.AttackType.Ranged);

	[SerializeField]
	protected ChronoInfo _chrono;

	private NonAllocOverlapper _overlapper;

	private PushInfo _pushInfo;

	private IAttackDamage _attackDamage;

	private void Awake()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		_limit = Math.Min(_limit, 2048);
		_overlapper = (NonAllocOverlapper)((_limit == _sharedOverlapper.capacity) ? ((object)_sharedOverlapper) : ((object)new NonAllocOverlapper(_limit)));
		Array.Sort(((SubcomponentArray<TargetedOperationInfo>)_operationInfo).components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
		((Behaviour)_collider).enabled = false;
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_operationInfo.Initialize();
		TargetedOperationInfo[] components = ((SubcomponentArray<TargetedOperationInfo>)_operationInfo).components;
		foreach (TargetedOperationInfo targetedOperationInfo in components)
		{
			if (targetedOperationInfo.operation is Characters.Operations.Movement.Knockback knockback)
			{
				_pushInfo = knockback.pushInfo;
				break;
			}
			if (targetedOperationInfo.operation is Characters.Operations.Movement.Smash smash)
			{
				_pushInfo = smash.pushInfo;
			}
		}
	}

	public override void Run(IProjectile projectile)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		Character owner = projectile.owner;
		if ((Object)(object)owner == (Object)null || !((Component)owner).gameObject.activeSelf)
		{
			return;
		}
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		if (_attackDamage == null)
		{
			if ((Object)(object)owner.playerComponents?.inventory.weapon != (Object)null)
			{
				_attackDamage = owner.playerComponents.inventory.weapon.weaponAttackDamage;
			}
			else
			{
				_attackDamage = ((Component)owner).GetComponent<IAttackDamage>();
			}
		}
		((Behaviour)_collider).enabled = true;
		_overlapper.OverlapCollider(_collider);
		Bounds bounds = _collider.bounds;
		((Behaviour)_collider).enabled = false;
		for (int i = 0; i < _overlapper.results.Count; i++)
		{
			Target component = ((Component)_overlapper.results[i]).GetComponent<Target>();
			if ((Object)(object)component == (Object)null)
			{
				continue;
			}
			Bounds bounds2 = component.collider.bounds;
			Bounds val = default(Bounds);
			((Bounds)(ref val)).min = Vector2.op_Implicit(MMMaths.Max(Vector2.op_Implicit(((Bounds)(ref bounds)).min), Vector2.op_Implicit(((Bounds)(ref bounds2)).min)));
			((Bounds)(ref val)).max = Vector2.op_Implicit(MMMaths.Min(Vector2.op_Implicit(((Bounds)(ref bounds)).max), Vector2.op_Implicit(((Bounds)(ref bounds2)).max)));
			Vector2 hitPoint = MMMaths.RandomPointWithinBounds(val);
			if ((Object)(object)projectile.owner == (Object)null)
			{
				continue;
			}
			Vector2 force = Vector2.zero;
			if (_pushInfo != null)
			{
				(Vector2, Vector2) tuple = _pushInfo.EvaluateTimeIndependent(owner, component);
				force = tuple.Item1 + tuple.Item2;
			}
			if ((Object)(object)component.character != (Object)null && component.character.liveAndActive && (Object)(object)component.character != (Object)(object)owner)
			{
				if (!component.character.cinematic.value)
				{
					Damage damage = owner.stat.GetDamage(_attackDamage.amount, hitPoint, _hitInfo);
					_chrono.ApplyTo(component.character);
					if (_hitInfo.attackType != 0)
					{
						CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)component).transform.position), val, force);
					}
					if (owner.TryAttackCharacter(component, ref damage))
					{
						((MonoBehaviour)this).StartCoroutine(_operationInfo.CRun(owner, component.character));
						_effect.Spawn(owner, val, in damage, component);
					}
				}
			}
			else if ((Object)(object)component.damageable != (Object)null)
			{
				Damage damage2 = owner.stat.GetDamage(_attackDamage.amount, hitPoint, _hitInfo);
				if (component.damageable.spawnEffectOnHit && _hitInfo.attackType != 0)
				{
					CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)component).transform.position), val, force);
					_effect.Spawn(owner, val, in damage2, component);
				}
				if (_hitInfo.attackType == Damage.AttackType.None)
				{
					break;
				}
				component.damageable.Hit(owner, ref damage2, force);
			}
		}
	}
}
