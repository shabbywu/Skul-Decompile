using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class MoveToPlatformEdge : Move
{
	[SerializeField]
	private float _distanceToEdge;

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		Bounds bounds = ((Collider2D)character.collider).bounds;
		float rightWidth = ((Bounds)(ref bounds)).max.x - ((Bounds)(ref bounds)).center.x;
		float leftWidth = ((Bounds)(ref bounds)).center.x - ((Bounds)(ref bounds)).min.x;
		base.result = Result.Doing;
		while (base.result.Equals(Result.Doing))
		{
			if (Object.op_Implicit((Object)(object)character.movement.controller.collisionState.lastStandingCollider))
			{
				Bounds bounds2 = character.movement.controller.collisionState.lastStandingCollider.bounds;
				if (wander && (Object)(object)controller.target != (Object)null)
				{
					character.movement.MoveHorizontal(direction);
					base.result = Result.Done;
					break;
				}
				if (checkWithinSight && LookAround(controller))
				{
					character.movement.MoveHorizontal(direction);
					base.result = Result.Done;
					break;
				}
				character.movement.MoveHorizontal(direction);
				if ((((Bounds)(ref bounds2)).max.x - rightWidth - _distanceToEdge < ((Component)character).transform.position.x && direction.x > 0f) || (((Bounds)(ref bounds2)).min.x + leftWidth + _distanceToEdge > ((Component)character).transform.position.x && direction.x < 0f))
				{
					yield return idle.CRun(controller);
					if (direction.x > 0f)
					{
						direction = Vector2.left;
					}
					else if (direction.x < 0f)
					{
						direction = Vector2.right;
					}
				}
				yield return null;
			}
			else
			{
				yield return null;
			}
		}
	}
}
