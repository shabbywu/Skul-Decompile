using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope;

public sealed class TentacleSummon : Behaviour
{
	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _attack;

	public override IEnumerator CRun(AIController controller)
	{
		yield return _attack.CRun(controller);
	}
}
