using Characters.AI;
using UnityEngine;

namespace Characters.Operations;

public class LookTarget : CharacterOperation
{
	[SerializeField]
	private AIController _controller;

	public override void Run(Character owner)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_controller.target == (Object)null))
		{
			owner.ForceToLookAt(((Component)_controller.target).transform.position.x);
		}
	}
}
