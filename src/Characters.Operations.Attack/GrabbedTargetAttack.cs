using System.Collections;
using System.Collections.Generic;
using GameResources;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Operations.Attack;

public sealed class GrabbedTargetAttack : CharacterOperation, IAttack
{
	[UnityEditor.Subcomponent(typeof(BoundsAttackInfoSequence))]
	[SerializeField]
	private BoundsAttackInfoSequence.Subcomponents _attackAndEffect;

	[SerializeField]
	private GrabBoard _grabBoard;

	[SerializeField]
	private bool _grabbedTarget = true;

	[SerializeField]
	private bool _failedTarget;

	private IAttackDamage _attackDamage;

	private List<Target> results;

	public event OnAttackHitDelegate onHit;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_attackAndEffect.Initialize();
		results = new List<Target>(64);
	}

	public override void Stop()
	{
		_attackAndEffect.StopAllOperationsToOwner();
	}

	public override void Run(Character owner)
	{
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_grabBoard == (Object)null)
		{
			return;
		}
		results.Clear();
		if (_grabbedTarget)
		{
			results.AddRange(_grabBoard.targets);
		}
		if (_failedTarget)
		{
			results.AddRange(_grabBoard.failTargets);
		}
		for (int i = 0; i < results.Count; i++)
		{
			Target target = results[i];
			if ((Object)(object)target == (Object)null)
			{
				Debug.LogError((object)"Target is null in GrabbedTargetAttack");
				break;
			}
			if (_attackAndEffect.noDelay)
			{
				BoundsAttackInfoSequence[] components = _attackAndEffect.components;
				foreach (BoundsAttackInfoSequence boundsAttackInfoSequence in components)
				{
					Attack(owner, target.collider.bounds, target, boundsAttackInfoSequence.attackInfo);
				}
			}
			else
			{
				((MonoBehaviour)target).StartCoroutine(CAttack(owner, ((Collider2D)target.character.collider).bounds, target));
			}
		}
	}

	private void Attack(Character owner, Bounds bounds, Target target, BoundsAttackInfo attackInfo)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)target == (Object)null)
		{
			Debug.LogError((object)"Target is null in GrabbedTargetAttack");
		}
		else
		{
			if (!((Behaviour)target).isActiveAndEnabled)
			{
				return;
			}
			Bounds val = bounds;
			Bounds bounds2 = target.collider.bounds;
			Bounds bounds3 = default(Bounds);
			((Bounds)(ref bounds3)).min = Vector2.op_Implicit(MMMaths.Max(Vector2.op_Implicit(((Bounds)(ref val)).min), Vector2.op_Implicit(((Bounds)(ref bounds2)).min)));
			((Bounds)(ref bounds3)).max = Vector2.op_Implicit(MMMaths.Min(Vector2.op_Implicit(((Bounds)(ref val)).max), Vector2.op_Implicit(((Bounds)(ref bounds2)).max)));
			Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds3);
			Vector2 force = Vector2.zero;
			if (attackInfo.pushInfo != null)
			{
				(Vector2, Vector2) tuple = attackInfo.pushInfo.EvaluateTimeIndependent(owner, target);
				force = tuple.Item1 + tuple.Item2;
			}
			if ((Object)(object)target.character != (Object)null && target.character.liveAndActive && !((Object)(object)target.character == (Object)(object)owner) && !target.character.cinematic.value)
			{
				attackInfo.ApplyChrono(owner, target.character);
				if (attackInfo.operationsToOwner.components.Length != 0)
				{
					((MonoBehaviour)owner).StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
				}
				Damage damage = owner.stat.GetDamage(_attackDamage.amount, hitPoint, attackInfo.hitInfo);
				if (attackInfo.hitInfo.attackType != 0)
				{
					CommonResource.instance.hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, force);
				}
				if (owner.TryAttackCharacter(target, ref damage))
				{
					((MonoBehaviour)owner).StartCoroutine(attackInfo.operationInfo.CRun(owner, target.character));
					this.onHit?.Invoke(target, ref damage);
					attackInfo.effect.Spawn(owner, bounds3, in damage, target);
				}
			}
		}
	}

	private IEnumerator CAttack(Character owner, Bounds bounds, Target target)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		int index = 0;
		float time = 0f;
		while ((Object)(object)this != (Object)null && index < _attackAndEffect.components.Length)
		{
			for (; index < _attackAndEffect.components.Length; index++)
			{
				BoundsAttackInfoSequence boundsAttackInfoSequence;
				if (!(time >= (boundsAttackInfoSequence = _attackAndEffect.components[index]).timeToTrigger))
				{
					break;
				}
				Attack(owner, bounds, target, boundsAttackInfoSequence.attackInfo);
			}
			yield return null;
			time += owner.chronometer.animation.deltaTime;
		}
	}
}
