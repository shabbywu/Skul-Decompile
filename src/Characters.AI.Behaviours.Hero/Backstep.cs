using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public sealed class Backstep : Behaviour
{
	[SerializeField]
	private Action _action;

	public override IEnumerator CRun(AIController controller)
	{
		_action.TryStart();
		while (_action.running)
		{
			yield return null;
		}
	}

	private void LookSide(Character character)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)character.movement.controller.collisionState.lastStandingCollider == (Object)null))
		{
			Bounds bounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			float x = ((Bounds)(ref bounds)).center.x;
			if (((Component)character).transform.position.x > x)
			{
				character.ForceToLookAt(Character.LookingDirection.Right);
			}
			else
			{
				character.ForceToLookAt(Character.LookingDirection.Left);
			}
		}
	}
}
