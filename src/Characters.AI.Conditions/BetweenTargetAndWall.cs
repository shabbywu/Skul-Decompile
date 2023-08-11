using UnityEngine;

namespace Characters.AI.Conditions;

public sealed class BetweenTargetAndWall : Condition
{
	protected override bool Check(AIController controller)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Character target = controller.target;
		Character character = controller.character;
		Collider2D lastStandingCollider = controller.character.movement.controller.collisionState.lastStandingCollider;
		float x = ((Component)character).transform.position.x;
		Bounds bounds = lastStandingCollider.bounds;
		if (x < ((Bounds)(ref bounds)).center.x)
		{
			if (((Component)character).transform.position.x <= ((Component)target).transform.position.x)
			{
				return true;
			}
			return false;
		}
		if (((Component)character).transform.position.x >= ((Component)target).transform.position.x)
		{
			return true;
		}
		return false;
	}
}
