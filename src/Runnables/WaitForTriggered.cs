using System.Collections;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables;

public class WaitForTriggered : CRunnable
{
	[SerializeField]
	[Trigger.Subcomponent]
	private Trigger _trigger;

	public override IEnumerator CRun()
	{
		while (!_trigger.IsSatisfied())
		{
			yield return null;
		}
	}
}
