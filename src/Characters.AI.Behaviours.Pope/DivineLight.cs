using System.Collections;
using Characters.AI.Behaviours.Attacks;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope;

public sealed class DivineLight : Behaviour
{
	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _attack;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _hardmodeAttack;

	[Subcomponent(typeof(MoveHandler))]
	[SerializeField]
	private MoveHandler _moveHandler;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		yield return _moveHandler.CMove(controller);
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			yield return _hardmodeAttack.CRun(controller);
		}
		else
		{
			yield return _attack.CRun(controller);
		}
		base.result = Result.Success;
	}
}
