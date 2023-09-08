using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope;

public sealed class Baptism : Behaviour
{
	[UnityEditor.Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _attack;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		yield return _attack.CRun(controller);
		base.result = Result.Success;
	}
}
