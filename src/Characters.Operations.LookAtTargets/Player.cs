using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public sealed class Player : Target
{
	public override Character.LookingDirection GetDirectionFrom(Character character)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		if ((Object)(object)player == (Object)null)
		{
			return character.lookingDirection;
		}
		if (((Component)player).transform.position.x > ((Component)character).transform.position.x)
		{
			return Character.LookingDirection.Right;
		}
		return Character.LookingDirection.Left;
	}
}
