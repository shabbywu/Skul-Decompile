using System.Collections;
using Characters;
using Runnables;
using UnityEngine;

namespace CutScenes.Shots.Sequences;

public class CharacterLootAt : Sequence
{
	[SerializeField]
	private Runnables.Target _target;

	[SerializeField]
	private Transform _point;

	public override IEnumerator CRun()
	{
		Character.LookingDirection lookingDirection = ((!(_point.position.x - ((Component)_target.character).transform.position.x > 0f)) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		_target.character.lookingDirection = lookingDirection;
		yield return null;
	}
}
