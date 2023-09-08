using System.Collections;
using Characters.Actions;
using Runnables.Triggers;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class RunTriggerAction : Behaviour
{
	[Trigger.Subcomponent]
	[SerializeField]
	private Trigger _trigger;

	[SerializeField]
	private Action _successAction;

	[SerializeField]
	private Action _failAction;

	private Stat.Values _stoppingResistanceStat = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.StoppingResistance, 0.0));

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		Character character = controller.character;
		if (character.type == Character.Type.Adventurer)
		{
			character.stat.AttachValues(_stoppingResistanceStat);
		}
		Action action = ((!_trigger.IsSatisfied()) ? _failAction : _successAction);
		if ((Object)(object)action != (Object)null)
		{
			if (!action.TryStart())
			{
				base.result = Result.Fail;
				yield break;
			}
			while (action.running && base.result == Result.Doing)
			{
				yield return null;
			}
			if (character.type == Character.Type.Adventurer)
			{
				character.stat.DetachValues(_stoppingResistanceStat);
			}
		}
		base.result = Result.Success;
	}
}
