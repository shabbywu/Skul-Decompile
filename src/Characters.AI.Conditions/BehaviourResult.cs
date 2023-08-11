using Characters.AI.Behaviours;
using UnityEngine;

namespace Characters.AI.Conditions;

public class BehaviourResult : Condition
{
	[SerializeField]
	private Behaviour _behaviour;

	protected override bool Check(AIController controller)
	{
		return _behaviour.result == Behaviour.Result.Success;
	}
}
