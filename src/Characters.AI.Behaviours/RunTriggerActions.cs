using System.Collections;
using Characters.Actions;
using Runnables.Triggers;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class RunTriggerActions : Behaviour
{
	[Trigger.Subcomponent]
	[SerializeField]
	private Trigger _trigger;

	[SerializeField]
	private Action[] _successActions;

	[SerializeField]
	private Action[] _failActions;

	private Stat.Values _stoppingResistanceStat = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.StoppingResistance, 0.0));

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		Character character = controller.character;
		if (character.type == Character.Type.Adventurer)
		{
			character.stat.AttachValues(_stoppingResistanceStat);
		}
		Action[] array = ((!_trigger.IsSatisfied()) ? _failActions : _successActions);
		if (array != null && array.Length != 0)
		{
			Action[] array2 = array;
			foreach (Action action in array2)
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
		}
		base.result = Result.Success;
	}
}
