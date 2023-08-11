using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class MoveToTarget : Move
{
	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		Character target = controller.target;
		base.result = Result.Doing;
		while (base.result == Result.Doing)
		{
			if ((Object)(object)target.movement.controller.collisionState.lastStandingCollider == (Object)null)
			{
				yield return null;
				continue;
			}
			if ((Object)(object)controller.target == (Object)null || !Precondition.CanChase(character, controller.target))
			{
				base.result = Result.Fail;
				break;
			}
			Bounds bounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			Bounds bounds2 = target.movement.controller.collisionState.lastStandingCollider.bounds;
			float y = ((Bounds)(ref bounds)).center.y;
			Bounds bounds3 = ((Collider2D)character.collider).bounds;
			if (y - ((Bounds)(ref bounds3)).size.y > ((Bounds)(ref bounds2)).center.y)
			{
				character.movement.move = ((character.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left);
				yield return null;
				continue;
			}
			float num = ((Component)controller.target).transform.position.x - ((Component)character).transform.position.x;
			if (Mathf.Abs(num) < 0.1f || LookAround(controller))
			{
				yield return idle.CRun(controller);
				base.result = Result.Success;
				break;
			}
			character.movement.move = ((num > 0f) ? Vector2.right : Vector2.left);
			yield return null;
		}
	}
}
