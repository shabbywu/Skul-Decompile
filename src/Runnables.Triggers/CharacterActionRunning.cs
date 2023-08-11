using Characters.Actions;
using UnityEngine;

namespace Runnables.Triggers;

public class CharacterActionRunning : Trigger
{
	[SerializeField]
	private Action _action;

	protected override bool Check()
	{
		if (_action.running)
		{
			return true;
		}
		return false;
	}
}
