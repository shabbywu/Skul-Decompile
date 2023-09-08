using Characters.Actions;
using UnityEngine;

namespace Runnables;

public sealed class RunAction : Runnable
{
	[SerializeField]
	private Action _action;

	public override void Run()
	{
		_action.TryStart();
	}
}
