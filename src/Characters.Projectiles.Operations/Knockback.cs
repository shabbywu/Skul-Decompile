using Characters.Movements;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class Knockback : CharacterHitOperation
{
	[SerializeField]
	private PushInfo _pushInfo = new PushInfo(ignoreOtherForce: false, expireOnGround: false);

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
	{
		character.movement.push.ApplyKnockback(projectile, _pushInfo);
	}
}
