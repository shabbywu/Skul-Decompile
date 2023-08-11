using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class GoldManeArcherAI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[Wander.Subcomponent(true)]
	[SerializeField]
	private Wander _wander;

	[Subcomponent(typeof(KeepDistance))]
	[SerializeField]
	private KeepDistance _keepDistanceWithJump;

	[SerializeField]
	[Subcomponent(typeof(KeepDistance))]
	private KeepDistance _keepDistanceWithMove;

	[SerializeField]
	[Subcomponent(typeof(HorizontalProjectileAttack))]
	private HorizontalProjectileAttack _attack;

	[SerializeField]
	[Chase.Subcomponent(true)]
	private Chase _chase;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _idle;

	[SerializeField]
	private Collider2D _minimumCollider;

	[SerializeField]
	private Collider2D _attackCollider;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _keepDistanceWithJump, _keepDistanceWithMove, _attack, _idle };
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
		yield return _wander.CRun(this);
		yield return _idle.CRun(this);
		yield return Combat();
	}

	private IEnumerator Combat()
	{
		((MonoBehaviour)this).StartCoroutine(ProcessBackStep());
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null || base.stuned || !character.movement.controller.isGrounded || _keepDistanceWithJump.result == Behaviour.Result.Doing || _keepDistanceWithMove.result == Behaviour.Result.Doing || _attack.result == Behaviour.Result.Doing || (Object)(object)FindClosestPlayerBody(_minimumCollider) != (Object)null)
			{
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_attackCollider) != (Object)null)
			{
				yield return _attack.CRun(this);
				continue;
			}
			yield return _chase.CRun(this);
			if (_chase.result == Behaviour.Result.Success)
			{
				yield return _attack.CRun(this);
			}
		}
	}

	private IEnumerator ProcessBackStep()
	{
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)FindClosestPlayerBody(_minimumCollider) == (Object)null) && _attack.result != Behaviour.Result.Doing)
			{
				StopAllBehaviour();
				if (_keepDistanceWithJump.CanUseBackStep())
				{
					yield return _keepDistanceWithJump.CRun(this);
				}
				else if (_keepDistanceWithMove.CanUseBackMove())
				{
					yield return _keepDistanceWithMove.CRun(this);
					yield return _attack.CRun(this);
				}
			}
		}
	}
}
