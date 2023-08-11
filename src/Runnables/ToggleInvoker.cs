using System.Collections;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables;

public class ToggleInvoker : MonoBehaviour
{
	[SerializeField]
	[Trigger.Subcomponent]
	private Trigger _trigger;

	[SerializeField]
	private Runnable _execute;

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
	}

	private IEnumerator CRun()
	{
		while (true)
		{
			if (!_trigger.IsSatisfied())
			{
				yield return null;
				continue;
			}
			_execute.Run();
			while (_trigger.IsSatisfied())
			{
				yield return null;
			}
		}
	}
}
