using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class MoveForDuration : Move
{
	[SerializeField]
	private bool _flipAtPlatformEdge = true;

	[SerializeField]
	[MinMaxSlider(0f, 10f)]
	private Vector2 _duration;

	[SerializeField]
	private bool _doIdle = true;

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		Bounds platformBounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
		Bounds bounds = ((Collider2D)character.collider).bounds;
		float rightWidth = ((Bounds)(ref bounds)).max.x - ((Bounds)(ref bounds)).center.x;
		float leftWidth = ((Bounds)(ref bounds)).center.x - ((Bounds)(ref bounds)).min.x;
		base.result = Result.Doing;
		((MonoBehaviour)this).StartCoroutine(CExpire(controller, _duration));
		while (base.result.Equals(Result.Doing))
		{
			if (wander && (Object)(object)controller.target != (Object)null)
			{
				character.movement.move = direction;
				base.result = Result.Done;
				yield break;
			}
			if (checkWithinSight && LookAround(controller))
			{
				character.movement.move = direction;
				base.result = Result.Done;
				yield break;
			}
			character.movement.move = direction;
			if (character.movement.controller.velocity.x != 0f)
			{
				if (character.lookingDirection == Character.LookingDirection.Right && character.movement.controller.collisionState.right)
				{
					direction = Vector2.left;
					yield return null;
					continue;
				}
				if (character.lookingDirection == Character.LookingDirection.Left && character.movement.controller.collisionState.left)
				{
					direction = Vector2.right;
					yield return null;
					continue;
				}
			}
			if (_flipAtPlatformEdge)
			{
				if (((Bounds)(ref platformBounds)).max.x - rightWidth < ((Component)character).transform.position.x && direction.x > 0f)
				{
					direction = Vector2.left;
				}
				else if (((Bounds)(ref platformBounds)).min.x + leftWidth > ((Component)character).transform.position.x && direction.x < 0f)
				{
					direction = Vector2.right;
				}
			}
			yield return null;
		}
		if (_doIdle)
		{
			yield return idle.CRun(controller);
		}
	}
}
