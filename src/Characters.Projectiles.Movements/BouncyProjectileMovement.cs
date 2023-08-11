using System;
using UnityEngine;

namespace Characters.Projectiles.Movements;

[Serializable]
public class BouncyProjectileMovement
{
	[SerializeField]
	private float _speed;

	[SerializeField]
	private float _gravity;

	[SerializeField]
	private float _maxFallSpeed;

	[SerializeField]
	[Tooltip("X축 스피드가 speed 값으로 고정")]
	private bool _fixedXSpeed;

	public float ySpeed;

	private Vector2 _velocity;

	public IProjectile projectile { get; private set; }

	public Vector2 directionVector { get; set; }

	public void Initialize(IProjectile projectile, float direction)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		this.projectile = projectile;
		float num = direction * ((float)Math.PI / 180f);
		directionVector = new Vector2(Mathf.Cos(num), Mathf.Sin(num));
		ySpeed = 0f;
	}

	public (Vector2 direction, float speed) GetSpeed(float deltaTime)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		float num = _speed * projectile.speedMultiplier;
		_velocity = num * directionVector;
		ySpeed -= _gravity * deltaTime;
		_velocity.y += ySpeed;
		if (_velocity.y < _maxFallSpeed)
		{
			_velocity.y = _maxFallSpeed;
		}
		if (_fixedXSpeed)
		{
			_velocity.x = ((directionVector.x > 0f) ? num : (0f - num));
		}
		return (((Vector2)(ref _velocity)).normalized, ((Vector2)(ref _velocity)).magnitude);
	}
}
