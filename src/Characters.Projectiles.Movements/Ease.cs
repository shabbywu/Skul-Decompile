using UnityEngine;

namespace Characters.Projectiles.Movements;

public class Ease : Movement
{
	[SerializeField]
	private float _startSpeed;

	[SerializeField]
	private float _targetSpeed;

	[SerializeField]
	private float _easingTime;

	[SerializeField]
	private EasingFunction.Method _easingMethod;

	private EasingFunction.Function _easingFunction;

	public override void Initialize(IProjectile projectile, float direction)
	{
		base.Initialize(projectile, direction);
		_easingFunction = EasingFunction.GetEasingFunction(_easingMethod);
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		float num = _easingFunction(_startSpeed, _targetSpeed, time / _easingTime);
		return (base.directionVector, num * base.projectile.speedMultiplier);
	}
}
