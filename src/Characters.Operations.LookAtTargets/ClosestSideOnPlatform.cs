using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public class ClosestSideOnPlatform : Target
{
	[SerializeField]
	private bool _farthest;

	public override Character.LookingDirection GetDirectionFrom(Character character)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider == (Object)null)
		{
			return character.lookingDirection;
		}
		float x = ((Component)character).transform.position.x;
		Bounds bounds = lastStandingCollider.bounds;
		if (x > ((Bounds)(ref bounds)).center.x)
		{
			if (!_farthest)
			{
				return Character.LookingDirection.Right;
			}
			return Character.LookingDirection.Left;
		}
		if (!_farthest)
		{
			return Character.LookingDirection.Left;
		}
		return Character.LookingDirection.Right;
	}
}
