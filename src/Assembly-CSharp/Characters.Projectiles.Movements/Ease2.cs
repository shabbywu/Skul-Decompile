using System;
using UnityEngine;

namespace Characters.Projectiles.Movements;

public class Ease2 : Movement
{
	[Serializable]
	private class Info
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<Info>
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

	[SerializeField]
	private float _startSpeed;

	[SerializeField]
	private Info.Reorderable _infos;

	private int _currentIndex;

	public override void Initialize(IProjectile projectile, float direction)
	{
		base.Initialize(projectile, direction);
		_currentIndex = 0;
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		float num = _startSpeed;
		for (int i = 0; i < _infos.values.Length; i++)
		{
			Info info = _infos.values[i];
			if (time > info.length)
			{
				num = info.targetSpeed;
				time -= info.length;
				continue;
			}
			if (info.clearHitHistory && _currentIndex != i)
			{
				base.projectile.ClearHitHistroy();
			}
			_currentIndex = i;
			float num2 = num + (info.targetSpeed - num) * info.curve.Evaluate(time / info.length);
			return (base.directionVector, num2 * base.projectile.speedMultiplier);
		}
		return (base.directionVector, num * base.projectile.speedMultiplier);
	}
}
