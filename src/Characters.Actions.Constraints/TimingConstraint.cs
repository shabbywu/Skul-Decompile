using UnityEngine;

namespace Characters.Actions.Constraints;

public class TimingConstraint : Constraint
{
	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	[Range(0f, 1f)]
	private Vector2 _timingCanCancel;

	public override bool Pass()
	{
		if (!((Object)(object)_action.owner.runningMotion == (Object)null))
		{
			if (_timingCanCancel.x <= _action.owner.runningMotion.normalizedTime)
			{
				return _action.owner.runningMotion.normalizedTime <= _timingCanCancel.y;
			}
			return false;
		}
		return true;
	}
}
