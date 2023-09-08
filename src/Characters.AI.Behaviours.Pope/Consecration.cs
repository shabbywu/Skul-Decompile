using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope;

public sealed class Consecration : Behaviour
{
	[UnityEditor.Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _attack;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Done;
		yield return _attack.CRun(controller);
		base.result = Result.Success;
	}
}
