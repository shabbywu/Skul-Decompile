using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class RunActions : Behaviour
{
	[SerializeField]
	private Characters.Actions.Action[] _actions;

	private Stat.Values _stoppingResistanceStat = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.StoppingResistance, 0.0));

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		Character character = controller.character;
		if (character.type == Character.Type.Adventurer)
		{
			character.stat.AttachValues(_stoppingResistanceStat);
		}
		if (_actions == null)
		{
			throw new NullReferenceException();
		}
		if (_actions.Length == 0)
		{
			Debug.LogError((object)"The number of actions is 0");
		}
		Characters.Actions.Action[] actions = _actions;
		foreach (Characters.Actions.Action action in actions)
		{
			while (controller.character.stunedOrFreezed)
			{
				yield return null;
			}
			action.TryStart();
			while (action.running)
			{
				yield return null;
			}
		}
		if (character.type == Character.Type.Adventurer)
		{
			character.stat.DetachValues(_stoppingResistanceStat);
		}
		base.result = Result.Success;
	}
}
