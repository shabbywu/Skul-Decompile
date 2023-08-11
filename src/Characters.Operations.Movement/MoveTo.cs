using System.Collections;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Movement;

public sealed class MoveTo : CharacterOperation
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Curve _curve;

	[SerializeField]
	private bool _setPostionOnEnd;

	[SerializeField]
	private Transform _rotateTransform;

	private CoroutineReference _coroutineReference;

	public override void Run(Character owner)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)owner.movement == (Object)null))
		{
			Vector2 val = Vector2.op_Implicit(((Component)_target).transform.position - ((Component)owner).transform.position);
			float magnitude = ((Vector2)(ref val)).magnitude;
			Vector2 val2 = ((Vector2)(ref val)).normalized * magnitude;
			float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
			if (val.x < 0f)
			{
				num -= 180f;
			}
			if ((Object)(object)_rotateTransform != (Object)null)
			{
				_rotateTransform.rotation = Quaternion.Euler(0f, 0f, num);
			}
			if (_curve.duration > 0f)
			{
				((CoroutineReference)(ref _coroutineReference)).Stop();
				_coroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)owner.movement, CMove(owner, _curve, val2));
			}
			else
			{
				Characters.Movements.Movement movement = owner.movement;
				movement.force += val2;
			}
		}
	}

	internal IEnumerator CMove(Character character, Curve curve, Vector2 distance)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		float t = 0f;
		float amountBefore = 0f;
		Vector2.op_Implicit(((Component)character).transform.position);
		for (; t < curve.duration; t += ((ChronometerBase)character.chronometer.animation).deltaTime)
		{
			if ((Object)(object)character == (Object)null || !character.liveAndActive)
			{
				yield break;
			}
			float num = curve.Evaluate(t);
			Characters.Movements.Movement movement = character.movement;
			movement.force += distance * (num - amountBefore);
			amountBefore = num;
			yield return null;
		}
		if ((Object)(object)_rotateTransform != (Object)null)
		{
			_rotateTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		if (_setPostionOnEnd)
		{
			character.movement.force = Vector2.op_Implicit(Vector3.zero);
			((Component)character).transform.position = ((Component)_target).transform.position;
		}
	}

	public override void Stop()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_rotateTransform != (Object)null)
		{
			_rotateTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		((CoroutineReference)(ref _coroutineReference)).Stop();
	}
}
