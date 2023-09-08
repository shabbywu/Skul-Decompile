using Characters;
using UnityEngine;

namespace BT.Conditions;

public class TargetOnOwnerPlatform : Condition
{
	protected override bool Check(Context context)
	{
		Character character = context.Get<Character>(Key.Target);
		Character character2 = context.Get<Character>(Key.OwnerCharacter);
		if ((Object)(object)character == (Object)null || (Object)(object)character2 == (Object)null)
		{
			return false;
		}
		Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
		Collider2D lastStandingCollider2 = character2.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider != (Object)(object)lastStandingCollider2)
		{
			return false;
		}
		return true;
	}
}
