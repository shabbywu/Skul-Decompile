using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class GoldmaneSpearMan : AIController
{
	[SerializeField]
	private Collider2D _upperAttackCollider;

	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Wander.Subcomponent(true)]
	private Wander _wander;

	[Subcomponent(typeof(ChaseAndAttack))]
	[SerializeField]
	private ChaseAndAttack _chaseAndAttack;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _upperAttack;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _idle;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _chaseAndAttack, _upperAttack, _wander, _idle };
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
		((MonoBehaviour)this).StartCoroutine(ProcessForUpperAttack());
		while (!base.dead)
		{
			yield return _wander.CRun(this);
			yield return _idle.CRun(this);
			yield return Combat();
		}
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)base.target == (Object)null) && character.movement.controller.isGrounded && _upperAttack.result != Behaviour.Result.Doing)
			{
				yield return _chaseAndAttack.CRun(this);
			}
		}
	}

	private IEnumerator ProcessForUpperAttack()
	{
		while (!base.dead)
		{
			yield return null;
			if (_upperAttack.CanUse() && _chaseAndAttack.attack.result != Behaviour.Result.Doing && !((Object)(object)FindClosestPlayerBody(_upperAttackCollider) == (Object)null))
			{
				StopAllBehaviour();
				yield return DoUpperAttack();
			}
		}
	}

	private IEnumerator DoUpperAttack()
	{
		yield return _upperAttack.CRun(this);
	}
}
