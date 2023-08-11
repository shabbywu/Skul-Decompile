using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public sealed class TargetObject : Target
{
	[SerializeField]
	private Transform _target;

	public override Character.LookingDirection GetDirectionFrom(Character character)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (!(_target.position.x < ((Component)character).transform.position.x))
		{
			return Character.LookingDirection.Right;
		}
		return Character.LookingDirection.Left;
	}
}
