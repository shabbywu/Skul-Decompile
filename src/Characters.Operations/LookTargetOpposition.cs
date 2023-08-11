using Characters.AI;
using UnityEngine;

namespace Characters.Operations;

public class LookTargetOpposition : CharacterOperation
{
	[SerializeField]
	private AIController _controller;

	public override void Run(Character owner)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_controller.target == (Object)null))
		{
			Character.LookingDirection lookingDirection = ((((Component)owner).transform.position.x < ((Component)_controller.target).transform.position.x) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			owner.ForceToLookAt(lookingDirection);
		}
	}
}
