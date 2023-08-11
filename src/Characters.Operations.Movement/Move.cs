using System.Collections;
using Characters.Controllers;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Movement;

public class Move : CharacterOperation
{
	private const float directionThreshold = 0.66f;

	[SerializeField]
	private bool _useDashDistanceStat;

	[SerializeField]
	private float _movementSpeedFactor;

	[SerializeField]
	private Force _force;

	[SerializeField]
	private Curve _curve;

	[SerializeField]
	private bool _needDirectionInput = true;

	private CoroutineReference _coroutineReference;

	public override void Run(Character owner)
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)owner.movement == (Object)null)
		{
			return;
		}
		if (_needDirectionInput)
		{
			PlayerInput component = ((Component)owner).GetComponent<PlayerInput>();
			if ((Object)(object)component != (Object)null && ((owner.lookingDirection == Character.LookingDirection.Left && component.direction.x >= -0.66f) || (owner.lookingDirection == Character.LookingDirection.Right && component.direction.x <= 0.66f)))
			{
				return;
			}
		}
		float extraPower = 0f;
		if (_movementSpeedFactor > 0f)
		{
			float num = Mathf.Abs((float)owner.stat.Get(Stat.Category.Constant, Stat.Kind.MovementSpeed));
			float num2 = Mathf.Abs((float)owner.stat.GetFinal(Stat.Kind.MovementSpeed));
			extraPower = Mathf.Max(0f, num2 - num) * _curve.duration * _movementSpeedFactor;
		}
		Vector2 val = _force.Evaluate(owner, extraPower);
		if (_useDashDistanceStat)
		{
			val *= (float)owner.stat.GetFinal(Stat.Kind.DashDistance);
		}
		if (_curve.duration > 0f)
		{
			((CoroutineReference)(ref _coroutineReference)).Stop();
			_coroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)owner.movement, CMove(owner, _curve, val));
		}
		else
		{
			Characters.Movements.Movement movement = owner.movement;
			movement.force += val;
		}
	}

	internal static IEnumerator CMove(Character character, Curve curve, Vector2 distance)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float t = 0f;
		float amountBefore = 0f;
		for (; t < curve.duration; t += ((ChronometerBase)character.chronometer.animation).deltaTime)
		{
			if ((Object)(object)character == (Object)null)
			{
				break;
			}
			if (!character.liveAndActive)
			{
				break;
			}
			float num = curve.Evaluate(t);
			Characters.Movements.Movement movement = character.movement;
			movement.force += distance * (num - amountBefore);
			amountBefore = num;
			yield return null;
		}
	}

	public override void Stop()
	{
		((CoroutineReference)(ref _coroutineReference)).Stop();
	}
}
