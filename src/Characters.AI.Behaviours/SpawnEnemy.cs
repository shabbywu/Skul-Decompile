using System;
using System.Collections;
using Characters.Actions;
using Characters.Actions.Constraints;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class SpawnEnemy : Behaviour
{
	private enum Condition
	{
		CooldownConstraint,
		Cleared
	}

	private Enum a;

	[SerializeField]
	private Characters.Actions.Action _spawnAction;

	[SerializeField]
	private CooldownConstraint _cooldownConstraint;

	[SerializeField]
	private Master _master;

	[SerializeField]
	private Condition _condition;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		if (!isSatisfy())
		{
			base.result = Result.Done;
			yield break;
		}
		_spawnAction.TryStart();
		while (_spawnAction.running)
		{
			yield return null;
		}
		base.result = Result.Done;
	}

	private bool isSatisfy()
	{
		return _condition switch
		{
			Condition.Cleared => _master.isCleared(), 
			Condition.CooldownConstraint => _cooldownConstraint.canUse, 
			_ => true, 
		};
	}
}
