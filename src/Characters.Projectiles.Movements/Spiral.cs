using System;
using UnityEngine;

namespace Characters.Projectiles.Movements;

public class Spiral : Movement
{
	private enum RotateMethod
	{
		Constant,
		Lerp,
		Slerp
	}

	[Serializable]
	private class MoveInfo
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<MoveInfo>
		{
		}

		[SerializeField]
		private AnimationCurve _curve;

		[SerializeField]
		private float _length;

		[SerializeField]
		private float _targetSpeed;

		[SerializeField]
		private bool _clearHitHistory;

		public AnimationCurve curve => _curve;

		public float length => _length;

		public float targetSpeed => _targetSpeed;

		public bool clearHitHistory => _clearHitHistory;
	}

	[Serializable]
	private class RotationInfo
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<RotationInfo>
		{
		}

		[SerializeField]
		private float _length;

		[SerializeField]
		private float _rotateSpeed;

		[SerializeField]
		private float _angle;

		[SerializeField]
		private RotateMethod _rotateMethod;

		public float length => _length;

		public float rotateSpeed => _rotateSpeed;

		public float angle => _angle;

		public RotateMethod rotateMethod => _rotateMethod;
	}

	[SerializeField]
	private float _delay;

	[SerializeField]
	private float _startSpeed;

	[SerializeField]
	private MoveInfo.Reorderable _moveInfos;

	[SerializeField]
	private RotationInfo.Reorderable _rotationInfos;

	private int _currentMoveIndex;

	private Quaternion _rotation;

	public override void Initialize(IProjectile projectile, float direction)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		base.Initialize(projectile, direction);
		_rotation = Quaternion.Euler(0f, 0f, direction);
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		if (time >= _delay)
		{
			float num = time;
			for (int i = 0; i < ((ReorderableArray<RotationInfo>)_rotationInfos).values.Length; i++)
			{
				RotationInfo rotationInfo = ((ReorderableArray<RotationInfo>)_rotationInfos).values[i];
				if (num > rotationInfo.length)
				{
					num -= rotationInfo.length;
				}
				else
				{
					UpdateDirection(deltaTime, rotationInfo);
				}
			}
		}
		float num2 = _startSpeed;
		for (int j = 0; j < ((ReorderableArray<MoveInfo>)_moveInfos).values.Length; j++)
		{
			MoveInfo moveInfo = ((ReorderableArray<MoveInfo>)_moveInfos).values[j];
			if (time > moveInfo.length)
			{
				num2 = moveInfo.targetSpeed;
				time -= moveInfo.length;
				continue;
			}
			if (moveInfo.clearHitHistory && _currentMoveIndex != j)
			{
				base.projectile.ClearHitHistroy();
			}
			_currentMoveIndex = j;
			float num3 = num2 + (moveInfo.targetSpeed - num2) * moveInfo.curve.Evaluate(time / moveInfo.length);
			return (base.directionVector, num3 * base.projectile.speedMultiplier);
		}
		return (base.directionVector, num2 * base.projectile.speedMultiplier);
	}

	private void UpdateDirection(float deltaTime, RotationInfo info)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		float num = base.direction + info.angle;
		switch (info.rotateMethod)
		{
		case RotateMethod.Constant:
			_rotation = Quaternion.RotateTowards(_rotation, Quaternion.AngleAxis(num, Vector3.forward), info.rotateSpeed * 100f * deltaTime);
			break;
		case RotateMethod.Lerp:
			_rotation = Quaternion.Lerp(_rotation, Quaternion.AngleAxis(num, Vector3.forward), info.rotateSpeed * deltaTime);
			break;
		case RotateMethod.Slerp:
			_rotation = Quaternion.Slerp(_rotation, Quaternion.AngleAxis(num, Vector3.forward), info.rotateSpeed * deltaTime);
			break;
		}
		base.direction = ((Quaternion)(ref _rotation)).eulerAngles.z;
		base.directionVector = Vector2.op_Implicit(_rotation * Vector3.right);
	}
}
