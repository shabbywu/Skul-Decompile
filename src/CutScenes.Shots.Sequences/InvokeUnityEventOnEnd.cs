using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CutScenes.Shots.Sequences;

public sealed class InvokeUnityEventOnEnd : Sequence
{
	[Subcomponent]
	[SerializeField]
	private Subcomponents _sequences;

	[SerializeField]
	private UnityEvent _event;

	public override IEnumerator CRun()
	{
		yield return _sequences.CRun();
		UnityEvent @event = _event;
		if (@event != null)
		{
			@event.Invoke();
		}
	}
}
