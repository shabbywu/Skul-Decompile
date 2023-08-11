using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class RunActionWhileArrive : Behaviour
{
	[SerializeField]
	private Action _action;

	[SerializeField]
	private Transform _dest;

	[SerializeField]
	private float _maxTime = 10f;

	[SerializeField]
	private float _epsilon = 0.5f;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		yield return CUpdate(controller);
		base.result = Result.Success;
	}

	private IEnumerator CUpdate(AIController controller)
	{
		float elapsed = 0f;
		if (!_action.TryStart())
		{
			base.result = Result.Fail;
			yield break;
		}
		while (_action.running && base.result == Result.Doing && elapsed <= _maxTime && !TryMove(controller.character))
		{
			yield return null;
			elapsed += ((ChronometerBase)controller.character.chronometer.master).deltaTime;
		}
	}

	private bool TryMove(Character character)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float num = _dest.position.x - ((Component)character).transform.position.x;
		if (Mathf.Abs(num) < _epsilon)
		{
			return false;
		}
		int num2 = ((!(num > 0f)) ? 180 : 0);
		character.movement.Move((float)num2);
		return true;
	}
}
