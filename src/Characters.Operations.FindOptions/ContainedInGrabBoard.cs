using System;
using UnityEngine;
using Utils;

namespace Characters.Operations.FindOptions;

[Serializable]
public class ContainedInGrabBoard : ICondition
{
	[SerializeField]
	private GrabBoard _grabBoard;

	public bool Satisfied(Character character)
	{
		return _grabBoard.HasInTargets(character);
	}
}
