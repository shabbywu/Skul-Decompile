using Characters.Controllers;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Movement;

public class DualMove : CharacterOperation
{
	private const float directionThreshold = 0.66f;

	[SerializeField]
	private bool _useDashDistanceStat;

	[SerializeField]
	private float _movementSpeedFactor1;

	[SerializeField]
	private Force _force1;

	[SerializeField]
	private Curve _curve1;

	[SerializeField]
	private float _movementSpeedFactor2;

	[SerializeField]
	private Force _force2;

	[SerializeField]
	private Curve _curve2;

	[SerializeField]
	private bool _needDirectionInput = true;

	private CoroutineReference _coroutineReference1;

	private CoroutineReference _coroutineReference2;

	public override void Run(Character owner)
	{
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
		float extraPower2 = 0f;
		float extraPower3 = 0f;
		if (_movementSpeedFactor1 > 0f || _movementSpeedFactor2 > 0f)
		{
			float num = Mathf.Abs((float)owner.stat.GetFinal(Stat.Kind.MovementSpeed));
			float num2 = Mathf.Abs((float)owner.stat.Get(Stat.Category.Constant, Stat.Kind.MovementSpeed));
			float num3 = Mathf.Max(0f, num - num2);
			extraPower2 = num3 * _curve1.duration * _movementSpeedFactor1;
			extraPower3 = num3 * _curve2.duration * _movementSpeedFactor2;
		}
		TriggerMove(_force1, _curve1, extraPower2, ref _coroutineReference1);
		TriggerMove(_force2, _curve2, extraPower3, ref _coroutineReference2);
		void TriggerMove(Force force, Curve curve, float extraPower, ref CoroutineReference coroutineReference)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = force.Evaluate(owner);
			if (_useDashDistanceStat)
			{
				val *= (float)owner.stat.GetFinal(Stat.Kind.DashDistance);
			}
			if (_curve1.duration > 0f)
			{
				coroutineReference.Stop();
				coroutineReference = ((MonoBehaviour)(object)owner).StartCoroutineWithReference(Move.CMove(owner, curve, val));
			}
			else
			{
				Characters.Movements.Movement movement = owner.movement;
				movement.force += val;
			}
		}
	}

	public override void Stop()
	{
		_coroutineReference1.Stop();
		_coroutineReference2.Stop();
	}
}
