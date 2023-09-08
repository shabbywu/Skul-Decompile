using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class Chance : Decorator
{
	[Range(0f, 1f)]
	[SerializeField]
	private float _successChance;

	[Subcomponent(true)]
	[SerializeField]
	private Behaviour _behaviour;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		if (MMMaths.Chance(_successChance))
		{
			yield return _behaviour.CRun(controller);
			base.result = Result.Success;
		}
		else
		{
			base.result = Result.Fail;
		}
	}
}
