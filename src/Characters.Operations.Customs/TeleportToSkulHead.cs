using UnityEngine;

namespace Characters.Operations.Customs;

public class TeleportToSkulHead : CharacterOperation
{
	[SerializeField]
	private SkulHeadController _skulHeadController;

	public override void Run(Character owner)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)SkulHeadToTeleport.instance).transform.position;
		if (owner.movement.controller.TeleportUponGround(Vector2.op_Implicit(position), 1.5f) || owner.movement.controller.Teleport(Vector2.op_Implicit(position), 3f))
		{
			_skulHeadController.cooldown.time.remainTime = 0f;
			SkulHeadToTeleport.instance.Despawn();
		}
	}
}
