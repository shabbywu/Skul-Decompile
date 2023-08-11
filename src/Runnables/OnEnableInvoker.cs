using Runnables.Triggers;
using UnityEngine;

namespace Runnables;

public sealed class OnEnableInvoker : MonoBehaviour
{
	[SerializeField]
	[Trigger.Subcomponent]
	private Trigger _trigger;

	[SerializeField]
	private Runnable _execute;

	private void OnEnable()
	{
		if (_trigger.IsSatisfied())
		{
			_execute.Run();
		}
	}
}
