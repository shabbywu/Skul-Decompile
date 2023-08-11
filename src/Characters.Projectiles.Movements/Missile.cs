using UnityEngine;

namespace Characters.Projectiles.Movements;

public class Missile : Movement
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

	[Header("Roatation")]
	[SerializeField]
	private RotateMethod _rotateMethod;

	[SerializeField]
	private float _rotateSpeed = 300f;

	[Header("Speed")]
	[SerializeField]
	private float _initialSpeed = 1f;

	[SerializeField]
	private float _acceleration = 2f;

	[SerializeField]
	private float _maxSpeed = 3f;

	private Target _target;

	private Vector2 _speed;

	private Quaternion _rotation;

	public override void Initialize(IProjectile projectile, float direction)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		base.Initialize(projectile, direction);
		_finder.Initialize(projectile);
		_target = null;
		_speed = Vector2.zero;
		_rotation = Quaternion.Euler(0f, 0f, direction);
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		if (time >= _delay)
		{
			UpdateTarget();
			UpdateDirection(deltaTime);
		}
		_speed += _acceleration * base.directionVector * deltaTime;
		Vector2 normalized = ((Vector2)(ref _speed)).normalized;
		float magnitude = ((Vector2)(ref _speed)).magnitude;
		if (magnitude > _maxSpeed)
		{
			_speed = _maxSpeed * normalized;
		}
		return (normalized, magnitude * base.projectile.speedMultiplier);
	}

	private void UpdateTarget()
	{
		if ((!((Object)(object)_target != (Object)null) || (!((Object)(object)_target.character == (Object)null) && _target.character.health.dead)) && !((Object)(object)_finder.range == (Object)null))
		{
			_target = _finder.Find();
		}
	}

	private void UpdateDirection(float deltaTime)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_target == (Object)null))
		{
			Bounds bounds = _target.collider.bounds;
			Vector3 val = ((Bounds)(ref bounds)).center - ((Component)this).transform.position;
			float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
			switch (_rotateMethod)
			{
			case RotateMethod.Constant:
				_rotation = Quaternion.RotateTowards(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _rotateSpeed * 100f * deltaTime);
				break;
			case RotateMethod.Lerp:
				_rotation = Quaternion.Lerp(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _rotateSpeed * deltaTime);
				break;
			case RotateMethod.Slerp:
				_rotation = Quaternion.Slerp(_rotation, Quaternion.AngleAxis(num, Vector3.forward), _rotateSpeed * deltaTime);
				break;
			}
			base.direction = ((Quaternion)(ref _rotation)).eulerAngles.z;
			base.directionVector = Vector2.op_Implicit(_rotation * Vector3.right);
		}
	}
}
