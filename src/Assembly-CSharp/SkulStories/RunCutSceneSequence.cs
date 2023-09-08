using System.Collections;
using CutScenes.Shots;
using UnityEngine;

namespace SkulStories;

public class RunCutSceneSequence : Sequence
{
	[CutScenes.Shots.Sequence.Subcomponent]
	[SerializeField]
	private CutScenes.Shots.Sequence.Subcomponents _sequences;

	public override IEnumerator CRun()
	{
		yield return _sequences.CRun();
	}
}
