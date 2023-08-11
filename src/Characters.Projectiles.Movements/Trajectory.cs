using UnityEngine;

namespace Characters.Projectiles.Movements;

public class Trajectory : Movement
{
	[SerializeField]
	private TargetFinder _finder;

	[SerializeField]
	private float _startSpeed;

	[SerializeField]
	private float _targetSpeed;

	[SerializeField]
	private AnimationCurve _curve;

	[SerializeField]
	private float _easingTime;

	[SerializeField]
	private float _gravity;

	[SerializeField]
	[Tooltip("이 값만큼 초기 발사각에 더해집니다. 주로 투사체에 노이즈를 추가하여 산발되게하는 식으로 사용할 수 있습니다.")]
	private CustomFloat _extraAngle;

	private float _ySpeed;

	public override void Initialize(IProjectile projectile, float direction)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_finder.range != (Object)null)
		{
			_finder.Initialize(projectile);
			Target target = _finder.Find();
			if ((Object)(object)target != (Object)null)
			{
				Bounds bounds = target.collider.bounds;
				Vector3 val = ((Bounds)(ref bounds)).center - ((Component)this).transform.position;
				direction = Mathf.Atan2(val.y, val.x) * 57.29578f;
			}
		}
		direction += _extraAngle.value;
		base.Initialize(projectile, direction);
		_ySpeed = 0f;
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		float num = ((!(time < _easingTime)) ? _targetSpeed : (_startSpeed + (_targetSpeed - _startSpeed) * _curve.Evaluate(time / _easingTime)));
		num *= base.projectile.speedMultiplier;
		Vector2 val = num * base.directionVector;
		_ySpeed -= _gravity * deltaTime;
		val.y += _ySpeed;
		return (((Vector2)(ref val)).normalized, ((Vector2)(ref val)).magnitude);
	}
}
