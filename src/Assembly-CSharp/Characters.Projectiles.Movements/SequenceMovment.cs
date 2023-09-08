using UnityEngine;

namespace Characters.Projectiles.Movements;

public class SequenceMovment : Movement
{
	[SerializeField]
	[Subcomponent]
	private Movement _firstMovement;

	[SerializeField]
	private float _firstMovePlayTime;

	[SerializeField]
	[Subcomponent]
	private Movement _secondMovement;

	public override void Initialize(IProjectile projectile, float direction)
	{
		_firstMovement.Initialize(projectile, direction);
		_secondMovement.Initialize(projectile, direction);
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		if (time < _firstMovePlayTime)
		{
			return _firstMovement.GetSpeed(time, deltaTime);
		}
		return _secondMovement.GetSpeed(time, deltaTime);
	}
}
