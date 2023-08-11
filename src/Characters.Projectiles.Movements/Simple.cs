using UnityEngine;

namespace Characters.Projectiles.Movements;

public class Simple : Movement
{
	[SerializeField]
	private float _speed;

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return (base.directionVector, _speed * base.projectile.speedMultiplier);
	}
}
