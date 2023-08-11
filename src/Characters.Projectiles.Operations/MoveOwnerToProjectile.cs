using Characters.Movements;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class MoveOwnerToProjectile : Operation
{
	public override void Run(IProjectile projectile)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Movement movement = projectile.owner.movement;
		if (movement.controller.Teleport(Vector2.op_Implicit(projectile.transform.position), -projectile.firedDirection, 5f))
		{
			movement.verticalVelocity = 0f;
		}
	}
}
