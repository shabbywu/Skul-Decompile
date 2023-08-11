using System.Collections;
using Characters.Movements;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class MoveToDestination : Move
{
	[SerializeField]
	private float _endDistance = 1f;

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		base.result = Result.Doing;
		Vector2 move = ((controller.destination.x - ((Component)character).transform.position.x > 0f) ? Vector2.right : Vector2.left);
		character.movement.move = move;
		((MonoBehaviour)this).StartCoroutine(CanMove(controller));
		while (base.result.Equals(Result.Doing))
		{
			yield return null;
			if (!character.stunedOrFreezed)
			{
				if (wander && (Object)(object)controller.target != (Object)null)
				{
					base.result = Result.Success;
					break;
				}
				if (checkWithinSight && (Object)(object)controller.target != (Object)null && Precondition.CanChase(character, controller.target))
				{
					base.result = Result.Success;
					break;
				}
				float num = controller.destination.x - ((Component)character).transform.position.x;
				move = ((num > 0f) ? Vector2.right : Vector2.left);
				if (Mathf.Abs(num) < _endDistance)
				{
					base.result = Result.Done;
					yield return idle.CRun(controller);
					break;
				}
				character.movement.move = move;
			}
		}
	}

	private IEnumerator CanMove(AIController controller)
	{
		Character character = controller.character;
		CharacterController2D characterController = character.movement.controller;
		while (base.result == Result.Doing)
		{
			yield return null;
			if (!character.stunedOrFreezed && characterController.velocity.x == 0f)
			{
				base.result = Result.Fail;
				break;
			}
		}
	}
}
