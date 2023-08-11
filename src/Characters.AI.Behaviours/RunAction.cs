using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class RunAction : Behaviour
{
	[SerializeField]
	private Action _action;

	private Stat.Values _stoppingResistanceStat = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.StoppingResistance, 0.0));

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		Character character = controller.character;
		if (character.type == Character.Type.Adventurer)
		{
			character.stat.AttachValues(_stoppingResistanceStat);
		}
		if (!_action.TryStart())
		{
			base.result = Result.Fail;
			yield break;
		}
		while (_action.running && base.result == Result.Doing)
		{
			yield return null;
		}
		if (character.type == Character.Type.Adventurer)
		{
			character.stat.DetachValues(_stoppingResistanceStat);
		}
		base.result = Result.Success;
	}
}
