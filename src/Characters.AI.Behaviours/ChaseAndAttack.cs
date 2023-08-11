using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours.Attacks;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class ChaseAndAttack : Behaviour
{
	[Chase.Subcomponent(true)]
	[SerializeField]
	private Chase _chase;

	[SerializeField]
	[Attack.Subcomponent(true)]
	private Attack _attack;

	[SerializeField]
	private Collider2D _attackCollider;

	public Chase chase => _chase;

	public Attack attack => _attack;

	private void Start()
	{
		_childs = new List<Behaviour> { _chase, _attack };
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		while (base.result == Result.Doing)
		{
			yield return null;
			if ((Object)(object)controller.target == (Object)null)
			{
				base.result = Result.Done;
				break;
			}
			if (!controller.character.movement.controller.isGrounded)
			{
				continue;
			}
			if (controller.dead)
			{
				break;
			}
			if ((Object)(object)controller.FindClosestPlayerBody(_attackCollider) != (Object)null)
			{
				yield return _attack.CRun(controller);
				continue;
			}
			yield return _chase.CRun(controller);
			if (_chase.result == Result.Success)
			{
				yield return _attack.CRun(controller);
			}
		}
	}
}
