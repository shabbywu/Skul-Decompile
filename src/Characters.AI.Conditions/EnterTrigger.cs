using UnityEngine;

namespace Characters.AI.Conditions;

public class EnterTrigger : Condition
{
	[SerializeField]
	private Collider2D _trigger;

	protected override bool Check(AIController controller)
	{
		return Object.op_Implicit((Object)(object)controller.FindClosestPlayerBody(_trigger));
	}
}
