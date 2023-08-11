using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public class PlatformPoint : Target
{
	private enum Point
	{
		Left,
		Center,
		Right
	}

	[SerializeField]
	private Point _point;

	public override Character.LookingDirection GetDirectionFrom(Character character)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider == (Object)null)
		{
			return character.lookingDirection;
		}
		float x = ((Component)character).transform.position.x;
		Bounds bounds;
		switch (_point)
		{
		case Point.Left:
		{
			bounds = lastStandingCollider.bounds;
			float x4 = ((Bounds)(ref bounds)).min.x;
			if (x >= x4)
			{
				return Character.LookingDirection.Left;
			}
			return Character.LookingDirection.Right;
		}
		case Point.Center:
		{
			bounds = lastStandingCollider.bounds;
			float x3 = ((Bounds)(ref bounds)).center.x;
			if (x >= x3)
			{
				return Character.LookingDirection.Left;
			}
			return Character.LookingDirection.Right;
		}
		case Point.Right:
		{
			bounds = lastStandingCollider.bounds;
			float x2 = ((Bounds)(ref bounds)).max.x;
			if (x >= x2)
			{
				return Character.LookingDirection.Left;
			}
			return Character.LookingDirection.Right;
		}
		default:
			return character.lookingDirection;
		}
	}
}
