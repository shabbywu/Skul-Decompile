using BT;
using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public sealed class BTTarget : Target
{
	[SerializeField]
	private BehaviourTreeRunner _bt;

	public override Character.LookingDirection GetDirectionFrom(Character character)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Character character2 = _bt.context.Get<Character>(BT.Key.Target);
		if ((Object)(object)character2 == (Object)null)
		{
			return character.lookingDirection;
		}
		if (((Component)character2).transform.position.x > ((Component)character).transform.position.x)
		{
			return Character.LookingDirection.Right;
		}
		return Character.LookingDirection.Left;
	}
}
