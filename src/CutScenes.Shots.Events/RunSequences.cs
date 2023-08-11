using UnityEngine;

namespace CutScenes.Shots.Events;

public sealed class RunSequences : Event
{
	[Sequence.Subcomponent]
	[SerializeField]
	private Sequence.Subcomponents _sequences;

	public override void Run()
	{
		((MonoBehaviour)this).StartCoroutine(_sequences.CRun());
	}
}
