using System.Collections;
using Runnables;
using UnityEditor;
using UnityEngine;

namespace SkulStories;

public class SkulStory : Runnable
{
	[SerializeField]
	[Event.Subcomponent]
	private Event.Subcomponents _onStart;

	[UnityEditor.Subcomponent(typeof(SequenceInfo))]
	[SerializeField]
	private SequenceInfo.Subcomponents _sequence;

	[Event.Subcomponent]
	[SerializeField]
	private Event.Subcomponents _onEnd;

	public override void Run()
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
	}

	private IEnumerator CRun()
	{
		_onStart.Run();
		yield return _sequence.CRun();
		_onEnd.Run();
	}
}
