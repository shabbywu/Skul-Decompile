using System.Collections;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Customs;

public class MoveToDestination : CharacterOperation
{
	[SerializeField]
	private Transform _destination;

	[SerializeField]
	private Curve _curve;

	private CoroutineReference _coroutineReference;

	public override void Run(Character owner)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)owner.movement == (Object)null))
		{
			Vector2 val = Vector2.op_Implicit(_destination.position - ((Component)owner).transform.position);
			if (_curve.duration > 0f)
			{
				_coroutineReference.Stop();
				_coroutineReference = ((MonoBehaviour)(object)owner).StartCoroutineWithReference(CMove(owner, _curve, val));
			}
			else
			{
				Characters.Movements.Movement movement = owner.movement;
				movement.force += val;
			}
		}
	}

	private IEnumerator CMove(Character character, Curve curve, Vector2 distance)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float t = 0f;
		float amountBefore = 0f;
		for (; t < curve.duration; t += character.chronometer.animation.deltaTime)
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
		_coroutineReference.Stop();
	}
}
