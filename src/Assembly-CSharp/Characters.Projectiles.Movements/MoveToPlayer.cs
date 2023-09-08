using Services;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Movements;

public class MoveToPlayer : Movement
{
	[SerializeField]
	private float _speed = 1f;

	private Vector2 _destination;

	private IProjectile _projectile;

	public override void Initialize(IProjectile projectile, float direction)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		_projectile = projectile;
		Character player = Singleton<Service>.Instance.levelManager.player;
		_destination = new Vector2(((Component)player).transform.position.x, projectile.transform.position.y);
		base.Initialize(projectile, direction);
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		if (base.directionVector.x > 0f && base.projectile.transform.position.x >= _destination.x)
		{
			_projectile.Despawn();
		}
		else if (base.directionVector.x < 0f && base.projectile.transform.position.x <= _destination.x)
		{
			_projectile.Despawn();
		}
		return (base.directionVector, _speed * base.projectile.speedMultiplier);
	}
}
