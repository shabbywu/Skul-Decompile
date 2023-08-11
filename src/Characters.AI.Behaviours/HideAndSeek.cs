using System.Collections;
using Characters.Actions;
using Characters.Movements;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class HideAndSeek : Behaviour
{
	[SerializeField]
	private Action _attackReady;

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		Character target = controller.target;
		if ((Object)(object)target == (Object)null)
		{
			base.result = Result.Fail;
			yield break;
		}
		CharacterController2D.CollisionState targetCollisionState = target.movement.controller.collisionState;
		base.result = Result.Doing;
		while (base.result == Result.Doing)
		{
			if ((Object)(object)target == (Object)null)
			{
				base.result = Result.Fail;
				break;
			}
			if (!CanChase())
			{
				yield return null;
			}
			else if (Object.op_Implicit((Object)(object)controller.FindClosestPlayerBody(controller.stopTrigger)))
			{
				if (!_attackReady.TryStart())
				{
					yield return null;
					continue;
				}
				while (_attackReady.running && base.result == Result.Doing)
				{
					yield return null;
					if (!CanChase())
					{
						character.CancelAction();
						base.result = Result.Fail;
						break;
					}
				}
				if (base.result != 0)
				{
					base.result = Result.Success;
					break;
				}
				base.result = Result.Doing;
			}
			else
			{
				float num = ((Component)controller.target).transform.position.x - ((Component)character).transform.position.x;
				character.movement.move = ((num > 0f) ? Vector2.right : Vector2.left);
				yield return null;
			}
		}
		bool CanChase()
		{
			if ((Object)(object)targetCollisionState.lastStandingCollider == (Object)null)
			{
				return false;
			}
			if (!Precondition.CanChase(character, target))
			{
				return false;
			}
			if (isFacingEachOther(character, target))
			{
				return false;
			}
			return true;
		}
	}

	private bool isFacingEachOther(Character character, Character target)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		character.ForceToLookAt(((Component)target).transform.position.x);
		if (character.lookingDirection == Character.LookingDirection.Right && target.lookingDirection == Character.LookingDirection.Left)
		{
			return true;
		}
		if (character.lookingDirection == Character.LookingDirection.Left && target.lookingDirection == Character.LookingDirection.Right)
		{
			return true;
		}
		return false;
	}
}
