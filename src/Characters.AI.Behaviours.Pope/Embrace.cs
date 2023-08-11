using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope;

public sealed class Embrace : Behaviour
{
	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _pull;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _attack;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _end;

	[SerializeField]
	[Subcomponent(typeof(MoveHandler))]
	private MoveHandler _moveHandler;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		yield return _moveHandler.CMove(controller);
		yield return _pull.CRun(controller);
		yield return _attack.CRun(controller);
		yield return _end.CRun(controller);
		base.result = Result.Success;
	}
}
