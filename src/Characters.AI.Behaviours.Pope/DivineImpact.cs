using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope;

public sealed class DivineImpact : Behaviour
{
	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _attack;

	[Subcomponent(typeof(MoveHandler))]
	[SerializeField]
	private MoveHandler _moveHandler;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		yield return _moveHandler.CMove(controller);
		yield return _attack.CRun(controller);
		base.result = Result.Success;
	}
}
