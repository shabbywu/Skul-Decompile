using Characters.AI;
using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public sealed class AITarget : Target
{
	[SerializeField]
	private AIController _aIController;

	public override Character.LookingDirection GetDirectionFrom(Character character)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Character target = _aIController.target;
		if ((Object)(object)target == (Object)null)
		{
			return character.lookingDirection;
		}
		if (((Component)target).transform.position.x > ((Component)character).transform.position.x)
		{
			return Character.LookingDirection.Right;
		}
		return Character.LookingDirection.Left;
	}
}
