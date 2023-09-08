using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class CaerleonAssassinAI : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Wander.Subcomponent(true)]
	private Wander _wander;

	[SerializeField]
	[Attack.Subcomponent(true)]
	private ActionAttack _attack;

	[SerializeField]
	[Subcomponent(typeof(Confusing))]
	private Confusing _confusing;

	[Chase.Subcomponent(true)]
	[SerializeField]
	private Chase _chase;

	[Subcomponent(typeof(ChaseTeleport))]
	[SerializeField]
	private ChaseTeleport _chaseTelport;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _idle;

	[SerializeField]
	private Collider2D _attackCollider;

	[SerializeField]
	private Collider2D _teleportTrigger;

	private bool _chaseTeleportAttack;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _chaseTelport, _idle };
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		while (!base.dead)
		{
			yield return _wander.CRun(this);
			yield return _idle.CRun(this);
			((MonoBehaviour)this).StartCoroutine(ChaseWithJump());
			yield return Combat();
		}
	}

	private IEnumerator ChaseWithJump()
	{
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)base.target == (Object)null) && _chaseTelport.CanUse() && _chaseTelport.result != Behaviour.Result.Doing && base.target.movement.controller.isGrounded && !((Object)(object)FindClosestPlayerBody(_teleportTrigger) == (Object)null) && !((Object)(object)base.target.movement.controller.collisionState.lastStandingCollider == (Object)(object)character.movement.controller.collisionState.lastStandingCollider))
			{
				StopAllBehaviour();
				character.movement.moveBackward = false;
				_chaseTeleportAttack = true;
				yield return _chaseTelport.CRun(this);
				character.ForceToLookAt(((Component)base.target).transform.position.x);
				yield return character.chronometer.master.WaitForSeconds(0.3f);
				yield return _attack.CRun(this);
				yield return _confusing.CRun(this);
				_chaseTeleportAttack = false;
			}
		}
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null)
			{
				break;
			}
			if (!character.movement.controller.isGrounded || _chaseTelport.result == Behaviour.Result.Doing || _chaseTeleportAttack)
			{
				continue;
			}
			if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_attackCollider)))
			{
				yield return _attack.CRun(this);
				yield return _confusing.CRun(this);
				continue;
			}
			yield return _chase.CRun(this);
			if (_chase.result == Behaviour.Result.Success)
			{
				yield return _attack.CRun(this);
			}
		}
	}
}
