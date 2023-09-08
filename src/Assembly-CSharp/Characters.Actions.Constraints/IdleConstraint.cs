using UnityEngine;

namespace Characters.Actions.Constraints;

public class IdleConstraint : Constraint
{
	public override bool Pass()
	{
		return (Object)(object)_action.owner.runningMotion == (Object)null;
	}
}
