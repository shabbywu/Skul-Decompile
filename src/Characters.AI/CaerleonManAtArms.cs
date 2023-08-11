using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class CaerleonManAtArms : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[Wander.Subcomponent(true)]
	[SerializeField]
	private Wander _wander;

	[SerializeField]
	[Chase.Subcomponent(true)]
	private Chase _chase;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private ActionAttack _attack;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private ActionAttack _tackle;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _idle;

	[SerializeField]
	private Collider2D _attackCollider;

	[SerializeField]
	private Collider2D _tackleCollider;

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
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null || !character.movement.controller.isGrounded)
			{
				continue;
			}
			if (_tackle.CanUse())
			{
				stopTrigger = _tackleCollider;
				if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_tackleCollider)))
				{
					yield return _tackle.CRun(this);
					stopTrigger = _attackCollider;
				}
				else if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_attackCollider)))
				{
					yield return _attack.CRun(this);
				}
				else
				{
					yield return _chase.CRun(this);
				}
				continue;
			}
			stopTrigger = _attackCollider;
			if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_attackCollider)))
			{
				yield return _attack.CRun(this);
				continue;
			}
			yield return _chase.CRun(this);
			if (_attack.result == Behaviour.Result.Success)
			{
				yield return _attack.CRun(this);
			}
		}
	}
}
