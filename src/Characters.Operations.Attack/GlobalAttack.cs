using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Movements;
using Characters.Operations.Movement;
using FX.BoundsAttackVisualEffect;
using GameResources;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public sealed class GlobalAttack : CharacterOperation, IAttack
{
	[SerializeField]
	private float _delay;

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
	internal OperationInfo.Subcomponents _operationToOwnerWhenHitInfo;

	[SerializeField]
	[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 512로 두는 게 좋음.")]
	private int _maxHits = 512;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _operationInfo;

	[SerializeField]
	[BoundsAttackVisualEffect.Subcomponent]
	private BoundsAttackVisualEffect.Subcomponents _effect;

	private PushInfo _pushInfo;

	private IAttackDamage _attackDamage;

	public event OnAttackHitDelegate onHit;

	private void Awake()
	{
		Array.Sort(((SubcomponentArray<TargetedOperationInfo>)_operationInfo).components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
	}

	public override void Initialize()
	{
		base.Initialize();
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_operationInfo.Initialize();
		TargetedOperationInfo[] components = ((SubcomponentArray<TargetedOperationInfo>)_operationInfo).components;
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

	public override void Run(Character owner)
	{
		List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
		if (_delay == 0f)
		{
			Attack(owner, allEnemies);
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(CRun(owner, allEnemies));
		}
	}

	private IEnumerator CRun(Character owner, List<Character> enemies)
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.master, _delay);
		Attack(owner, enemies);
	}

	private void Attack(Character owner, List<Character> enemies)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		foreach (Character enemy in enemies)
		{
			Target componentInChildren = ((Component)enemy).GetComponentInChildren<Target>();
			if ((Object)(object)componentInChildren == (Object)null || !((Component)enemy).gameObject.activeInHierarchy)
			{
				continue;
			}
			Bounds bounds = ((Collider2D)enemy.collider).bounds;
			Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds);
			Vector2 force = Vector2.zero;
			if (_pushInfo != null)
			{
				(Vector2, Vector2) tuple = _pushInfo.EvaluateTimeIndependent(owner, componentInChildren);
				force = tuple.Item1 + tuple.Item2;
			}
			if (!enemy.liveAndActive || (Object)(object)enemy == (Object)(object)owner)
			{
				continue;
			}
			flag = true;
			_chronoToTarget.ApplyTo(enemy);
			Damage damage = owner.stat.GetDamage(_attackDamage.amount, hitPoint, _hitInfo);
			if (_hitInfo.attackType != 0)
			{
				CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)componentInChildren).transform.position), bounds, force);
			}
			if (enemy.cinematic.value)
			{
				continue;
			}
			flag = owner.TryAttackCharacter(componentInChildren, ref damage);
			if (flag)
			{
				if (((Component)this).gameObject.activeInHierarchy)
				{
					((MonoBehaviour)this).StartCoroutine(_operationInfo.CRun(owner, enemy));
				}
				this.onHit?.Invoke(componentInChildren, ref damage);
				_effect.Spawn(owner, bounds, in damage, componentInChildren);
			}
			_effect.Spawn(owner, bounds, in damage, componentInChildren);
		}
		if (flag)
		{
			_chronoToGlobe.ApplyGlobe();
			_chronoToOwner.ApplyTo(owner);
			if (((SubcomponentArray<OperationInfo>)_operationToOwnerWhenHitInfo).components.Length != 0)
			{
				((MonoBehaviour)this).StartCoroutine(_operationToOwnerWhenHitInfo.CRun(owner));
			}
		}
	}

	public override void Stop()
	{
		_operationToOwnerWhenHitInfo.StopAll();
	}
}
