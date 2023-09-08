using UnityEngine;

namespace Characters.Projectiles.Movements;

public class Homing : Movement
{
	public enum RotateMethod
	{
		Constant,
		Lerp,
		Slerp
	}

	[SerializeField]
	private TargetFinder _finder;

	[SerializeField]
	private float _delay;

	[SerializeField]
	private RotateMethod _rotateMethod;

	[SerializeField]
	private float _rotateSpeed = 2f;

	[SerializeField]
	private float _rotateSpeedAcc;

	[SerializeField]
	private float _startSpeed;

	[SerializeField]
	private float _targetSpeed;

	[SerializeField]
	private AnimationCurve _curve;

	[SerializeField]
	private float _easingTime;

	private Target _target;

	private Quaternion _rotation;

	private float _currentRotateSpeed;

	[SerializeField]
	private float _timeToStopChasing;

	private bool _doNotFound;

	public override void Initialize(IProjectile projectile, float direction)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		base.Initialize(projectile, direction);
		_finder.Initialize(projectile);
		_target = null;
		_doNotFound = false;
		_rotation = Quaternion.Euler(0f, 0f, direction);
		_currentRotateSpeed = _rotateSpeed;
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (time >= _delay)
		{
			UpdateTarget();
			UpdateDirection(deltaTime);
		}
		if (_timeToStopChasing > 0f && time > _timeToStopChasing)
		{
			_target = null;
			_doNotFound = true;
		}
		float num = _startSpeed + (_targetSpeed - _startSpeed) * _curve.Evaluate(time / _easingTime);
		return (base.directionVector, num * base.projectile.speedMultiplier);
	}

	private void UpdateTarget()
	{
		if ((!((Object)(object)_target != (Object)null) || (!((Object)(object)_target.character == (Object)null) && _target.character.health.dead)) && !((Object)(object)_finder.range == (Object)null) && !_doNotFound)
		{
			Target target = _finder.Find();
			if ((Object)(object)target != (Object)null && (Object)(object)target.character != (Object)null)
			{
				_target = target;
			}
		}
	}

	private void UpdateDirection(float deltaTime)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_target == (Object)null) && !((Object)(object)_target.character == (Object)null))
		{
			Bounds bounds = _target.collider.bounds;
			Vector3 val = ((Bounds)(ref bounds)).center - ((Component)this).transform.position;
			float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
			_currentRotateSpeed += _rotateSpeedAcc * deltaTime;
			switch (_rotateMethod)
			{
			case RotateMethod.Constant:
				_rotation = Quaternion.RotateTowards(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _currentRotateSpeed * 100f * deltaTime);
				break;
			case RotateMethod.Lerp:
				_rotation = Quaternion.Lerp(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _currentRotateSpeed * deltaTime);
				break;
			case RotateMethod.Slerp:
				_rotation = Quaternion.Slerp(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _currentRotateSpeed * deltaTime);
				break;
			}
			base.direction = ((Quaternion)(ref _rotation)).eulerAngles.z;
			base.directionVector = Vector2.op_Implicit(_rotation * Vector3.right);
		}
	}
}
