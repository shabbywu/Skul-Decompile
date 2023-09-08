using System.Collections;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Movement;

public class TeleportOverTime : CharacterOperation
{
	private Characters.Movements.Movement.Config _staticMovementConfig = new Characters.Movements.Movement.Config(Characters.Movements.Movement.Config.Type.Static);

	[SerializeField]
	private Curve _curve;

	[Information("0이상이면 텔레포트 실패 시 거리 1마다 재시도, 이동에 특별한 문제가 없는 한 0으로 유지.", InformationAttribute.InformationType.Info, false)]
	[SerializeField]
	private float _maxRetryDistance;

	[Header("Start")]
	[Information("둘 다 비워두면 캐릭터의 현재 위치 사용", InformationAttribute.InformationType.Info, false)]
	[SerializeField]
	private Collider2D _startRange;

	[SerializeField]
	private Transform _startPoint;

	[Header("End")]
	[SerializeField]
	private Collider2D _endRange;

	[SerializeField]
	private Transform _endPoint;

	private Character _currentTarget;

	public override void Run(Character owner)
	{
		if (_curve.duration == 0f)
		{
			Debug.LogError((object)"The duration of the curve is zero. Set it higher than 0 or use teleport operation.");
			return;
		}
		Stop();
		((MonoBehaviour)this).StartCoroutine(CTeleportOverTime(owner));
	}

	private Vector2 GetPosition(Collider2D collider, Transform transform)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)collider != (Object)null)
		{
			return MMMaths.RandomPointWithinBounds(collider.bounds);
		}
		if ((Object)(object)transform != (Object)null)
		{
			return Vector2.op_Implicit(transform.position);
		}
		Debug.LogError((object)"Both of collider and transform are null on teleport over time.");
		return Vector2.zero;
	}

	private void Teleport(Character target, Vector2 destination)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (_maxRetryDistance > 0f)
		{
			target.movement.controller.Teleport(destination, _maxRetryDistance);
		}
		else
		{
			target.movement.controller.Teleport(destination);
		}
	}

	private IEnumerator CTeleportOverTime(Character target)
	{
		Transform transform = (((Object)(object)_startPoint == (Object)null) ? ((Component)target).transform : _startPoint);
		Vector2 startPosition = GetPosition(_startRange, transform);
		Vector2 endPosition = GetPosition(_endRange, _endPoint);
		float time = 0f;
		float duration = _curve.duration;
		target.movement.configs.Add(int.MaxValue, _staticMovementConfig);
		while (time < duration)
		{
			time += target.chronometer.master.deltaTime;
			Vector2 destination = Vector2.LerpUnclamped(startPosition, endPosition, time / duration);
			Teleport(target, destination);
			yield return null;
		}
		Teleport(target, endPosition);
		target.movement.configs.Remove(_staticMovementConfig);
		_currentTarget = null;
	}

	public override void Stop()
	{
		((MonoBehaviour)this).StopAllCoroutines();
		if ((Object)(object)_currentTarget != (Object)null)
		{
			_currentTarget.movement.configs.Remove(_staticMovementConfig);
			_currentTarget = null;
		}
	}
}
