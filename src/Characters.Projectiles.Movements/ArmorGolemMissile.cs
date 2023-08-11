using System;
using UnityEngine;

namespace Characters.Projectiles.Movements;

public sealed class ArmorGolemMissile : Movement
{
	[Serializable]
	private class SpeedInfo
	{
		[SerializeField]
		internal float startSpeed = 1f;

		[SerializeField]
		internal float targetSpeed;

		[SerializeField]
		internal AnimationCurve curve;

		[SerializeField]
		internal float easingTime;
	}

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
	private Vector2 _targetOffset;

	[Header("Wait For Targeting")]
	[SerializeField]
	private AnimationCurve _waitCurve;

	[SerializeField]
	private float _wait;

	[Header("Targeting Roatation")]
	[SerializeField]
	private bool _startRotationOnAppear;

	[SerializeField]
	private RotateMethod _rotateMethod;

	[SerializeField]
	private float _appearRotateSpeed = 300f;

	[SerializeField]
	private float _targetingRotateSpeed = 300f;

	[Header("Speed")]
	[SerializeField]
	private SpeedInfo _onDelay;

	[SerializeField]
	private SpeedInfo _onTarget;

	[SerializeField]
	private Transform _missileTransform;

	private float _lookTargetAngle;

	private Quaternion _rotationCache;

	private Target _target;

	private Vector3 _targetGroundPosition;

	public override void Initialize(IProjectile projectile, float direction)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		base.Initialize(projectile, direction);
		_finder.Initialize(projectile);
		_target = null;
		_rotationCache = Quaternion.Euler(0f, 0f, direction);
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		if (time < _wait)
		{
			UpdateTarget();
		}
		UpdateLookDirection(deltaTime, (time < _delay) ? _appearRotateSpeed : _targetingRotateSpeed);
		if (time >= _delay || _startRotationOnAppear)
		{
			UpdateMoveDirection();
		}
		float num = ((time < _delay) ? GetSpeed(_onDelay, time) : GetSpeed(_onTarget, time - _delay));
		if (time >= _delay && time < _wait)
		{
			num *= 1f - _waitCurve.Evaluate(time / _wait);
		}
		return (base.directionVector, num * base.projectile.speedMultiplier);
	}

	private float GetSpeed(SpeedInfo info, float time)
	{
		return info.startSpeed + (info.targetSpeed - info.startSpeed) * info.curve.Evaluate(time / info.easingTime);
	}

	private void UpdateTarget()
	{
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_finder.range != (Object)null)
		{
			_target = _finder.Find();
			if ((Object)(object)_target != (Object)null && (Object)(object)_target.character != (Object)null)
			{
				RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(((Component)_target.character).transform.position), Vector2.down, float.PositiveInfinity, LayerMask.op_Implicit(Layers.groundMask));
				if (RaycastHit2D.op_Implicit(val))
				{
					_targetGroundPosition = Vector2.op_Implicit(((RaycastHit2D)(ref val)).point);
				}
				else if ((Object)(object)_target.character.movement.controller.collisionState.lastStandingCollider != (Object)null)
				{
					Bounds bounds = _target.character.movement.controller.collisionState.lastStandingCollider.bounds;
					_targetGroundPosition = Vector2.op_Implicit(new Vector2(((Component)_target).transform.position.x, ((Bounds)(ref bounds)).center.y));
				}
				else
				{
					_targetGroundPosition = ((Component)_target).transform.position;
				}
				_targetGroundPosition = new Vector3(_targetGroundPosition.x + _targetOffset.x, _targetGroundPosition.y + _targetOffset.y, 0f);
			}
		}
		Vector3 val2 = _targetGroundPosition - ((Component)this).transform.position;
		_lookTargetAngle = Mathf.Atan2(val2.y, val2.x) * 57.29578f;
	}

	private void UpdateMoveDirection()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		base.direction = ((Quaternion)(ref _rotationCache)).eulerAngles.z;
		base.directionVector = Vector2.op_Implicit(_rotationCache * Vector3.right);
	}

	private void UpdateLookDirection(float deltaTime, float rotateSpeed)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		switch (_rotateMethod)
		{
		case RotateMethod.Constant:
			_rotationCache = Quaternion.RotateTowards(_rotationCache, Quaternion.AngleAxis(_lookTargetAngle, Vector3.forward), rotateSpeed * 100f * deltaTime);
			break;
		case RotateMethod.Lerp:
			_rotationCache = Quaternion.Lerp(_rotationCache, Quaternion.AngleAxis(_lookTargetAngle, Vector3.forward), rotateSpeed * deltaTime);
			break;
		case RotateMethod.Slerp:
			_rotationCache = Quaternion.Slerp(_rotationCache, Quaternion.AngleAxis(_lookTargetAngle, Vector3.forward), rotateSpeed * deltaTime);
			break;
		}
		_missileTransform.rotation = _rotationCache;
	}
}
