using System;
using Characters.Movements;
using Characters.Operations.Movement;
using FX.BoundsAttackVisualEffect;
using GameResources;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public sealed class PlayerAttack : CharacterOperation
{
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
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _ownerWhenHit;

	[Subcomponent(typeof(TargetedOperationInfo))]
	[SerializeField]
	private TargetedOperationInfo.Subcomponents _targetWhenHit;

	[BoundsAttackVisualEffect.Subcomponent]
	[SerializeField]
	private BoundsAttackVisualEffect.Subcomponents _effect;

	private IAttackDamage _attackDamage;

	private PushInfo _pushInfo;

	private void Awake()
	{
		Array.Sort(((SubcomponentArray<TargetedOperationInfo>)_targetWhenHit).components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
	}

	public override void Run(Character owner)
	{
		Attack(owner);
	}

	public override void Initialize()
	{
		base.Initialize();
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_ownerWhenHit.Initialize();
		_targetWhenHit.Initialize();
		TargetedOperationInfo[] components = ((SubcomponentArray<TargetedOperationInfo>)_targetWhenHit).components;
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

	private void Attack(Character owner)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		Target component = ((Component)Singleton<Service>.Instance.levelManager.player.collider).GetComponent<Target>();
		bool flag = false;
		if ((Object)(object)component == (Object)null)
		{
			return;
		}
		Vector2 hitPoint = MMMaths.RandomPointWithinBounds(component.collider.bounds);
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
			if (!component.character.liveAndActive || (Object)(object)component.character == (Object)(object)owner || component.character.cinematic.value)
			{
				return;
			}
			_chronoToTarget.ApplyTo(component.character);
			Damage damage = owner.stat.GetDamage(_attackDamage.amount, hitPoint, _hitInfo);
			if (_hitInfo.attackType != 0)
			{
				CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)component).transform.position), component.collider.bounds, force);
			}
			flag = owner.TryAttackCharacter(component, ref damage);
			if (flag)
			{
				((MonoBehaviour)this).StartCoroutine(_targetWhenHit.CRun(owner, component.character));
				_effect.Spawn(owner, component.collider.bounds, in damage, component);
			}
		}
		if (flag)
		{
			_chronoToGlobe.ApplyGlobe();
			_chronoToOwner.ApplyTo(owner);
			if (((SubcomponentArray<OperationInfo>)_ownerWhenHit).components.Length != 0)
			{
				((MonoBehaviour)this).StartCoroutine(_ownerWhenHit.CRun(owner));
			}
		}
	}

	public override void Stop()
	{
		_ownerWhenHit.StopAll();
	}
}
