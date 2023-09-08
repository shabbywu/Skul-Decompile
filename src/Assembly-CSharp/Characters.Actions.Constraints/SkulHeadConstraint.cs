using UnityEngine;

namespace Characters.Actions.Constraints;

public class SkulHeadConstraint : Constraint
{
	public override bool Pass()
	{
		if ((Object)(object)SkulHeadToTeleport.instance != (Object)null)
		{
			return ((Component)SkulHeadToTeleport.instance).gameObject.activeSelf;
		}
		return false;
	}
}
