using UnityEngine;

namespace Characters.AI;

public static class Precondition
{
	public static bool CanMove(Character character)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider == (Object)null)
		{
			return true;
		}
		Bounds bounds = lastStandingCollider.bounds;
		if (((Bounds)(ref bounds)).size.x > 3f)
		{
			return true;
		}
		return false;
	}

	public static bool CanChase(Character character, Character target)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)target == (Object)null || (Object)(object)target.movement == (Object)null || (Object)(object)target.movement.controller == (Object)null || target.movement.controller.collisionState == null || (Object)(object)target.movement.controller.collisionState.lastStandingCollider == (Object)null)
		{
			return false;
		}
		Collider2D lastStandingCollider2 = target.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider != (Object)(object)lastStandingCollider2)
		{
			return false;
		}
		Bounds bounds = lastStandingCollider.bounds;
		float y = ((Bounds)(ref bounds)).center.y;
		bounds = ((Collider2D)character.collider).bounds;
		float num = y - ((Bounds)(ref bounds)).size.y;
		bounds = lastStandingCollider2.bounds;
		if (num > ((Bounds)(ref bounds)).center.y)
		{
			bounds = lastStandingCollider.bounds;
			if (((Bounds)(ref bounds)).max.x > ((Component)target).transform.position.x)
			{
				bounds = lastStandingCollider.bounds;
				if (((Bounds)(ref bounds)).min.x < ((Component)target).transform.position.x)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool CanMoveToDirection(Character character, Vector2 direction, float minimumDistance)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider == (Object)null)
		{
			return false;
		}
		if (!CanMove(character))
		{
			return false;
		}
		float num = character.movement.velocity.x * ((ChronometerBase)character.chronometer.master).deltaTime;
		Bounds bounds;
		if (direction.x > 0f)
		{
			bounds = ((Collider2D)character.collider).bounds;
			float num2 = ((Bounds)(ref bounds)).max.x + num + minimumDistance;
			bounds = lastStandingCollider.bounds;
			if (num2 >= ((Bounds)(ref bounds)).max.x)
			{
				return false;
			}
		}
		if (direction.x < 0f)
		{
			bounds = ((Collider2D)character.collider).bounds;
			float num3 = ((Bounds)(ref bounds)).min.x + num - minimumDistance;
			bounds = lastStandingCollider.bounds;
			if (num3 <= ((Bounds)(ref bounds)).min.x)
			{
				return false;
			}
		}
		return true;
	}
}
