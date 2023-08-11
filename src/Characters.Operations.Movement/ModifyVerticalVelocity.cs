using System.Collections;
using UnityEngine;

namespace Characters.Operations.Movement;

public class ModifyVerticalVelocity : CharacterOperation
{
	private enum Method
	{
		Add,
		Set
	}

	[SerializeField]
	private float _amount;

	[SerializeField]
	private Method _method;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private Curve _curve;

	private Character _owner;

	private CoroutineReference _coroutineReference;

	public override void Run(Character owner)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		_owner = owner;
		if (_curve.duration > 0f)
		{
			switch (_method)
			{
			case Method.Add:
				_coroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)owner, CRun(owner));
				break;
			case Method.Set:
				_coroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)owner, CRunWithIgnoreGravity(owner));
				break;
			}
		}
		else
		{
			switch (_method)
			{
			case Method.Add:
				owner.movement.verticalVelocity += _amount;
				break;
			case Method.Set:
				owner.movement.verticalVelocity = _amount;
				break;
			}
		}
	}

	private IEnumerator CRunWithIgnoreGravity(Character character)
	{
		character.movement.ignoreGravity.Attach((object)this);
		yield return CRun(character);
		character.movement.ignoreGravity.Detach((object)this);
	}

	private IEnumerator CRun(Character character)
	{
		float t = 0f;
		float normAmountBefore = 0f;
		for (; t < _curve.duration; t += ((ChronometerBase)character.chronometer.animation).deltaTime)
		{
			float num = _curve.Evaluate(t);
			switch (_method)
			{
			case Method.Add:
				character.movement.verticalVelocity += _amount * (num - normAmountBefore);
				break;
			case Method.Set:
				character.movement.verticalVelocity = _amount;
				break;
			}
			normAmountBefore = num;
			yield return null;
		}
	}

	public override void Stop()
	{
		if ((Object)(object)_owner == (Object)(object)((CoroutineReference)(ref _coroutineReference)).monoBehaviour)
		{
			((CoroutineReference)(ref _coroutineReference)).Stop();
		}
		if (_method == Method.Set)
		{
			Character owner = _owner;
			if (owner != null)
			{
				owner.movement.ignoreGravity.Detach((object)this);
			}
		}
	}
}
